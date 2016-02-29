using HRSystem.Data;
using HRSystem.Models;
using HRSystem.Models.Enums;
using HRSystem.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HRSystem.Services.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeDbContext context = EmployeeDbContext.Create();

        // GET: Employee
        public ActionResult Index()
        {
            var employees = context.Employees.Include(e => e.Manager);
            return View(employees);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = context.Employees.FirstOrDefault(e => e.Id == id);
            var managers = GetManagers(employee);
            var model = new EmployeeManagersViewModel { Employee = employee, Managers = managers };
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            fillTheViewBags();
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id, Name,Position,Delivery,Salary,WorkPlace,Email,CellNumber,ManagerId")] Employee employeeModel)
        {
            //Validations
            if (employeeModel.Position == 0)
            {
                ModelState.AddModelError("Position", "The position field is required");
            }
            if (employeeModel.Delivery == 0 && employeeModel.Position != Position.CEO)
            {
                ModelState.AddModelError("Delivery", "The delivery field is required");
            }
            if (employeeModel.ManagerId != null)
            {
                var manager = context.Employees.Find(employeeModel.ManagerId);
                if (employeeModel.Position > manager.Position)
                {
                    ModelState.AddModelError("", "The employee cannot have manager with lower position then his");
                }
            }

            if (employeeModel.Position == Position.DeliveryDirector && employeeModel.ManagerId == null)
            {
                employeeModel.ManagerId = context.Employees.FirstOrDefault(e => e.Position == Position.CEO).Id;
            }

            if (!ModelState.IsValid)
            {
                fillTheViewBags(employeeModel);
                return View(employeeModel);
            }

            context.Employees.Add(employeeModel);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Taking employee 
            var employee = context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            fillTheViewBags(employee);
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Delivery,Salary,Position,WorkPlace,Email,CellNumber,ManagerId")] Employee employeeModel)
        {
            //Validations

            if (employeeModel.Delivery == 0 && employeeModel.Position != Position.CEO)
            {
                ModelState.AddModelError("Delivery", "The delivery field is required");
            }
            if (employeeModel.ManagerId != null)
            {
                var manager = context.Employees.Find(employeeModel.ManagerId);
                if (employeeModel.Position >= manager.Position)
                {
                    ModelState.AddModelError("", "The employee cannot have manager with lower or equal position then his");
                }

            }
            if (!ModelState.IsValid)
            {
                fillTheViewBags(employeeModel);
                return View(employeeModel);
            }

            //Telling the EF that 2 of the entities are modified and to save the changes           
            context.Entry(employeeModel).State = EntityState.Modified;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Selecting employee with position lower then TL and printing all its team members, TL, PM, DD, CEO
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View with list of team members for the choosen employee</returns>
        public ActionResult EmployeeTeamPreview(int id)
        {
            var teamView = new EmployeeTeamViewModel();
            var teamMember = context.Employees.Find(id);

            if (teamMember.Teams.Count != 0)
            {
                var teams = teamMember.Teams.ToList();

                teamView.Name = teams[0].Name;
                teamView.Project = teams[0].Project;
                teamView.Members = teams[0].Members;
                teamView.Delivery = teams[0].Delivery;

                var teamLeaderPosition = teams[0].Leader.Position;
                switch (teamLeaderPosition)
                {
                    case Position.TeamLeader:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    case Position.ProjectManager:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    case Position.DeliveryDirector:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    case Position.CEO:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    default:
                        break;
                }

                //Change this to make it generic for more then one team
                if (teamView.TeamLeader.Manager != null)
                {
                    teamView.ProjectManager = teamView.TeamLeader.Manager;
                    if (teamView.ProjectManager.Manager != null)
                    {
                        teamView.DeliveryDirector = teamView.ProjectManager.Manager;
                        if (teamView.DeliveryDirector.Manager != null)
                        {
                            teamView.CEO = teamView.DeliveryDirector.Manager;
                        }
                    }
                }

            }

            return View(teamView);
        }

        /// <summary>
        /// Fill the ViewBags with Managers("ManagerId")
        /// </summary>
        private void fillTheViewBags()
        {
            var managers = context.Employees.Where(e => e.Position >= Position.TeamLeader);

            ViewBag.ManagerId = new SelectList(managers, "Id", "NamePositionAndEmail");
        }

        /// <summary>
        /// Fill the ViewBags with Managers("ManagerId")
        /// </summary>
        /// 
        private void fillTheViewBags(Employee employee)
        {
            var managers = context.Employees.Where(e => e.Position > employee.Position && e.Position >= Position.TeamLeader && e.Id != employee.Id).OrderBy(e => e.Position);

            ViewBag.ManagerId = new SelectList(managers, "Id", "NamePositionAndEmail", employee.ManagerId);
        }

        private List<Employee> GetManagers(Employee employee)
        {
            List<Employee> managerList = new List<Employee>();

            while (employee.ManagerId != null)
            {
                managerList.Add(employee.Manager);
                employee = employee.Manager;
            }

            return managerList;
        }

    }
}