﻿using HRSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Models
{
    public class Project
    {
        public Project()
        {
            this.Teams = new HashSet<Team>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [Required]
        public DeliveryUnit Delivery { get; set; }

        [Display(Name = "Teams names")]
        public virtual ICollection<Team> Teams { get; set; }

        public override int GetHashCode()
        {
            //if ((Id == null))
            //{
            //    return base.GetHashCode();
            //}
            return Id.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var project = other as Project;
            if (other == null || project == null)
            {
                return false;
            }

            return this.Id.Equals(project.Id);
        }
    }
}
