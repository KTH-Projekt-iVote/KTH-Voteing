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
                    return null;
            }
        }

        public bool Voted { get; set; }
        //public bool Voted
        //{
        //    get
        //    {
        //        Question question = session.Questions.ElementAt(session.CurrentQuestionIndex);
        //        if (currentQuestion.Equals(question))
        //            return privVoted;

        //        return false;
        //    }
        //    set
        //    {
        //        privVoted = value;
        //    }
        //}
        public Question currentQuestion
        {
            get
            {

                Question question = null;
                try
                {
                    question = session.Questions.ElementAt(session.CurrentQuestionIndex);
                }catch(NullReferenceException e){
                    return null;
                }
                
                return question;
            }
        }

        public string ip { get; set; }

        /**
         * Methods
         * 
         **/

        public Student() { }

        public Student(string pin)
        {
            this.pin = pin;
            this.Voted = false;
        }

        public bool JoinedSession(int SessionID)
        {
            if (session == null)
                return false;

            return this.session.ID == SessionID;
        }

        //public bool Vote(int index)
        //{
        //    //Question question = session.Questions.ToList().ElementAt(session.CurrentQuestionIndex);

        //    if (index < 0 || index > this.currentQuestion.NoOfAnswers - 1)
        //        return false;

        //    Answer answer = currentQuestion.Answers.ToList().ElementAt(index);
        //    answer.voteCount++;

        //    Voted = true;
        //    return true;
        //}

    }
}