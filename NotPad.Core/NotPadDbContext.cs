using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NotPad.Core
{
    public class NotPadDbContext : DbContext
    {
        public NotPadDbContext() : base("Default")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
            Database.SetInitializer<NotPadDbContext>(null);

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static NotPadDbContext Create()
        {
            return new NotPadDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException("modelBuilder");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += string.Format("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage) + " / ";
                    }
                }
                
                throw dbEx;
            }
        }

        public DbSet<Note> Notes { get; set; }
    }
}
