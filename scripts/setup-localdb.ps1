<#
.SYNOPSIS
Sets up the Flow Solution Workbench demo database on a SQL Server LocalDB instance.

.DESCRIPTION
Ensures a LocalDB instance is available, creates the FlowWorkbenchLocal database (or a
custom name supplied through parameters), and runs the bundled schema and seed scripts.

.PARAMETER InstanceName
Name of the LocalDB instance to create or reuse.

.PARAMETER DatabaseName
Name of the database that will host the demo schema and data.

.PARAMETER SqlCmd
Path to the sqlcmd executable. Defaults to the sqlcmd found on PATH.

.PARAMETER SchemaScript
Path to the SQL file that creates the schema.

.PARAMETER SeedScript
Path to the SQL file that seeds the demo data.

.EXAMPLE
pwsh -File scripts/setup-localdb.ps1

Runs the script with the default instance and database names.

.EXAMPLE
powershell -ExecutionPolicy Bypass -File scripts/setup-localdb.ps1 -InstanceName Demo -DatabaseName FlowDemo

Creates (or reuses) a LocalDB instance called Demo and provisions a FlowDemo database.
#>

[CmdletBinding()]
param(
    [string]$InstanceName = "FlowWorkbenchLocal",
    [string]$DatabaseName = "FlowWorkbenchLocal",
    [string]$SqlCmd = "sqlcmd",
    [string]$SchemaScript = "Database/localdb/001_create_schema.sql",
    [string]$SeedScript = "Database/localdb/002_seed_data.sql"
)

function Invoke-LocalDbCommand {
    param(
        [Parameter(Mandatory=$true)][string]$Arguments
    )

    $exe = "sqllocaldb"
    Write-Verbose "Running: $exe $Arguments"
    $process = Start-Process -FilePath $exe -ArgumentList $Arguments -NoNewWindow -PassThru -Wait -ErrorAction SilentlyContinue
    if ($process.ExitCode -ne 0) {
        throw "sqllocaldb command failed: $Arguments (exit code $($process.ExitCode))"
    }
}

function Invoke-SqlScript {
    param(
        [Parameter(Mandatory=$true)][string]$ScriptPath,
        [string]$Database = 'master'
    )

    if (-not (Test-Path $ScriptPath)) {
        throw "SQL script not found: $ScriptPath"
    }

    $args = @(
        '-S', "(localdb)\\$InstanceName",
        '-d', $Database,
        '-i', (Resolve-Path $ScriptPath)
    )

    Write-Verbose "Executing $ScriptPath against (localdb)\\$InstanceName (database: $Database)"
    & $SqlCmd @args
    if ($LASTEXITCODE -ne 0) {
        throw "sqlcmd failed for $ScriptPath with exit code $LASTEXITCODE"
    }
}

try {
    if (-not (Get-Command sqllocaldb -ErrorAction SilentlyContinue)) {
        throw "sqllocaldb.exe not found. Install SQL Server Express LocalDB or ensure it is on the PATH."
    }
    if (-not (Get-Command $SqlCmd -ErrorAction SilentlyContinue)) {
        throw "sqlcmd not found. Install the SQL Server command-line utilities or specify the path via -SqlCmd."
    }

    Write-Host "Ensuring LocalDB instance '$InstanceName' exists..."
    & sqllocaldb info $InstanceName 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Instance not found. Creating..."
        Invoke-LocalDbCommand "create `"$InstanceName`""
    }

    Write-Host "Starting LocalDB instance '$InstanceName'..."
    Invoke-LocalDbCommand "start `"$InstanceName`""

    Write-Host "Creating database '$DatabaseName' if missing..."
    $createDb = @"
IF DB_ID('$DatabaseName') IS NULL
BEGIN
    PRINT 'Creating database $DatabaseName';
    EXEC('CREATE DATABASE [$DatabaseName]');
END
"@
    $createDbPath = New-TemporaryFile
    Set-Content -LiteralPath $createDbPath -Value $createDb -Encoding UTF8
    try {
        Invoke-SqlScript -ScriptPath $createDbPath -Database 'master'
    }
    finally {
        Remove-Item $createDbPath -Force
    }

    Write-Host "Running schema script..."
    Invoke-SqlScript -ScriptPath $SchemaScript -Database $DatabaseName

    Write-Host "Running seed script..."
    Invoke-SqlScript -ScriptPath $SeedScript -Database $DatabaseName

    Write-Host "Local database setup complete. Update app.config to use connection string:"
    Write-Host "  Data Source=(localdb)\\$InstanceName;Initial Catalog=$DatabaseName;Integrated Security=True"
}
catch {
    Write-Error $_
    exit 1
}
