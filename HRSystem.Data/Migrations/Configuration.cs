namespace HRSystem.Data.Migrations
{
    using HRSystem.Models;
    using HRSystem.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EmployeeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        // Seed database 
        protected override void Seed(EmployeeDbContext context)
        {
            if (!context.Projects.Any())
            {
                SeedProjects(context);
            }
            if (!context.Employees.Any())
            {
                SeedEmployees(context);
            }
        }

        // Seed employees method
        private static void SeedEmployees(EmployeeDbContext context)
        {
            var employee = new List<Employee>()
            {
                new Employee
                {
                    Name = "Misho Mishev",
                    Position = Position.CEO,
                    Salary = 20000,
                    Workplace = "Bulgaria Air",
                    Email = "misho@gmail.com",
                    CellNumber = "0999 11 22 33"
                },
                new Employee
                {
                    Name = "Don Korleone",
                    Position = Position.DeliveryDirector,
                    ManagerId = 1,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 15000,
                    Workplace = "Bulgaria Air",
                    Email = "korleone@money.bg",
                    CellNumber = "0999 11 22 33"
                },
                new Employee
                {
                    Name = "Hristo Hristov",
                    Delivery = DeliveryUnit.Healthcare,
                    ManagerId = 1,
                    Position = Position.DeliveryDirector,
                    Salary = 15000,
                    Workplace = "Bulgaria Air",
                    Email = "tripleH@abv.bg",
                    CellNumber = "0999 99 55 33"
                },
                new Employee
                {
                    Name = "Vladimir Klichko",
                    Position = Position.DeliveryDirector,
                    ManagerId = 1,
                    Delivery = DeliveryUnit.Government,
                    Salary = 30000,
                    Workplace = "Bulgaria Air",
                    Email = "klichko@abv.bg",
                    CellNumber = "0999 11 22 33"
                },
                new Employee
                {
                    Name = "Mirela Mileva",
                    Delivery = DeliveryUnit.Finance,
                    Position = Position.ProjectManager,
                    ManagerId = 2,
                    Salary = 10000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "mirela@abv.bg",
                    CellNumber = "0999 99 88 77"
                },
                new Employee
                {
                    Name = "Boiko Borisov",
                    Position = Position.ProjectManager,
                    ManagerId = 4,
                    Delivery = DeliveryUnit.Government,
                    Salary = 20000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "borko@gov.bg",
                    CellNumber = "0888 88 88 88"
                },
                new Employee
                {
                    Name = "Dimitur Berbatov",
                    Position = Position.ProjectManager,
                    ManagerId = 3,
                    Delivery = DeliveryUnit.Healthcare,
                    Salary = 20000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "mitko@abv.bg",
                    CellNumber = "0999 11 22 33"
                },
                new Employee
                {
                    Name = "Juliyan Boyanov",
                    Position = Position.TeamLeader,
                    ManagerId = 5,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 10000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "juliqn@gmail.bg"
                },
                new Employee
                {
                    Name = "Desislav Bonchev",
                    Position = Position.TeamLeader,
                    ManagerId = 6,
                    Delivery = DeliveryUnit.Government,
                    Salary = 10000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "desislav@abv.bg",
                    CellNumber = "0979 423 432"
                },
                new Employee
                {
                    Name = "Joro Vratarcheto",
                    Position = Position.TeamLeader,
                    ManagerId = 7,
                    Delivery = DeliveryUnit.Healthcare,
                    Salary = 2000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "joro@abv.bg"
                },
                new Employee
                {
                    Name = "Petq Petrova",
                    Position = Position.Senior,
                    //ManagerId = 10,
                    Delivery = DeliveryUnit.Entertainment,
                    Salary = 10000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "pepi@gmail.bg",
                    CellNumber = "0999 11 22 33"
                },
                new Employee
                {
                    Name = "Dimitur Penev",
                    Position = Position.Senior,
                    //ManagerId = 12,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 10000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "penata@gmail.com",
                    CellNumber = "0999 11 22 33"
                },
                new Employee
                {
                    Name = "Christopher",
                    Position = Position.Intermediate,
                    //ManagerId = 12,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 5000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "christopher@gmail.com",
                    CellNumber = "0999 11 22 33"
                },
                new Employee
                {
                    Name = "Dimo",
                    Position = Position.Intermediate,
                    //ManagerId = 12,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 5000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "dimo@gmail.com",
                    CellNumber = "0999 11 72 33"
                },
                new Employee
                {
                    Name = "Lora",
                    Position = Position.Trainee,
                    //ManagerId = 12,
                    Delivery = DeliveryUnit.Government,
                    Salary = 5000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "lora@gmail.com",
                    CellNumber = "0999 11 21 33"
                },
                new Employee
                {
                    Name = "Milena",
                    Position = Position.Trainee,
                    //ManagerId = 12,
                    Delivery = DeliveryUnit.Healthcare,
                    Salary = 5000,
                    Workplace = "Bulgaria Air Bulgaria",
                    Email = "mmmm@gmail.com",
                    CellNumber = "0999 11 00 33"
                }
            };

            foreach (var item in employee)
            {
                context.Employees.AddOrUpdate(item);
            }
            context.SaveChanges();
        }

        // Seed projects method
        private static void SeedProjects(EmployeeDbContext context)
        {
            var projects = new List<Project>{
                  new Project
                  {
                      Name = "HR management system",
                      Delivery = DeliveryUnit.Finance
                  },
                  new Project
                  {
                      Name = "Air Force One",
                      Delivery = DeliveryUnit.Government
                  },
                  new Project
                  {
                      Name = "Gym Airplane",
                      Delivery = DeliveryUnit.Healthcare
                  }
            };

            foreach (var item in projects)
            {
                context.Projects.AddOrUpdate(item);
            }
            context.SaveChanges();
        }
        
    }
}
