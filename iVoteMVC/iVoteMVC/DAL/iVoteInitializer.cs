using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iVoteMVC.Models;
using System.Data.Entity;

namespace iVoteMVC.DAL
{
    public class iVoteInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<iVoteContext>
    {
        protected override void Seed(iVoteContext context)
        {
           
        }
    }
}