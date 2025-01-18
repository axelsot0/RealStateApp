using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.Core.Application.Services
{
    public class GenericService<TRequest, TResponse, TEntity> : IGenericService<TRequest, TResponse, TEntity>
        where TRequest : class
        where TResponse : class
        where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TResponse> Add(TRequest request)
        {
            TEntity entity = _mapper.Map<TEntity>(request);

            var response = await _repository.AddAsync(entity);

            return _mapper.Map<TResponse>(response);
        }

        public virtual async Task Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new Exception($"The entity with the id = {id} doesn't exist");
            }

            await _repository.DeleteAsync(entity);
        }

        public virtual async Task<List<TResponse>> GetAll()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<TResponse>>(entities);
        }

        public virtual async Task<TResponse?> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<TResponse>(entity);
        }

        public virtual async Task<TResponse?> Update(TResponse request, int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                return null;
            }

            _mapper.Map(request, entity);

            await _repository.UpdateAsync(entity, id);

            return _mapper.Map<TResponse>(entity);
        }
    }
}
