using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyReloadedOfficeApp.Models
{
    public class UsersClustersModel
    {
  
        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(128, ErrorMessage = "String too long (max 128 chars)")]
        public string Id { get; set; }

        [ForeignKey("UserName")]
        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(128, ErrorMessage = "String too long (max 128 chars)")]
        public string Name { get; set; }

        [ForeignKey("Name")]
        [Required(ErrorMessage = "Mandatory field")]
        [StringLength(256, ErrorMessage = "String too long (max 256 chars)")]
        public string IdDepartment { get; set; }

        [ForeignKey("IdUserTypes")]
        [Required(ErrorMessage = "Mandatory field")]
        public string IdUserType { get; set; }

        [StringLength(128, ErrorMessage = "String too long (max 128 chars)")]
        public string Discriminator { get; set; }
    }
}