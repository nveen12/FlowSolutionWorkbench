IF DB_ID(N'FlowWorkbenchLocal') IS NULL
BEGIN
    CREATE DATABASE FlowWorkbenchLocal;
END
GO
USE FlowWorkbenchLocal;
GO

IF OBJECT_ID(N'dbo.userInRollesAllRightsView', N'V') IS NOT NULL DROP VIEW dbo.userInRollesAllRightsView;
IF OBJECT_ID(N'dbo.userInRolesView', N'V') IS NOT NULL DROP VIEW dbo.userInRolesView;
IF OBJECT_ID(N'dbo.whUserOrganizationsView', N'V') IS NOT NULL DROP VIEW dbo.whUserOrganizationsView;
GO

DROP TABLE IF EXISTS dbo.warehouse_grid_user_size_order;
DROP TABLE IF EXISTS dbo.warehouse_grid_columns;
DROP TABLE IF EXISTS dbo.warehouseUserNotifications;
DROP TABLE IF EXISTS dbo.comments;
DROP TABLE IF EXISTS dbo.warehouseUserFilters;
DROP TABLE IF EXISTS dbo.warehouseFilterTemplates;
DROP TABLE IF EXISTS dbo.warehouseWatchList;
DROP TABLE IF EXISTS dbo.warehouseUsersLogins;
DROP TABLE IF EXISTS dbo.whRoleRightsOnForms;
DROP TABLE IF EXISTS dbo.warehouse_user_roles;
DROP TABLE IF EXISTS dbo.whUserRoles;
DROP TABLE IF EXISTS dbo.whUserFactories;
DROP TABLE IF EXISTS dbo.warehouse_users;
DROP TABLE IF EXISTS dbo.whOrganizationDefinition;
DROP TABLE IF EXISTS dbo.salesOrderLinesView;
DROP TABLE IF EXISTS dbo.poQueueView;
DROP TABLE IF EXISTS dbo.wipQueueView;
DROP TABLE IF EXISTS dbo.saveMRPdemand;
DROP TABLE IF EXISTS dbo.mrpShortagesView;
DROP TABLE IF EXISTS dbo.warehouse_lovs;
DROP TABLE IF EXISTS dbo.items;
DROP TABLE IF EXISTS dbo.tblWork;
DROP TABLE IF EXISTS dbo.supplyDemandMRPView;
DROP TABLE IF EXISTS dbo.MSCsupplyDemandMRPView;
GO

CREATE TABLE dbo.whOrganizationDefinition (
    organizationId INT IDENTITY(1,1) PRIMARY KEY,
    organizationName NVARCHAR(200) NOT NULL,
    organizationLanguage NVARCHAR(50) NOT NULL,
    currentInstance NVARCHAR(50) NULL,
    transformationRun DATETIME NULL
);

CREATE TABLE dbo.warehouse_users (
    user_Name NVARCHAR(128) NOT NULL PRIMARY KEY,
    standard_operating_unit_id INT NOT NULL,
    standard_organization_id INT NOT NULL,
    [language] NVARCHAR(50) NOT NULL,
    userDefaultOrganization INT NOT NULL,
    lastRoleSelected INT NOT NULL,
    firstName NVARCHAR(100) NULL,
    lastName NVARCHAR(100) NULL,
    eMail NVARCHAR(200) NULL,
    outOfOffice BIT NULL,
    outUnitilDate DATE NULL,
    outFromDate DATE NULL,
    enabled BIT NULL
);

CREATE TABLE dbo.whUserRoles (
    roleId INT IDENTITY(1,1) PRIMARY KEY,
    roleName NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.warehouse_user_roles (
    userName NVARCHAR(128) NOT NULL,
    roleId INT NOT NULL,
    PRIMARY KEY (userName, roleId),
    CONSTRAINT FK_wur_users FOREIGN KEY (userName) REFERENCES dbo.warehouse_users(user_Name),
    CONSTRAINT FK_wur_roles FOREIGN KEY (roleId) REFERENCES dbo.whUserRoles(roleId)
);

CREATE TABLE dbo.whRoleRightsOnForms (
    rightId INT IDENTITY(1,1) PRIMARY KEY,
    roleID INT NOT NULL,
    formName NVARCHAR(200) NOT NULL,
    controlName NVARCHAR(200) NOT NULL,
    visibility BIT NOT NULL,
    enabled BIT NOT NULL,
    CONSTRAINT FK_role_right_role FOREIGN KEY (roleID) REFERENCES dbo.whUserRoles(roleId)
);

CREATE TABLE dbo.warehouseUserNotifications (
    notificationId INT IDENTITY(1,1) PRIMARY KEY,
    headerID INT NOT NULL,
    lineID INT NOT NULL,
    notificationTo NVARCHAR(128) NOT NULL,
    createdOn DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE dbo.comments (
    commentId INT IDENTITY(1,1) PRIMARY KEY,
    header_id INT NOT NULL,
    line_id INT NOT NULL,
    entity_id NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX) NOT NULL,
    creation_date DATETIME NOT NULL DEFAULT GETDATE(),
    created_by NVARCHAR(128) NOT NULL,
    forecast BIT NOT NULL DEFAULT 0,
    organizationId INT NULL,
    shipment_no INT NULL,
    releaseNo INT NULL
);

CREATE TABLE dbo.warehouseUserFilters (
    filterId INT IDENTITY(1,1) PRIMARY KEY,
    filterName NVARCHAR(200) NOT NULL,
    userName NVARCHAR(128) NOT NULL,
    filterString NVARCHAR(MAX) NOT NULL
);

CREATE TABLE dbo.warehouseFilterTemplates (
    filterTemplateId INT IDENTITY(1,1) PRIMARY KEY,
    filterName NVARCHAR(200) NOT NULL,
    organizationID INT NOT NULL,
    filterString NVARCHAR(MAX) NOT NULL
);

CREATE TABLE dbo.warehouseWatchList (
    watchID INT IDENTITY(1,1) PRIMARY KEY,
    userName NVARCHAR(128) NOT NULL,
    entityName NVARCHAR(100) NOT NULL,
    entityKey NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.warehouse_grid_columns (
    id INT IDENTITY(1,1) PRIMARY KEY,
    entity_name NVARCHAR(200) NOT NULL,
    columnHeader NVARCHAR(200) NOT NULL,
    columnWidth INT NOT NULL,
    columnOrder INT NOT NULL,
    user_visibility BIT NOT NULL
);

CREATE TABLE dbo.warehouse_grid_user_size_order (
    user_name NVARCHAR(128) NOT NULL,
    entity_name NVARCHAR(200) NOT NULL,
    column_id INT NOT NULL,
    column_size INT NOT NULL,
    column_order INT NOT NULL,
    visibility BIT NOT NULL,
    PRIMARY KEY (user_name, entity_name, column_id)
);

CREATE TABLE dbo.warehouseUsersLogins (
    loginID INT PRIMARY KEY,
    creationDate DATETIME NOT NULL,
    userName NVARCHAR(128) NOT NULL,
    [type] NVARCHAR(20) NOT NULL
);

CREATE TABLE dbo.salesOrderLinesView (
    lineKey INT IDENTITY(1,1) PRIMARY KEY,
    header_id INT NOT NULL,
    line_no INT NOT NULL,
    organization_id INT NOT NULL,
    demand_id INT NOT NULL,
    order_no NVARCHAR(50) NOT NULL,
    order_type NVARCHAR(50) NOT NULL,
    party_name NVARCHAR(200) NOT NULL,
    schedule_ship_date DATE NULL,
    creation_date_coline DATE NULL,
    extended_price DECIMAL(18,2) NOT NULL,
    no_of_problems INT NULL,
    promise_date DATE NULL,
    inventory_item_id INT NOT NULL,
    attribute1 NVARCHAR(200) NULL,
    buyer NVARCHAR(128) NULL,
    planner_code NVARCHAR(50) NULL,
    item_no NVARCHAR(100) NULL,
    description NVARCHAR(200) NULL,
    currentLeadtime INT NULL,
    possibleLeadtime INT NULL
);

CREATE TABLE dbo.poQueueView (
    poLineKey INT IDENTITY(1,1) PRIMARY KEY,
    PO_HEADER_ID INT NOT NULL,
    LINE_NO INT NOT NULL,
    SHIPMENT_NO INT NOT NULL,
    releaseNo INT NULL,
    organization_id INT NOT NULL
);

CREATE TABLE dbo.wipQueueView (
    wipKey INT IDENTITY(1,1) PRIMARY KEY,
    WIP_ENTITY_ID INT NOT NULL,
    OPERATION_SEQ_NO INT NOT NULL,
    organization_id INT NOT NULL
);

CREATE TABLE dbo.saveMRPdemand (
    demandKey INT IDENTITY(1,1) PRIMARY KEY,
    ORDER_NO INT NOT NULL,
    ORGANIZATION_ID INT NOT NULL,
    LINE_NO INT NOT NULL,
    INVENTORY_ITEM_ID INT NOT NULL,
    reasonCode NVARCHAR(100) NULL,
    end_demand_id INT NOT NULL,
    buyer NVARCHAR(128) NULL,
    planner_code NVARCHAR(50) NULL
);

CREATE TABLE dbo.mrpShortagesView (
    shortageKey INT IDENTITY(1,1) PRIMARY KEY,
    OrderNoLine NVARCHAR(100) NOT NULL,
    organization_id INT NOT NULL,
    end_demand_id INT NOT NULL,
    buyer NVARCHAR(128) NULL,
    planner_code NVARCHAR(50) NULL,
    OP1 NVARCHAR(100) NULL,
    item_no NVARCHAR(100) NULL,
    description NVARCHAR(200) NULL,
    delay INT NULL,
    purchasingReviewDate DATE NULL
);

CREATE TABLE dbo.warehouse_lovs (
    lov_id INT IDENTITY(1,1) PRIMARY KEY,
    ORDER_TYPE NVARCHAR(50) NOT NULL,
    lov_value_char NVARCHAR(50) NOT NULL,
    calculation_value DECIMAL(18,2) NOT NULL
);

CREATE TABLE dbo.items (
    inventory_item_id INT NOT NULL,
    organization_id INT NOT NULL,
    item_no NVARCHAR(100) NULL,
    description NVARCHAR(200) NULL,
    critical_component_flag BIT NOT NULL,
    PRIMARY KEY (inventory_item_id, organization_id)
);

CREATE TABLE dbo.tblWork (
    workId INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(200) NOT NULL,
    createdOn DATETIME NOT NULL DEFAULT GETDATE(),
    owner NVARCHAR(128) NOT NULL
);

CREATE TABLE dbo.whUserFactories (
    userName NVARCHAR(128) NOT NULL,
    organizationID INT NOT NULL,
    PRIMARY KEY (userName, organizationID)
);

CREATE TABLE dbo.supplyDemandMRPView (
    supplyKey INT IDENTITY(1,1) PRIMARY KEY,
    organization_id INT NOT NULL,
    item_no NVARCHAR(100) NOT NULL,
    demand_id INT NOT NULL,
    order_no NVARCHAR(50) NOT NULL,
    order_type NVARCHAR(50) NOT NULL,
    end_demand_id INT NOT NULL,
    description NVARCHAR(200) NULL,
    due_date DATE NULL,
    quantity INT NOT NULL
);

CREATE TABLE dbo.MSCsupplyDemandMRPView (
    supplyKey INT IDENTITY(1,1) PRIMARY KEY,
    organization_id INT NOT NULL,
    item_no NVARCHAR(100) NOT NULL,
    demand_id INT NOT NULL,
    order_no NVARCHAR(50) NOT NULL,
    order_type NVARCHAR(50) NOT NULL,
    end_demand_id INT NOT NULL,
    description NVARCHAR(200) NULL,
    due_date DATE NULL,
    quantity INT NOT NULL
);
GO

CREATE VIEW dbo.userInRolesView
AS
SELECT wur.userName AS user_Name,
       wur.roleId,
       r.roleName,
       u.firstName,
       u.lastName
FROM dbo.warehouse_user_roles AS wur
JOIN dbo.whUserRoles AS r ON r.roleId = wur.roleId
JOIN dbo.warehouse_users AS u ON u.user_Name = wur.userName;
GO

CREATE VIEW dbo.userInRollesAllRightsView
AS
SELECT u.user_Name,
       f.formName,
       f.controlName,
       f.visibility,
       f.enabled
FROM dbo.warehouse_users AS u
JOIN dbo.warehouse_user_roles AS wur ON wur.userName = u.user_Name
JOIN dbo.whRoleRightsOnForms AS f ON f.roleID = wur.roleId;
GO

CREATE VIEW dbo.whUserOrganizationsView
AS
SELECT u.user_Name,
       u.userDefaultOrganization,
       u.[language] AS organizationLanguage,
       CASE WHEN u.lastRoleSelected = 1 THEN 'MRP' ELSE 'STANDARD' END AS planningType
FROM dbo.warehouse_users AS u;
GO
