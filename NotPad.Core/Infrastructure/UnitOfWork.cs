using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotPad.Core.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory databaseFactory;
        private NotPadDbContext dataContext;

        public UnitOfWork(IDatabaseFactory _databaseFactory)
        {
            this.databaseFactory = _databaseFactory;
        }

        public NotPadDbContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }

    public interface IUnitOfWork
    {
        void Commit();
    }
}
