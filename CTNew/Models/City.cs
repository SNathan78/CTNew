using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTNew.Models
{
    public partial class City
    {
        public City()
        {
            Users = new HashSet<Users>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }

        [ForeignKey("StateId")]
        public int StateId { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
