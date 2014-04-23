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
            new Teacher{name="Teacher Teacher",username="teacher1",password="pw_teacher1",email="teacher@teach.com"}
            };

            teachers.ForEach(s => context.Teachers.Add(s));
            context.SaveChanges();

            var sessions = new List<Session>
            {
                new Session{name="session1", dateCreated=System.DateTime.Now, dateModifed=System.DateTime.Now, description="desc_test", TeacherID=1, published=false},
                new Session{name="session2", dateCreated=System.DateTime.Now, dateModifed=System.DateTime.Now, description="desc_test", TeacherID=1, published=false}
            };

            sessions.ForEach(s => context.Sessions.Add(s));
            context.SaveChanges();

            var questions = new List<Question>
            {
                new Question{SessionID=1,text="question1.1"},
                new Question{SessionID=1,text="question1.2"},
                new Question{SessionID=2,text="question2.1"}
            };

            questions.ForEach(s => context.Questions.Add(s));
            context.SaveChanges();

            var answers = new List<Answer>{
                new Answer{QuestionID=1, text="answer1.1.1"},
                new Answer{QuestionID=1, text="answer1.1.2"},
                new Answer{QuestionID=1, text="answer1.1.3"},
                new Answer{QuestionID=2, text="answer1.2.1"},
                new Answer{QuestionID=2, text="answer1.2.2"},
                new Answer{QuestionID=3, text="answer2.1.1"}
            };

            answers.ForEach(s => context.Answers.Add(s));
            context.SaveChanges();
        }
    }
}