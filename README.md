# Prems - Web Application for School Management
## Author: 
- Luu Le Ba Chinh - 20521133  
- Nguyen Trung Duc - 20521200
- Nguyen Ngoc Duc - 20521197
- Lam Phu Sy - 20521853  
## Purpose:
- An educational system that supports teachers and students in teaching and learning
- Help organization owner manage their education system easy
## Key features:
The app has 4 main roles:
- Administrator: Dashboard
- User as Principal:
  + Create schools/organizations
  + Mangage organizations: Manage Student/Teacher accounts, Subjects, School years, Classes, Timetables, Regulars...
- User as Teacher:
  + Login to school with teacher account created by Principal
  + Manage students, view Timetables, manage scores...
- User as Student:
  + Login to school with student account created by Principal
  + View timetables, view scores, view subject...
## Platform:
- ASP.NET MVC 5
## How to install and run app:
### Steps to Follow:

1. **Clone or download the project**:
   - Clone the repository with the following command:
     ```bash
     git clone <repository-url>
     ```
   - Or download the project from the repository and extract it.

2. **Install dependencies**:
   - Open the terminal or command prompt in the root folder of the project.
   - Run the following command to restore the necessary packages:
     ```bash
     dotnet restore
     ```

3. **Update the database**:
   - Run the following command to apply migrations and update the database:
     ```bash
     dotnet ef database update --project DHDCShop.Models
     ```

4. **Run the project**:
   - Run the project with the following command:
     ```bash
     dotnet run
     ```

5. **Access the application**:
   - Once the project is running, open your browser and navigate to the provided URL.

### Requirements:
- **.NET Core SDK** must be installed on your machine to use CLI commands like `dotnet`.
- Ensure your database configuration in `appsettings.json` or the relevant configuration file is correct, if needed.
