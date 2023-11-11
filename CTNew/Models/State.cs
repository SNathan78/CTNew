using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTNew.Models
{
    public partial class State
    {
        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }

       public virtual ICollection<City> City { get; set; }
    
    }

}
