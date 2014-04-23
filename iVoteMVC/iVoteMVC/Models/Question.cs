using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iVoteMVC.Models
{
    public class Question
    {
        public int ID { get; set; }
        public int SessionID { get; set; }
        public string text { get; set; }

        public ICollection<Answer> answers { get; set; }

    }
}