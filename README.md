This is a full-stack ASP.NET Core 7 application for managing loan applications with role-based access:

User: Can register and submit loan applications
Approver: Can approve or reject loan applications
Admin: Can register new approvers and manage the system

The app includes:
JWT Authentication
RabbitMQ (via Docker) 
Ext.NET Razor Pages UI for the frontend
MSSQL (Entity Framework Core) for persistence

1. Clone the Repository
git clone https://github.com/yourusername/loan-processing.git
cd loan-processing

2. Run RabbitMQ with Docker
RabbitMQ is required for messaging between services.

Run the following command:
docker run -d --hostname rabbitmq-host --name rabbitmq-container -p 5672:5672 -p 15672:15672 rabbitmq:3-management

Login with:
Username: guest
Password: guest

3. Seed Default Admin 
On first run, the application will seed a default Admin account:

Username: Admin
Password: Admin

Use this account to Log in as admin and register new Approvers from the UI
