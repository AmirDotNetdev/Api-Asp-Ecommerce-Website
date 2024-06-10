using System.Collections;

namespace TestApi.Repositories.IRepositories
{
    public interface IRepository<TModel> where TModel : class
    {
        Task<ICollection<TModel>> GetAll();
    }
}
