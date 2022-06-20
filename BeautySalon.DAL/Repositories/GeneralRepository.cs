using BeautySalon.DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.DAL.Repositories
{
    public class GeneralRepository<T>
      where T : class
    {
        SalonContext context;
        DbSet<T> dbSet;
        public GeneralRepository(SalonContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        public virtual IEnumerable<T> GetAll() => dbSet;
        public virtual T GetByID(int id) => dbSet.Find(id);
        public virtual void CreateOrUpdate(T entity) => dbSet.AddOrUpdate(entity);
        public virtual void Delete(T entity) => dbSet.Remove(entity);
        public void Save() => context.SaveChanges();

    }
}
