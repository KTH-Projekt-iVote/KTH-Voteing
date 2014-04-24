using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace iVoteMVC.Models
{
    public class Teacher
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="Name")]
        [StringLength(50, MinimumLength=1)]
        public string name { get; set; }

        [Required]
        [Display(Name="Username")]
        [StringLength(25, MinimumLength=3)]
        public string username { get; set; }

        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength=6)]
        public string password { get; set; }

        [Required]
        [Display(Name="Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength=5)]
        public string email { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
        /*
         * Lista
         * */
    }
}