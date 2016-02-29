using HRSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRSystem.Services.ViewModels
{
    public class EmployeeManagersViewModel
    {
        public Employee Employee { get; set; }

        public IEnumerable<Employee> Managers { get; set; }
    }
}