/* Insert new supplements for patients, variables
@patient_id = is obtained from the user's id
@supplies_id = is obtained from the selector of the list of exiting supplies
@indications = you get them from the text box where you indicate how to apply the supply
*/

SELECT @patien_id=id
FROM nursehackdb.dbo.patient
WHERE user_id=@user_id;

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

SELECT @patien_id=id
FROM nursehackdb.dbo.patient
WHERE user_id=@user_id;

UPDATE nursehackdb.dbo.patient
SET condition_id=@condition_id
WHERE id=@patien_id; 



/* To assign patients to doctors or nurses
@doctor_id - selector
@patient_id - get patient_id from the app
*/

SELECT @patien_id=id
FROM nursehackdb.dbo.patient
WHERE user_id=@user_id;

SELECT @doctor_id=id
FROM nursehackdb.dbo.doctor
WHERE user_id=@user_id;

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
SELECT avalible-in_use
FROM ventilator_inventory vi
    JOIN ventilator_in_use vu on (vi.org=vu.org)

/* consults the patients asigned to a doctor and shows the patient name, history number, and condition.  
*/
SELECT u2.full_name,
    p.history_number,
    c.value as p_condition
FROM dbo.patient_assignation pa
    JOIN dbo.patient p on (p.id=pa.patient_id)
    JOIN dbo.doctor d on (pa.doctor_id=d.id)
    JOIN dbo.users u on (d.user_id=u.id)
    JOIN dbo.users u2 on (u2.id=p.user_id)
    JOIN dbo.conditions c on (p.condition_id=c.id)
WHERE u.id=@user_id;

/**/

WITH
    supplies_inventory
    as
    (
        /* consults how many supplies are in the inventory
    */
        SELECT s.id,
            s.sku,
            s.name,
            s.inventory as deposit
        FROM dbo.supplies s
    ),

    supplies_in_use
    as
    (
        /* consults how many supplies are in use
    */
        SELECT ps.supplies_id,
            COUNT (id) as in_use
        FROM dbo.patient_supplies ps
        GROUP BY ps.supplies_id
    )
/* consults how many supplies avalible an saves in the supplies_avalible var.  
*/
SELECT si.sku,
    si.name,
    SUM(si.deposit - suin.in_use) as supplies_avalible
FROM supplies_inventory si
    JOIN supplies_in_use suin on (suin.supplies_id=si.id)
GROUP BY si.sku,si.name;

/* calls the powerbi token
*/

SELECT pt.token
FROM dbo.powerbi_token 

/* to update if a patient need a ventilator you need get the patient id linked to that user id
*/

SELECT @patient_id = id
FROM dbo.patient
WHERE user_id=@userId

UPDATE dbo.Patient SET using_ventilator = @using_ventilator Where id = @patient_id;

/*Query who calls the email from the persons who has the adminitration role. 
*/

SELECT ua.email, 
       u.full_name
FROM rol r
    JOIN rol_type rt on (rt.id=r.rol_type_id)
    JOIN users u on (u.id=r.user_id)
    JOIN user_auth ua on (ua.user_id=u.id)
WHERE rt.id=3; 

