using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyReloadedOfficeApp.Models
{
    public class BookingsModel
    {

        public Guid IdBooking { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime BookingValidFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime BookingValidTo { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}")]
        public DateTime BookingTimeStamp { get; set; }
        
        [Required(ErrorMessage = "Mandatory field")]
        public int BookedSeats { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [ForeignKey("IdFloor")]
        public Guid IdFloor { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [ForeignKey("IdBuilding")]
        public Guid IdBuilding { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(256, ErrorMessage = "String too long (max 256 chars)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(100, ErrorMessage = "String too long (max 100 chars)")]
        public string UserRole { get; set; }

        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(100, ErrorMessage = "String too long (max 100 chars)")]
        public string UserDepartment { get; set; }

    }
}