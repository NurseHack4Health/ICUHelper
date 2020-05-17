/* Insert new supplements for patients, variables
@patient_id = is obtained from the user's id
@supplies_id = is obtained from the selector of the list of exiting supplies
@indications = you get them from the text box where you indicate how to apply the supply
*/
INSERT INTO nursehackdb.dbo.patient_supplies
    (patient_id, supplies_id, indications, date_beg, date_end)
VALUES(@patien_id, @supplies_id, @indications, GETDATE(), null);

/*Insert new users
All this variables are providen by the users when they create the profile
@full_name text box
@phone - text box (number)
@emergency_contact - text box
@phone_emergency_contact - text box (number)
@gender_id - selector 
@date_of_birth - text box (YYYY-MM-DD)
@identification_number  - text box (number)
@identificaton_type - selector

WARNING: ACTUALLY THE DB HAS SOME TRIGGERS THAT AUTOMATICAL CREATE THE NEW USERS AS PATIENTS
*/

INSERT INTO nursehackdb.dbo.users
    (full_name, phone, emergency_contact, phone_emergency_contact, gender_id, date_of_birth, identification_number, identificaton_type)
VALUES(@full_name,@phone, @emergency_contact, @phone_emergency_contact, @gender_id, @date_of_birth, @identification_number, @identificaton_type);

/*
TRIGGER IN USERS
BEGIN
    declare @id_usert int
    declare @vrol_type_id int
    SELECT @vrol_type_id=5;
    SELECT @id_usert = id
    from inserted;
    INSERT INTO dbo.patient (user_id,date_in,condition_id, using_ventilator)
    VALUES (@id_usert, GETDATE(), 1, 0);
    INSERT INTO dbo.rol (rol_type_id,user_id)
    VALUES  (@vrol_type_id, @id_usert);
END

*/

/* insert a patient, if the user already exists
@user_id = who already exist 
@condition_id = from the select condition default = 1 (Good)
@using_ventilator = from the select using_ventilator default = 0 (false) non using. 
*/

INSERT INTO nursehackdb.dbo.patient
    (user_id, date_in, condition_id, release_date, using_ventilator)
VALUES(@user_id, GETDATE(), @condition_id, null, @using_ventilator);

/* To Update the patient state
@condition_id = new condition
@patient_id = get from the app patient_id
*/
UPDATE nursehackdb.dbo.patient
SET condition_id=@condition_id
WHERE id=@patien_id; 



/* To assign patients to doctors or nurses
@doctor_id - selector
@patient_id - get patient_id from the app
*/
INSERT INTO nursehackdb.dbo.patient_assignation
    (doctor_id, patient_id)
VALUES(@doctor_id, @patient_id);

/* Ventilators Avalible query
*/

WITH
    ventilator_inventory
    as
    (/* consults how many ventilators are in the inventory
    */
        SELECT s.inventory as avalible,
            1 as org
        FROM dbo.supplies s
        WHERE name like ('%Ventilator%')
    ),

    ventilator_in_use
    as
    (/* consults how many ventilators are in use
    */
        SELECT COUNT (id) as in_use,
            1 as org
        from dbo.patient p
        WHERE p.using_ventilator=1
    )
/* consults how many ventilators are in the inventory ans saves in the @vents_avalible var.  
*/
DECLARE @vents_avalible int
SELECT @vents_avalible = avalible-in_use
FROM ventilator_inventory vi
    JOIN ventilator_in_use vu on (vi.org=vu.org)

