using System.Threading.Tasks;

namespace CrudDemo.Persistence.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
