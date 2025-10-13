# Local Deployment Guide

This guide walks you through running the Flow Solution Workbench application on a development
workstation with a self-contained SQL Server LocalDB instance that hosts randomised demo data.
The setup is intentionally lightweight so you can explore the UI without access to the
enterprise infrastructure that the production build expects.

## 1. Prerequisites

1. **Visual Studio 2022 (or newer)** with the “.NET desktop development” workload installed.
2. **SQL Server Express LocalDB** (ships with Visual Studio). You can also use SQL Server Express
   or a full SQL Server instance if you prefer, but the scripts below assume LocalDB.
3. **SQL Server command-line utilities** (`SqlLocalDB` and `sqlcmd`). These are installed with
   recent Visual Studio builds; if you deselected them you can add the “SQL Server Express LocalDB”
   and “SQL Server Data Tools” components via the Visual Studio Installer.
4. (Optional) **SQL Server Management Studio (SSMS)** if you prefer a graphical client to inspect
   the database.

## 2. Create the Local Database

1. Open a **Developer PowerShell for Visual Studio** prompt, `cd` to the repository root, and run the
   helper script that automates the entire database setup. Either PowerShell 5 (`powershell.exe`) or
   PowerShell 7 (`pwsh`) works:

   ```powershell
   powershell -ExecutionPolicy Bypass -File scripts/setup-localdb.ps1
   ```

   The script will create (or start) the `FlowWorkbenchLocal` LocalDB instance, provision the
   `FlowWorkbenchLocal` database, and run both schema and seed scripts.

   * To customise the instance or database names run `Get-Help ./scripts/setup-localdb.ps1 -Full`
     and pass the relevant parameters, e.g. `pwsh -File scripts/setup-localdb.ps1 -InstanceName Demo`.
   * If you prefer PowerShell 7, swap `powershell` for `pwsh` in the command above.

2. If you prefer to run the steps manually, create and start the LocalDB instance and then execute
   the SQL scripts individually:

   ```powershell
   sqllocaldb create "FlowWorkbenchLocal"
   sqllocaldb start "FlowWorkbenchLocal"
   sqlcmd -S "(localdb)\\FlowWorkbenchLocal" -i Database/localdb/001_create_schema.sql
   sqlcmd -S "(localdb)\\FlowWorkbenchLocal" -d FlowWorkbenchLocal -i Database/localdb/002_seed_data.sql
   ```

   The scripts create a pared-down schema that mimics the tables/views the application touches
   during start-up and seeds them with randomised yet coherent demo records (orders, shortages,
   notifications, etc.). You can re-run `002_seed_data.sql` at any time to refresh the demo data.

## 3. Update the Application Configuration

The stock project points to an internal production database and LDAP server. Replace those values
with the local connection string:

1. Open `app.config` inside Visual Studio.
2. Update the `FlowConnectionString` entry to point to the LocalDB instance (this repository already
   contains an example value). If you changed the instance or database names when running the setup
   script, mirror that here:

   ```xml
   <add name="Flow_Solution_Workbenches.My.MySettings.FLOWConnectionString"
        connectionString="Data Source=(localdb)\FlowWorkbenchLocal;Initial Catalog=FlowWorkbenchLocal;Integrated Security=True"
        providerName="System.Data.SqlClient" />
   ```

3. If you do not need Oracle connectivity or LDAP integration you can leave the placeholder
   connection strings as-is. The application uses the SQL Server connection for the features that
   were enabled for the demo dataset.

## 4. Build and Run the Application

1. Double-click `Flow Solution Workbenches.vbproj` to open the project in Visual Studio.
2. Ensure **Debug** configuration and **Any CPU** are selected.
3. Press <kbd>F5</kbd> to build and run. Log in with your Windows user – the first launch will seed
   your user profile in the local database using the scripts above.

   * The main dashboard will load data from the `salesOrderLinesView`, `mrpShortagesView`, `poQueueView`
     and related tables populated in the seed script.
   * Demo users `local.user` and `buyer.user` are seeded so you can impersonate different roles by
     temporarily changing `Environment.UserName` via Visual Studio’s **Debug > Start Without Debugging**
     command-line arguments if required.

## 5. Optional: Regenerate Demo Data

You can reseed the dataset at any time by re-running the helper script:

```powershell
pwsh -File scripts/setup-localdb.ps1
```

or, to keep the schema intact while refreshing the sample data only, execute just the seed script:

```powershell
sqlcmd -S "(localdb)\FlowWorkbenchLocal" -d FlowWorkbenchLocal -i Database/localdb/002_seed_data.sql
```

Feel free to tweak the INSERT statements in `002_seed_data.sql` to craft scenario-specific data
(e.g. higher backlogs or additional comments).

## 6. Running on non-Windows Hosts

The Workbench is a Windows Forms application that targets .NET Framework 4.8. You can develop and
run it only on Windows. If you are on macOS or Linux, use a Windows virtual machine, WSL2 with a
Windows 11 host, or a remote Windows development box with Visual Studio installed. The SQL Server
database can still run locally in a Docker container on those platforms, but the application itself
must execute on Windows.

## 7. Troubleshooting

* If you receive a login error ensure the LocalDB instance is running (`sqllocaldb info FlowWorkbenchLocal`).
* Clearing user-specific grid settings: delete rows from `warehouse_grid_user_size_order` and rerun
  the seed script to restore the defaults.
* The demo schema only includes the tables required for the core workbench screens. If you navigate
  to a rarely used feature that relies on an omitted table/view you can add it following the pattern
  established in `001_create_schema.sql`.

Happy exploring!
