using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;

namespace MyReloadedOfficeApp.Models
{
    public class OfficeBuildingsModel
    {

        public Guid IdBuilding { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(100, ErrorMessage = "String too long (max 100 chars)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(100, ErrorMessage = "String too long (max 100 chars)")]
        public string StreetHouseNumber { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(50, ErrorMessage = "String too long (max 50 chars)")]
        public string City { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(50, ErrorMessage = "String too long (max 50 chars)")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(50, ErrorMessage = "String too long (max 50 chars)")]
        public string RentalContract { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(1000, ErrorMessage = "String too long (max 1000 chars)")]
        public string BuildingDescription { get; set; }
    }
}