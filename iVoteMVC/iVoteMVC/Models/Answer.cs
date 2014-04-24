using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace iVoteMVC.Models
{
    public class Answer
    {
        public int ID {get; set;}
        public int QuestionID { get; set; }
        public int voteCount { get; set; }

        [Required]
        [Display(Name="Answer")]
        [StringLength(80, MinimumLength=1)]
        public string text { get; set; }
    }
}