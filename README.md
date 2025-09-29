This project implements a Database Management Service that provides secure, role-based access to a text-file database of electricity consumption data. 
The service exposes an IDatabaseManagement interface for different client types — Admins (create, archive, delete databases), Writers (add and modify records), and Readers (query average consumption and find largest consumers). 
Authentication uses Windows authentication for clients and certificate-based authentication between primary and secondary services, while authorization follows the RBAC scheme. 
All operations are logged in the Windows Event Log, and data is replicated to a secondary service to ensure reliability and fault tolerance.
