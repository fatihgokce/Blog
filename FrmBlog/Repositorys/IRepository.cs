
namespace FrmBlog.Repositorys
{
    public interface IRepository<T>
    {
        void Insert(T item);
        void Update(T item);
        T GetById(long id);
    }
}