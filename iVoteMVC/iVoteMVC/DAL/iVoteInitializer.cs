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
            new Teacher{name="Teacher Teacher",username="teacher1",password="pw_teacher1",email="teacher@teach.com"},
            new Teacher{name="Teacher2",username="teacher2",password="pw_teacher2", email="techer2@t.tch"}
            };

            teachers.ForEach(s => context.Teachers.Add(s));
            context.SaveChanges();

            var sessions = new List<Session>
            {
                new Session{name="session1", dateCreated=System.DateTime.Now, dateModifed=System.DateTime.Now, description="desc_test", TeacherID=1, published=true, PIN="0000", CurrentQuestionIndex=0},
                new Session{name="session2", dateCreated=System.DateTime.Now, dateModifed=System.DateTime.Now, description="desc_test", TeacherID=1, published=true, PIN="1111", CurrentQuestionIndex=0},
                new Session{name="session1", dateCreated=System.DateTime.Now, dateModifed=System.DateTime.Now, description="desc_test", TeacherID=2, published=false}
            };

            sessions.ForEach(s => context.Sessions.Add(s));
            context.SaveChanges();

            var questions = new List<Question>
            {
                new Question{SessionID=1,text="question1.1"},
                new Question{SessionID=1,text="question1.2"},

                new Question{SessionID=2,text="question2.1"},

                new Question{SessionID=3,text="question1.1"},
                new Question{SessionID=3,text="question1.2"}
            };

            questions.ForEach(s => context.Questions.Add(s));
            context.SaveChanges();

            var answers = new List<Answer>{
                new Answer{QuestionID=1, text="answer1.1.1", voteCount=0},
                new Answer{QuestionID=1, text="answer1.1.2", voteCount=2},

                new Answer{QuestionID=1, text="answer1.1.3", voteCount=4},

                new Answer{QuestionID=2, text="answer1.2.1", voteCount=4},
                new Answer{QuestionID=2, text="answer1.2.2", voteCount=4},

                new Answer{QuestionID=3, text="answer2.1.1", voteCount=2},

                new Answer{QuestionID=4, text="answer1.1.1", voteCount=29},
                new Answer{QuestionID=4, text="answer1.1.2", voteCount=41},

                new Answer{QuestionID=5, text="answer2.1.1", voteCount=2},
            };

            answers.ForEach(s => context.Answers.Add(s));
            context.SaveChanges();

            //var students = new List<Student>{
            //    new Student{ID=1, Voted=false, session=context.Sessions.Find(1)},
            //    new Student{ID=2, Voted=false, session=context.Sessions.Find(2)}
            //};

            //students.ForEach(s => context.Students.Add(s));
            //context.SaveChanges();
        }
    }
}