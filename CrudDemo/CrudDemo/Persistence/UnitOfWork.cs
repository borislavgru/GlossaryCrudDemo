using CrudDemo.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GlossaryDBContext _dbContext;
        public UnitOfWork(GlossaryDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
