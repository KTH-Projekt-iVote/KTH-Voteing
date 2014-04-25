using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace iVoteMVC.Models
{
    public class Question
    {
        public int ID { get; set; }
        public int SessionID { get; set; }
        
        [Required]
        [Display(Name="Question")]
        [StringLength(160, MinimumLength=1)]
        public string text { get; set; }

        [Display(Name="Answers")]
        public int NoOfAnswers
        {
            get
            {
                return Answers.Count();
            }
        }

        public virtual ICollection<Answer> Answers { get; set; }

    }
}