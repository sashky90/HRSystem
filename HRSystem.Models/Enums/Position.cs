using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Models.Enums
{
    public enum Position
    {
        Unknown,
        Trainee,
        Junior,
        Intermediate,
        Senior,

        //[EnumMember(Value = "Team leader")]
        [Display(Name = "Team leader")]
        TeamLeader,

        //[EnumMember(Value = "Project manager")]
        [Display(Name = "Project manager")]
        ProjectManager,

        //[EnumMember(Value = "Delivery director")]
        [Display(Name = "Delivery director")]
        DeliveryDirector,

        CEO
    }
}
