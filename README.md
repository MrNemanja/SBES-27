Implement a service that provides database usage (text file) to different types of client applications by exposing the IDatabaseManagement interface. 
The database contains the following information:

 - unique identifier in the database
 - region
 - city
 - year
 - electricity consumption for the months of the year.

Depending on the type of client application:

Clients of type Admins have the right to create, archive, and delete the database.
Database deletion can be done during archiving, but also at the request of these users without prior archiving.
Clients of type Writers have the right to write and modify the database.
Clients of type Readers have the right to read data from the database. 
Specifically, they should be able to obtain the following information:

 - average consumption for a specific city
 - average consumption for a specific region
 - finding the largest consumer for a specific region

Authentication between client applications and this service is implemented using the Windows authentication protocol, and authorization is based on the RBAC authorization scheme.
It is necessary to provide data replication to a secondary service. Authentication between the primary and secondary service is established via certificates.
All actions in the system, starting from authentication, authorization, and operations on the database itself, should be logged within a dedicated log file inside the Windows Event Log.
