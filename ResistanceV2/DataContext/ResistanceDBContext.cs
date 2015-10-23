using ResistanceV2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ResistanceV2.DataContext
{
    public class ResistanceDBContext: DbContext
    {


        public ResistanceDBContext()
            : base("ResistanceDBContext")
        {
           
            Database.SetInitializer<ResistanceDBContext>(new DropCreateDatabaseIfModelChanges<ResistanceDBContext>());
        }
        public DbSet<UserModel> User { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Comments> Comment { get; set; }
        public DbSet<SavedMessages> SavedMessages {get;set;}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
    }
}