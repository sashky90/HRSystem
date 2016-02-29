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
    public class TeamController : Controller
    {
        private EmployeeDbContext context = EmployeeDbContext.Create();

        // GET: Team
        public ActionResult Index()
        {
            var teams = context.Teams.Include(t => t.Leader).Include(t => t.Project);

            return View(teams);

        }

        // GET: Team/Create
        public ActionResult Create()
        {
            FillTheViewBags();
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId, Members")] TeamWithEmployeesViewModel teamModel)
        {
            if (teamModel.Members != null)
            {
                var disinctEmployees = teamModel.Members.Distinct();
                if (disinctEmployees.Count() < teamModel.Members.Count)
                {
                    ModelState.AddModelError("", "Each employee may exists only once in a team.");
                }
            }
            if (teamModel.LeaderId != null)
            {
                var leader = context.Employees.Find(teamModel.LeaderId);
                if (teamModel.Members != null)
                {
                    foreach (var member in teamModel.Members)
                    {
                        var memberFromBase = context.Employees.Find(member.Id);
                        if (memberFromBase.Position > leader.Position)
                        {
                            ModelState.AddModelError("", "The leader of the team must be the employee with the highest position.");
                        }
                        if (member.Id == teamModel.LeaderId)
                        {
                            ModelState.AddModelError("LeaderId", "The leader cannot be duplicated as a member of the team");
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("LeaderId", "Leader of the team is required.");
            }

            if (teamModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
            }

            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Members")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                FillTheViewBags();
                return View(teamModel);
            }

            //Creating a team and filling up its props
            var team = new Team();
            team.Name = teamModel.Name;
            team.Delivery = teamModel.Delivery;
            team.LeaderId = teamModel.LeaderId;
            team.ProjectId = teamModel.ProjectId;

            var teamLeader = context.Employees.Find(teamModel.LeaderId);
            teamLeader.Teams.Add(team);
            //Changing employees Manager and Delivery to be the same as their current team and after that adding them in the team
            if (teamModel.Members != null)
            {
                foreach (var employee in teamModel.Members)
                {
                    var teamMember = context.Employees.Find(employee.Id);
                    teamMember.ManagerId = team.LeaderId;
                    teamMember.Delivery = team.Delivery;
                    team.Members.Add(teamMember);
                }
            }


            context.Teams.Add(team);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var teamEdit = context.Teams.Find(id);
            //Creating TeamModelView and filling it up
            var teamView = new TeamWithEmployeesViewModel();
            teamView.Name = teamEdit.Name;
            teamView.Id = teamEdit.Id;
            teamView.Members = teamEdit.Members.ToList();
            teamView.LeaderId = teamEdit.LeaderId;
            teamView.ProjectId = teamEdit.ProjectId;
            teamView.Delivery = teamEdit.Delivery;

            FillTheViewBags(teamView);
            return View(teamView);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Delivery,LeaderId, Leader,ProjectId, Members")] TeamWithEmployeesViewModel teamModel)
        {
            if (teamModel.Members != null)
            {
                var disinctEmployees = teamModel.Members.Distinct();
                if (disinctEmployees.Count() < teamModel.Members.Count)
                {
                    ModelState.AddModelError("", "Each employee may exists only once in a team.");
                }
            }
            if (teamModel.LeaderId != null)
            {
                var leader = context.Employees.Find(teamModel.LeaderId);
                if (teamModel.Members != null)
                {
                    foreach (var member in teamModel.Members)
                    {
                        var memberFromBase = context.Employees.Find(member.Id);
                        if (memberFromBase.Position > leader.Position)
                        {
                            ModelState.AddModelError("", "The leader of the team must be the employee with the highest position.");
                        }
                        if (member.Id == teamModel.LeaderId)
                        {
                            ModelState.AddModelError("LeaderId", "The leader cannot be duplicated as a member of the team");
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("LeaderId", "Leader of the team is required.");
            }

            if (teamModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
            }
            var teamEditted = context.Teams.Find(teamModel.Id);
            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Members")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                teamModel.Members = teamEditted.Members.ToList();
                FillTheViewBags(teamModel);
                return View(teamModel);
            }

            //taking the current team from the context and updating its fields

            teamEditted.Name = teamModel.Name;
            teamEditted.ProjectId = teamModel.ProjectId;
            teamEditted.Delivery = teamModel.Delivery;

            if (teamEditted.LeaderId != teamModel.LeaderId)
            {
                var oldLeader = context.Employees.Find(teamEditted.LeaderId);
                oldLeader.Teams.Remove(teamEditted);
                var newLeader = context.Employees.Find(teamModel.LeaderId);
                newLeader.Teams.Add(teamEditted);
                teamEditted.LeaderId = teamModel.LeaderId;
            }

            if (teamModel.Members != null)
            {
                //Subtraction the Old from New team so we can get the teams who are removed from the project
                var subractOldFromNewMembers = teamEditted.Members.Except(teamModel.Members);
                var listWithMembersToRemove = subractOldFromNewMembers.ToList();
                listWithMembersToRemove.Remove(teamEditted.Leader);
                if (listWithMembersToRemove != null)
                {
                    foreach (var memberToRemove in listWithMembersToRemove)
                    {
                        //Finding the employee from the context and checking if he is the leader of team
                        //after that deleting if he is not cos the leader of team is still member of the team
                        var employeeToDelete = context.Employees.Find(memberToRemove.Id);
                        if (employeeToDelete.Id != teamEditted.LeaderId)
                        {
                            teamEditted.Members.Remove(employeeToDelete);
                        }
                    }
                }

                //Subtraction the New from Old team so we can get the teams who are added to the project
                var subractNewFromOldMemebers = teamModel.Members.Except(teamEditted.Members);
                if (subractNewFromOldMemebers != null)
                {
                    foreach (var memberToAdd in subractNewFromOldMemebers)
                    {
                        //Finding the member to add from the context and checking his ManagerId and
                        //if its null we are assigning him to the current team Leader as Manager and changing his delivery to the team ones
                        var employeeToAdd = context.Employees.Find(memberToAdd.Id);
                        if (employeeToAdd.ManagerId == null)
                        {
                            employeeToAdd.ManagerId = teamEditted.LeaderId;
                        }
                        employeeToAdd.Delivery = teamEditted.Delivery;
                        teamEditted.Members.Add(employeeToAdd);
                    }
                }
            }
            else
            {
                foreach (var member in teamEditted.Members)
                {
                    if (member.Id != teamEditted.LeaderId)
                    {
                        member.Teams.Remove(teamEditted);
                    }
                }
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        private void FillTheViewBags(TeamWithEmployeesViewModel teamView = null)
        {
            List<Employee> freeLeaders = new List<Employee>();

            if (teamView != null)
            {
                // Taking all the free leaders who are: TLs without team or already the asigned leader 
                freeLeaders = context.Employees.Where(e => e.Position == Position.TeamLeader &&
                                                                (e.Teams.Count == 0 || e.Id == teamView.LeaderId))
                                                                .OrderBy(e => e.Position)
                                                                .ToList();
            }
            else
            {
                // Taking all the free leaders who are: TLs without team or already the asigned leader 
                freeLeaders = context.Employees.Where(e => e.Position == Position.TeamLeader &&
                                                                e.Teams.Count == 0)
                                                                .OrderBy(e => e.Position)
                                                                .ToList();
            }

            
            // Taking employees who are: with position lower then TL
            var freeEmployees = context.Employees.Where(e => e.Teams.Count == 0 && e.Position < Position.TeamLeader)
                                                            .OrderBy(e => e.Position)
                                                            .ToList();

            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "NamePositionAndEmail");
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name");
            ViewBag.FreeEmployees = new SelectList(freeEmployees, "Id", "NamePositionAndEmail");
        }

    }
}