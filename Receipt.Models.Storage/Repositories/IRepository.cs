namespace Receipt.Models.Storage.Repositories;

public interface IRepository<TModel> 
{
    TModel? Get(Guid id);
    void Add(TModel model);
}