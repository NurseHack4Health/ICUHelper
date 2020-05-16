/* trigger that creates a patient when a new users is created. 
*/
CREATE   TRIGGER PatientTrigger ON nursehackdb.dbo.users
AFTER INSERT
AS
BEGIN
    declare @id_usert int
    declare @vrol_type_id int
    declare @history_number int
    SELECT @vrol_type_id=5;
    /*5 para paciente, 1 para nurse, 2 para doctor, 3 administration, 4 root*/
    SELECT @id_usert = id
    from inserted;
    SELECT @history_number = identification_number
    from inserted;
    INSERT INTO dbo.patient
        (user_id,date_in,condition_id, using_ventilator,history_number)
    VALUES
        (@id_usert, GETDATE(), 1, 0, @history_number);
    INSERT INTO dbo.rol
        (rol_type_id,user_id)
    VALUES
        (@vrol_type_id, @id_usert);
END
;

/* trigger that changes patient's condition when ventilator flag is on 
*/

CREATE OR ALTER TRIGGER conditionTrigger ON nursehackdb.dbo.patient
AFTER UPDATE
AS
BEGIN
    IF UPDATE (using_ventilator)
	BEGIN
        DECLARE @id_usert int
        SELECT @id_usert = id
        FROM inserted;
        UPDATE nursehackdb.dbo.patient SET condition_id=4 WHERE user_id=@id_usert;
    END
END
;

/* trigger that add new supplies when we change the number of inventory stock
*/

CREATE OR ALTER TRIGGER AddInventoryTrigger ON nursehackdb.dbo.supplies
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE (inventory) 
BEGIN
        UPDATE s
    SET inventory = (new.inventory + old.inventory)
    FROM nursehackdb.dbo.supplies AS s
            JOIN inserted AS new ON new.sku = s.sku
            JOIN deleted AS old ON old.sku = s.sku
    WHERE new.inventory <> old.inventory;
    END
END
;

/* Procedure to insert supplies
*/

CREATE PROCEDURE dbo.validate_supplies_insert
    @sku_in varchar(250),
    @name_in varchar(700),
    @description_in varchar(MAX),
    @inventory_in int,
    @id_out int OUT
AS
BEGIN

    SELECT @id_out = s.id
    FROM dbo.supplies s
    WHERE s.sku=@sku_in;
    SET @id_out=IsNull(@id_out,0);

    IF (@id_out=0)
	BEGIN
        INSERT INTO nursehackdb.dbo.supplies
            (sku, name, description, inventory)
        VALUES(@sku_in, @name_in, @description_in, @inventory_in);
        SELECT @id_out=id
        FROM nursehackdb.dbo.supplies
        WHERE sku=@sku_in;
        RETURN @id_out;
    END
    ELSE
    BEGIN
        UPDATE nursehackdb.dbo.supplies SET inventory=@inventory_in WHERE sku=@sku_in;
        RETURN @id_out;
    END
END
;



