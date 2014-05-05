using System.Data.Entity;
using PathToSuccess.Models;

namespace PathToSuccess.DAL
{
    public class Context : DbContext
    {
        public DbSet GetDbSet<T>() where T : class
        {
            var set = Set<T>();
            set.Load();
            return set;
        }

        public Context(System.Data.Common.DbConnection connection)
            :base(connection,true)
        {
            Database.SetInitializer<Context>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("public.users");
            modelBuilder.Entity<Urgency>().ToTable("public.urgency");
            modelBuilder.Entity<Tree>().ToTable("public.tree");
            modelBuilder.Entity<TimeRule>().ToTable("public.timerule");
            modelBuilder.Entity<TimeBinding>().ToTable("public.time_binding");
            modelBuilder.Entity<Task>().ToTable("public.task");
            modelBuilder.Entity<Step>().ToTable("public.step");
            modelBuilder.Entity<Models.Schedule>().ToTable("public.schedule");
            modelBuilder.Entity<Interval>().ToTable("public.interval");
            modelBuilder.Entity<Importance>().ToTable("public.importance");
            modelBuilder.Entity<Criteria>().ToTable("public.criteria");
        }
    }
}
