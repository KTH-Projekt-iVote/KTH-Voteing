using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iVoteMVC.Models
{
    public class Session
    {
        public int ID { get; set; }
        public int TeacherID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateModifed { get; set; }
        public bool published { get; set; }

        public virtual ICollection<Question> Questions { get; set; }    
    }
}