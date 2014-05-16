using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using iVoteMVC.DAL;

namespace iVoteMVC.Models
{
    public class Session
    {
        public int ID { get; set; }
        public int TeacherID { get; set; }

        [Required]
        [Display(Name="Name")]
        [StringLength(50, MinimumLength=1)]
        public string name { get; set; }
        
        [Display(Name="Description")]
        [StringLength(160)]
        public string description { get; set; }
        
        [Required]
        [Display(Name="Created")]
        public DateTime dateCreated { get; set; }
        
        [Required]
        [Display(Name="Modified")]
        public DateTime dateModifed { get; set; }

        [Required]
        [Display(Name="Published")]
        public bool published { get; set; }

        public string PIN { get; set; }

        [Display(Name="Current Question")]
        public int CurrentQuestionIndex { get; set; }

        [Display(Name="Questions")]
        public int NoOfQuestions
        {
            get
            {
                if (Questions == null) return 0;
                return Questions.Count();
            }
        }

        public virtual ICollection<Question> Questions { get; set; }

        [Display(Name="Total number of votes")]
        public int NoofVotes
        {
            get
            {
                int votes = 0;
                if (Questions.Count > 0)
                {
                    foreach (Question q in Questions)
                    {
                        votes += q.NoOfVotes;
                    }
                }
                return votes;
            }
        }

        /**
         * Methods
        **/

        public Session()
        {
            CurrentQuestionIndex = 0;
            dateCreated = System.DateTime.Now;
        }

        public void NextQuestion()
        {
            CurrentQuestionIndex++;
        }

    }
}