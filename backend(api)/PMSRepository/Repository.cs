﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;

namespace PMSRepository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        PMSDBContext context = new PMSDBContext();
        public List<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public TEntity Get(object id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public int Insert(TEntity entity)
        {
            try
            {
                context.Set<TEntity>().Add(entity);
                }
            catch (Exception ex)
            {
                string exp = ex.StackTrace;
            } return context.SaveChanges();
            
        }

        public int Update(TEntity entity)
        {
            context.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            return context.SaveChanges();
        }
    }
}
