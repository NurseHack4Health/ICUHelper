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
END;

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
END;

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
END;


