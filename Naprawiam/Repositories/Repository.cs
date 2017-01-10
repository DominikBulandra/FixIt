using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Naprawiam.Repositories
{
    public interface IIdentity
    {
        int Id { get; set; }
    }
    public interface IRepository<Type> where Type : IIdentity
    {
        Type Get(int id);
        IEnumerable<Type> GetAll();
        void Create(Type item);
        void Delete(Type item);
        void Update(Type item);
    }
    public class Repository<Type> : IRepository<Type> where Type : IIdentity
    {
        public static List<Type> _data = new List<Type>();
        public virtual Type Get(int id)
        {
            return _data.Single(r => r.Id == id);
        }
        public virtual IEnumerable<Type> GetAll()
        {
            return _data;
        }
        public virtual void Create(Type item)
        {
            int newId;
            if (_data.Count() == 0)
                newId = 1;
            else
                newId = _data.Max(r => r.Id) + 1;

            item.Id = newId;
            _data.Add(item);
        }
        public virtual void Delete(Type item)
        {
            _data.Remove(item);
        }
        public virtual void Update(Type item)
        {
            throw new NotImplementedException();
        }
    }
}