using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iVoteMVC.Models
{
    public class Teacher
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}