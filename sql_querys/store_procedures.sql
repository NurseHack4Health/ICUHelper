
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
END; 

/*
Store procedure that updates the last userauth created to assign the password and email
*/

CREATE PROCEDURE dbo.new_user_auth
    @password_in varchar(50),
    @email_in varchar(100),
    @userid_out int OUT
AS
BEGIN
    SELECT @userid_out = ua.user_id
    FROM dbo.user_auth ua
    WHERE email=''
    SET @userid_out=IsNull(@userid_out,0);
    IF (@userid_out > 0)
	BEGIN
        UPDATE nursehackdb.dbo.user_auth SET password=@password_in, email=@email_in WHERE user_id=@userid_out;
        RETURN @userid_out;
    END
	ELSE
	BEGIN
        RETURN	@userid_out;
    END
END