TODO: 
- [ ] Models (with Entity)
- [ ] Backend

DATABASE STRUCTURE:
Objections:
- singula table names
- self id's should be "id"
- snake case

User:
- id
- username
- email
- password
- role
- name
- birth_date
- cnp
- address
- phone_number
- created_at

Admin:
- id
- user_id

Patient:
- id
- user_id
- blood_type
- emergency_contact
- allergies
- weight
- height

Doctor:
- id
- user_id
- department_id
- years_of_experience
- license_number

Department:
- id
- name

Log:
- id
- user_id
- action
- created_at

Notification:
- id
- user_id
- message
- delivery_datetime
// we can use these for appointments
// both for patients and for doctors

Procedure:
- id
- department_id
- name
- duration

MedicalRecord:
- id
- patient_id
- doctor_id
- procedure_id
- conclusion ? diagnosis
- created_at

Appointment:
- id
- patient_id
- doctor_id
- procedure_id
- date_time
// finished can be date_time + procedure.duration < current_time
// should this have room?

Room:
- id
- department_id
- capacity ?

RoomEquipment:
- room_id
- equipment_id
- quantity / stock ?

Equipment:
- id
- name
- specification
- type
- stock

Schedule:
- shift_id
- doctor_id

Shift:
- id
- date
- start_time
- end_time

Review:
- id
- patient_id
- doctor_id
- value / ?? stars / good / bad

