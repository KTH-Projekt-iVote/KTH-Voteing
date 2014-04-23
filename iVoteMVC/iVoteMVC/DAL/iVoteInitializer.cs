using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iVoteMVC.Models;
using System.Data.Entity;

namespace iVoteMVC.DAL
{
    public class iVoteInitializer : System.Data.Entity.DropCreateDatabaseAlways<iVoteContext>
    {
        protected override void Seed(iVoteContext context)
        {
            var teachers = new List<Teacher>
            {
            new Teacher{name="Test",username="user_test",password="pw_test"}
            };

            teachers.ForEach(s => context.Teachers.Add(s));
            context.SaveChanges();

            var sessions = new List<Session>
            {
                new Session{name="session_test", dateCreated=System.DateTime.Now, dateModifed=System.DateTime.Now, description="desc_test", TeacherID=1, published=false},
            };

            sessions.ForEach(s => context.Sessions.Add(s));
            context.SaveChanges();

            var questions = new List<Question>
            {
                new Question{SessionID=1,text="question_test"}
            };

            questions.ForEach(s => context.Questions.Add(s));
            context.SaveChanges();

            var answers = new List<Answer>{
                new Answer{QuestionID=1, text="answer_test"}
            };

            answers.ForEach(s => context.Answers.Add(s));
            context.SaveChanges();
        }
    }
}