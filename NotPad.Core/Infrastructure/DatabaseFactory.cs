using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotPad.Core.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private NotPadDbContext dataContext;

        public NotPadDbContext Get()
        {
            return dataContext ?? (dataContext = new NotPadDbContext());
        }

        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }

    public interface IDatabaseFactory : IDisposable
    {
        NotPadDbContext Get();
    }
}
