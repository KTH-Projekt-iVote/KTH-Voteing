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
                if (Answers == null) return 0;
                return Answers.Count();
            }
        }

        public int NoOfVotes
        {
            get
            {
                int count = 0;
                if(NoOfAnswers > 0)
                    foreach (Answer a in Answers)
                    {
                        count += a.voteCount;
                    }   
                return count;
            }
        }

        public virtual ICollection<Answer> Answers { get; set; }

    }
}