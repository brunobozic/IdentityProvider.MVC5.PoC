# 1) Set password with the Secret Manager tool.
  - dotnet user-secrets set SeedUserPW <pw>

# 2) Create your database using a local SQLSERVER installation
  - run Update-Database from "Package Manager Console" (found in VS2019 from the "Tools" menu item) 
  - use the MVC6 project as your starting project and the "EFCore" project as the assembly that houses the DbContext and migrations

# 3) Log in as user name : bruno.bozic@gmail.com (and the password you defined using the dotnet user-secrets in step #1)