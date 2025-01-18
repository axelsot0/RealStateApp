namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<TRequest, TResponse, TEntity>
        where TRequest : class
        where TResponse : class
        where TEntity : class
    {
        Task<TResponse> Add(TRequest request);
        Task<TResponse?> Update(TResponse request, int id);
        Task Delete(int id);
        Task<TResponse?> GetById(int id);
        Task<List<TResponse>> GetAll();
    }
}
