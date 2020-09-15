# DentistApp
This project was created while learning ASP .Net Core. 
Views uses Bootstrap, migrations and the database were created using Entity Framework Core. The project was based on the Clean Architecture concept.


DentistApp is an application designed to manage visits to a dental clinic. 
The home page contains scheduled visits for the current day and allows you to add a new visit, new patient or first visit. Each row in the table contains basic information about the visit: time, status, patient, dentist. 
![Home page](/Images/home.PNG)

All visits are listed in the "Visits" tab. There you can filter visits by date, dentist and/or view only future visits. This tab uses the paging mechanism, displaying visits from the next two days on one page.
!["Visits" tab](/Images/visits.PNG)

The "Patients" tab lists all registered patients. Each patient has his own Patient Card. 
!["Patients" tab](/Images/patients.PNG)
The Patient Card has detailed information about the patient: personal data, address and visit history. 
!["Patient Card"](/Images/patientcard.PNG)
In the history of the patient's visits, you can check the details of a given visit, such as diagnosis and procedure.
![Details of visit](/Images/visitdetails.PNG)

The "Dentist" tab lists all dentists.
!["Dentists" tab](/Images/dentists.PNG)
![Details of dentist](/Images/dentistdetails.PNG)
 
 
When adding a visit, the current date and any dentist are selected by default. Selecting a date and/or dentist will display the available times for the selected options. The available times ensure that a visit is made at a given time and that the dentist may have one visit at a time.

After selecting the patient, it is checked whether the patient has a different visit scheduled for the selected date and time. If so, an appropriate message is displayed.

The application provides users with three roles: Dentist, Secretary and Admin. The Secretary can add new patients and visits, edit patients and visits. The Dentist can add and edit details (diagnoses, procedures) only in his visits. Admin can manage roles, add and edit visits or patients but cannot add or edit diagnoses and procedures.
