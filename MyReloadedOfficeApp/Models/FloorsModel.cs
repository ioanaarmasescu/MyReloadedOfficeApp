using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyReloadedOfficeApp.Models
{
    public class FloorsModel
    {
        public Guid IdFloor { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(100, ErrorMessage = "String too long (max 100 chars)")]
        public string Name { get; set; }

        [ForeignKey("IdDepartment")]
        [Required(ErrorMessage = "Mandatory field")]
        public Guid IdDepartment { get; set; }

        [ForeignKey("IdBuilding")]
        [Required(ErrorMessage = "Mandatory field")]
        public Guid IdBuilding { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        public int BookableSeats { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(1000, ErrorMessage = "String too long (max 1000 chars)")]
        public string FloorDescription { get; set; }




    }
}