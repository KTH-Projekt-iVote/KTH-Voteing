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
        //public Session session { get; set; }
        public string pin { get; set; }
        public Session session
        {
            get
            {
                iVoteContext db = new iVoteContext();
                List<Session> sessions = db.Sessions.Where(s => s.PIN.Equals(pin) && s.published == true).ToList();
                if (sessions.Count > 0)
                    return sessions.ElementAt(0);
                else
                    throw new InvalidOperationException("No session found.");
            }
        }

        public bool Voted { get; set; }
        public Question currentQuestion
        {
            get
            {
                return session.Questions.ElementAt(session.CurrentQuestionIndex);
            }
        }

        /**
         * Methods
         * 
         **/

        public Student(string pin)
        {
            this.pin = pin;
        }

        //public Student(string pin)
        //{
        //    iVoteContext db = new iVoteContext();

        //    List<Session> sessions = db.Sessions.Where(s => s.PIN.Equals(pin) && s.published == true).ToList();
        //    var sessionQuery = from s in db.Sessions
        //                       select s;
        //    sessionQuery = sessionQuery.Where(s => s.PIN.Equals(pin) && s.published == true);

        //    if (sessions.Count > 0)
        //    {
        //        this.session = sessions.ElementAt(0);
        //    }
        //    else
        //        throw new InvalidOperationException("No session found. size : " + sessions.Count() );

        //}

        public bool Vote(int index)
        {
            //Question question = session.Questions.ToList().ElementAt(session.CurrentQuestionIndex);

            if (index < 0 || index > this.currentQuestion.NoOfAnswers - 1)
                return false;

            Answer answer = currentQuestion.Answers.ToList().ElementAt(index);
            answer.voteCount++;
            Voted = true;
            return true;
        }

    }
}