using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyReloadedOfficeApp.Models
{
    public class DepartmentsModel
    {

        public Guid IdDepartment { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(100, ErrorMessage = "String too long (max 100 chars)")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Mandatory field")]
        public int MaximumSeatsPerDepartment { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(100, ErrorMessage = "String too long (max 100 chars)")]
        public string DepartmentDescription { get; set; }
   

    }
}