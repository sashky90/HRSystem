using HRSystem.Models;
using HRSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRSystem.Services.ViewModels
{
    public class EmployeeTeamViewModel
    {
        [Display(Name = "Team name")]
        public string Name { get; set; }

        public DeliveryUnit Delivery { get; set; }

        public virtual Project Project { get; set; }

        [Display(Name = "Team leader")]
        public virtual Employee TeamLeader { get; set; }

        [Display(Name = "Project manager")]
        public virtual Employee ProjectManager { get; set; }

        [Display(Name = "Delivery director")]
        public virtual Employee DeliveryDirector { get; set; }

        [Display(Name = "CEO")]
        public virtual Employee CEO { get; set; }

        [Display(Name = "Team members")]
        public virtual ICollection<Employee> Members { get; set; }


    }
}