﻿using HRSystem.Models;
using HRSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRSystem.Services.ViewModels
{
    public class TeamWithEmployeesViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DeliveryUnit Delivery { get; set; }

        [Required]
        public int? LeaderId { get; set; }

        [Display(Name = "Leader name")]
        public virtual Employee Leader { get; set; }

        [Display(Name = "Team members")]
        public virtual IList<Employee> Members { get; set; }

        public int? ProjectId { get; set; }

        [Display(Name = "Project assigned name")]
        public virtual Project Project { get; set; }

        public override int GetHashCode()
        {
            if ((Id == null))
            {
                return base.GetHashCode();
            }
            return Id.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var team = other as Team;
            if (other == null || team == null)
            {
                return false;
            }

            return this.Id.Equals(team.Id);
        }
    }
}