using HRSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Models
{
    public class Employee
    {
        public Employee()
        {
            this.Teams = new HashSet<Team>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public Position Position { get; set; }

        public DeliveryUnit Delivery { get; set; }

        public double Salary { get; set; }

        [Display(Name = "Workplace")]
        public string Workplace { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }

        [Display(Name = "Cell number")]
        public string CellNumber { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        [Display(Name = "Manager name")]
        public virtual Employee Manager { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public string NamePositionAndEmail
        {
            get
            {
                var returnString = "";
                switch (Position)
                {
                    case Position.Trainee:
                        returnString = String.Format("Trainee: {0}, {1}", Name, Email);
                        break;
                    case Position.Junior:
                        returnString = String.Format("Junior: {0}, {1}", Name, Email);
                        break;
                    case Position.Intermediate:
                        returnString = String.Format("Intermediate: {0}, {1}", Name, Email);
                        break;
                    case Position.Senior:
                        returnString = String.Format("Senior: {0}, {1}", Name, Email);
                        break;
                    case Position.TeamLeader:
                        returnString = String.Format("TL: {0}, {1}", Name, Email);
                        break;
                    case Position.ProjectManager:
                        returnString = String.Format("PM: {0}, {1}", Name, Email);
                        break;
                    case Position.DeliveryDirector:
                        returnString = String.Format("DD: {0}, {1}", Name, Email);
                        break;
                    case Position.CEO:
                        returnString = String.Format("CEO: {0}, {1}", Name, Email);
                        break;
                    default:
                        break;
                }
                return returnString;
            }
        }

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
            var employee = other as Employee;
            if (other == null || employee == null)
            {
                return false;
            }

            return this.Id.Equals(employee.Id);
        }
    }
}
