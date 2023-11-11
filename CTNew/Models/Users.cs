using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTNew.Models
{
    public partial class Users
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        [ForeignKey("CityId")]
        public int? CityId { get; set; }

        [NotMapped]
        public string GenderId { get; set; }
        [NotMapped]
        public int? StateId { get; set; }
        [NotMapped]
        public string CityName { get; set; }
        [NotMapped]
        public string StateName { get; set; }
        [NotMapped]
        public SelectList States { get; set; }
        [NotMapped]
        public SelectList Cities { get; set; }
        public string Gender { get; set; }
        public virtual City City { get; set; }
    }
}
