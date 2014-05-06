using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iVoteMVC.DAL;

namespace iVoteMVC.Models
{
    public class Student
    {
        public int ID { get; set; }
        public Session session { get; set; }

        public bool Voted { get; set; }
        public Question question
        {
            get
            {
                return question;
            }
            set
            {
                Question currentQuestion = session.Questions.ToList().ElementAt(session.CurrentQuestionIndex);
                if (question != currentQuestion)
                {
                    Voted = false;
                    question = currentQuestion;
                }
            }
        }

        public Student(string pin)
        {
            iVoteContext db = new iVoteContext();

            List<Session> sessions = db.Sessions.Where(s => s.PIN.Equals(pin) && s.published == true).ToList();

            if (sessions.Count != 0)
            {
                this.session = sessions.ElementAt(0);
                this.question = session.Questions.ToList().ElementAt(session.CurrentQuestionIndex);
            }
        }

        public bool Vote(int index)
        {
            //Question question = session.Questions.ToList().ElementAt(session.CurrentQuestionIndex);

            if (index < 0 || index > this.question.NoOfAnswers - 1)
                return false;

            Answer answer = question.Answers.ToList().ElementAt(index);
            answer.voteCount++;
            Voted = true;
            return true;
        }

    }
}