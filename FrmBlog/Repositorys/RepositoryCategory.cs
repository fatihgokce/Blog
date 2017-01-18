using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrmBlog.Models;

namespace FrmBlog.Repositorys
{
    public class RepositoryCategory:BaseRepository,IRepository<Category>
    {
        public RepositoryCategory(string pathDatabase, DbType dbType)
            : base(pathDatabase,dbType)
        { }

        #region IRepository<Category> Members

        public void Insert(Category item)
        {
            throw new NotImplementedException();
        }

        public void Update(Category item)
        {
            throw new NotImplementedException();
        }

        public Category GetById(long id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}