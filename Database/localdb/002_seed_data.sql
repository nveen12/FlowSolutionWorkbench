USE FlowWorkbenchLocal;
GO

DELETE FROM dbo.warehouse_grid_user_size_order;
DELETE FROM dbo.warehouse_grid_columns;
DELETE FROM dbo.warehouseUserNotifications;
DELETE FROM dbo.comments;
DELETE FROM dbo.warehouseUserFilters;
DELETE FROM dbo.warehouseFilterTemplates;
DELETE FROM dbo.warehouseWatchList;
DELETE FROM dbo.warehouseUsersLogins;
DELETE FROM dbo.whRoleRightsOnForms;
DELETE FROM dbo.warehouse_user_roles;
DELETE FROM dbo.whUserRoles;
DELETE FROM dbo.whUserFactories;
DELETE FROM dbo.warehouse_users;
DELETE FROM dbo.whOrganizationDefinition;
DELETE FROM dbo.salesOrderLinesView;
DELETE FROM dbo.poQueueView;
DELETE FROM dbo.wipQueueView;
DELETE FROM dbo.saveMRPdemand;
DELETE FROM dbo.mrpShortagesView;
DELETE FROM dbo.warehouse_lovs;
DELETE FROM dbo.items;
DELETE FROM dbo.tblWork;
DELETE FROM dbo.supplyDemandMRPView;
DELETE FROM dbo.MSCsupplyDemandMRPView;
GO

SET IDENTITY_INSERT dbo.whOrganizationDefinition ON;
INSERT INTO dbo.whOrganizationDefinition (organizationId, organizationName, organizationLanguage, currentInstance, transformationRun)
VALUES (255, N'Local Demo Plant', N'EN', N'LOCAL', GETDATE()),
       (256, N'Local Demo Plant 2', N'DE', N'LOCAL', DATEADD(DAY, -1, GETDATE()));
SET IDENTITY_INSERT dbo.whOrganizationDefinition OFF;

SET IDENTITY_INSERT dbo.whUserRoles ON;
INSERT INTO dbo.whUserRoles (roleId, roleName)
VALUES (1, N'Planner'),
       (2, N'Buyer');
SET IDENTITY_INSERT dbo.whUserRoles OFF;

INSERT INTO dbo.warehouse_users (user_Name, standard_operating_unit_id, standard_organization_id, [language], userDefaultOrganization, lastRoleSelected, firstName, lastName, eMail, outOfOffice, enabled)
VALUES (N'local.user', 254, 255, N'EN', 255, 1, N'Local', N'User', N'local.user@example.com', 0, 1),
       (N'buyer.user', 254, 255, N'EN', 255, 2, N'Buyer', N'User', N'buyer.user@example.com', 0, 1);

INSERT INTO dbo.warehouse_user_roles (userName, roleId)
VALUES (N'local.user', 1),
       (N'buyer.user', 2);

INSERT INTO dbo.whRoleRightsOnForms (roleID, formName, controlName, visibility, enabled)
VALUES (1, N'frmWorkbenchProjects', N'BuyToolStripMenuItem', 1, 1),
       (1, N'frmWorkbenchProjects', N'SellToolStripMenuItem', 1, 1),
       (1, N'frmWorkbenchProjects', N'ViewToolStripMenuItem', 1, 1),
       (2, N'frmWorkbenchProjects', N'BuyToolStripMenuItem', 1, 1),
       (2, N'frmWorkbenchProjects', N'SellToolStripMenuItem', 0, 0);

INSERT INTO dbo.whUserFactories (userName, organizationID)
VALUES (N'local.user', 255),
       (N'buyer.user', 255);

INSERT INTO dbo.warehouseWatchList (userName, entityName, entityKey)
VALUES (N'local.user', N'salesOrderLinesView', N'1001-1'),
       (N'local.user', N'mrpShortagesView', N'210-1');

INSERT INTO dbo.warehouseFilterTemplates (filterName, organizationID, filterString)
VALUES (N'Local - High Value Orders', 255, N'AND extended_price &gt; 25000'),
       (N'Local - Late Shipments', 255, N'AND schedule_ship_date &lt; GETDATE()');

INSERT INTO dbo.warehouseUserFilters (filterName, userName, filterString)
VALUES (N'My Urgent Orders', N'local.user', N'AND no_of_problems &gt; 0'),
       (N'Upcoming Deliveries', N'local.user', N'AND schedule_ship_date BETWEEN GETDATE() AND DATEADD(DAY, 14, GETDATE())');

INSERT INTO dbo.salesOrderLinesView (
    header_id, line_no, organization_id, demand_id, order_no, order_type, party_name, schedule_ship_date, creation_date_coline,
    extended_price, no_of_problems, promise_date, inventory_item_id, attribute1, buyer, planner_code, item_no, description,
    currentLeadtime, possibleLeadtime)
VALUES
    (1001, 1, 255, 5001, N'SO-1001', N'Standard', N'Pleuger Pumps', DATEADD(DAY, 7, GETDATE()), DATEADD(DAY, -10, GETDATE()),
     32500.00, 2, DATEADD(DAY, 5, GETDATE()), 9001, N'USR001', N'local.user', N'PLN', N'PMP-9001', N'High pressure pump', 25, 18),
    (1002, 1, 255, 5002, N'SO-1002', N'Project', N'Industrial Motors Inc', DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, -20, GETDATE()),
     18000.00, 1, DATEADD(DAY, -1, GETDATE()), 9002, N'USR002', N'buyer.user', N'BYR', N'MTR-2110', N'AC Motor Assembly', 30, 20),
    (1003, 1, 255, 5003, N'SO-1003', N'Spare', N'Global Services', DATEADD(DAY, 21, GETDATE()), DATEADD(DAY, -5, GETDATE()),
     4200.00, 0, DATEADD(DAY, 14, GETDATE()), 9003, N'USR003', N'local.user', N'PLN', N'SPR-3300', N'Spare seal kit', 12, 15);

INSERT INTO dbo.poQueueView (PO_HEADER_ID, LINE_NO, SHIPMENT_NO, releaseNo, organization_id)
VALUES (2001, 1, 1, 0, 255),
       (2002, 1, 1, 1, 255);

INSERT INTO dbo.wipQueueView (WIP_ENTITY_ID, OPERATION_SEQ_NO, organization_id)
VALUES (3001, 10, 255),
       (3002, 20, 255);

INSERT INTO dbo.saveMRPdemand (ORDER_NO, ORGANIZATION_ID, LINE_NO, INVENTORY_ITEM_ID, reasonCode, end_demand_id, buyer, planner_code)
VALUES (1001, 255, 1, 9001, N'COMP', 5001, N'local.user', N'PLN'),
       (1002, 255, 1, 9002, N'MATL', 5002, N'buyer.user', N'BYR');

INSERT INTO dbo.mrpShortagesView (OrderNoLine, organization_id, end_demand_id, buyer, planner_code, OP1, item_no, description, delay, purchasingReviewDate)
VALUES (N'SO-1001-1', 255, 5001, N'local.user', N'PLN', N'ASSEMBLY', N'PMP-9001', N'Pump Assembly', 5, DATEADD(DAY, -3, GETDATE())),
       (N'SO-1002-1', 255, 5002, N'buyer.user', N'BYR', N'FAB', N'MTR-2110', N'Motor Fabrication', 12, DATEADD(DAY, -1, GETDATE()));

INSERT INTO dbo.warehouse_lovs (ORDER_TYPE, lov_value_char, calculation_value)
VALUES (N'Standard', N'LOV_STD', 1.25),
       (N'Project', N'LOV_PRJ', 1.50);

INSERT INTO dbo.items (inventory_item_id, organization_id, item_no, description, critical_component_flag)
VALUES (9001, 255, N'PMP-9001', N'High pressure pump', 1),
       (9002, 255, N'MTR-2110', N'AC Motor Assembly', 0),
       (9003, 255, N'SPR-3300', N'Spare seal kit', 0);

INSERT INTO dbo.tblWork (title, owner)
VALUES (N'Assemble pump order backlog', N'local.user'),
       (N'Coordinate supplier follow-up', N'buyer.user');

INSERT INTO dbo.supplyDemandMRPView (organization_id, item_no, demand_id, order_no, order_type, end_demand_id, description, due_date, quantity)
VALUES (255, N'PMP-9001', 6001, N'SO-1001', N'Standard', 5001, N'Demand for pump', DATEADD(DAY, 3, GETDATE()), 4),
       (255, N'MTR-2110', 6002, N'SO-1002', N'Project', 5002, N'Demand for motor', DATEADD(DAY, 10, GETDATE()), 2);

INSERT INTO dbo.MSCsupplyDemandMRPView (organization_id, item_no, demand_id, order_no, order_type, end_demand_id, description, due_date, quantity)
VALUES (255, N'PMP-9001', 7001, N'SO-1001', N'Standard', 5001, N'MSC alt demand', DATEADD(DAY, 5, GETDATE()), 4);

INSERT INTO dbo.warehouseUserNotifications (headerID, lineID, notificationTo)
VALUES (1001, 1, N'local.user'),
       (1002, 1, N'buyer.user');

INSERT INTO dbo.comments (header_id, line_id, entity_id, description, created_by, organizationId, shipment_no, releaseNo)
VALUES (1001, 1, N'salesOrderLinesView', N'Expedite due to customer escalation.', N'local.user', 255, 1, 0),
       (1002, 1, N'salesOrderLinesView', N'Supplier confirmed new delivery date.', N'buyer.user', 255, 1, 1);

INSERT INTO dbo.warehouseUsersLogins (loginID, creationDate, userName, [type])
VALUES (1, DATEADD(HOUR, -5, GETDATE()), N'local.user', N'Login'),
       (2, DATEADD(HOUR, -3, GETDATE()), N'local.user', N'Logout'),
       (3, DATEADD(HOUR, -2, GETDATE()), N'buyer.user', N'Login');

INSERT INTO dbo.warehouse_grid_columns (entity_name, columnHeader, columnWidth, columnOrder, user_visibility)
VALUES (N'salesOrderLinesView', N'Order', 150, 1, 1),
       (N'salesOrderLinesView', N'Customer', 180, 2, 1),
       (N'salesOrderLinesView', N'Value', 100, 3, 1),
       (N'mrpShortagesView', N'Item', 140, 1, 1),
       (N'mrpShortagesView', N'Delay', 80, 2, 1),
       (N'poQueueView', N'PO Number', 140, 1, 1),
       (N'wipQueueView', N'Job', 140, 1, 1),
       (N'supplyDemandMRPView', N'Demand', 140, 1, 1),
       (N'MSCsupplyDemandMRPView', N'Demand', 140, 1, 1);

INSERT INTO dbo.warehouse_grid_user_size_order (user_name, entity_name, column_id, column_size, column_order, visibility)
SELECT N'local.user', entity_name, id, columnWidth, columnOrder, user_visibility
FROM dbo.warehouse_grid_columns;

