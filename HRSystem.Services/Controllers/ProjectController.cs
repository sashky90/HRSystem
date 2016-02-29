using HRSystem.Data;
using HRSystem.Models;
using HRSystem.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HRSystem.Services.Controllers
{
    public class ProjectController : Controller
    {
        private EmployeeDbContext context = EmployeeDbContext.Create();

        // GET: Project
        public ActionResult Index()
        {
            var projects = context.Projects.ToList();

            return View(projects);
        }


        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = context.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Name,Delivery, Teams, Description")]ProjectWithTeamsViewModel projectModel)
        {
            //Validations
            if (projectModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
            }

            if (projectModel.Teams != null)
            {
                var disinctTeams = projectModel.Teams.Distinct();
                if (disinctTeams.Count() < projectModel.Teams.Count)
                {
                    ModelState.AddModelError("Teams", "Each team may exists only once in a project.");
                }
            }

            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Teams")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
                return View(projectModel);
            }

            //Creating new project and filling its props
            var projectToSave = new Project();
            projectToSave.Name = projectModel.Name;
            projectToSave.Delivery = projectModel.Delivery;

            //Adding teams to the project
            if (projectModel.Teams != null)
            {
                foreach (var team in projectModel.Teams)
                {
                    var teamToAdd = context.Teams.Find(team.Id);
                    projectToSave.Teams.Add(teamToAdd);
                }
            }

            context.Projects.Add(projectToSave);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}