namespace Receipt.Models.Storage.Repositories;

public interface IRepository<TModel> 
{
    TModel? Get(Guid id, bool allowDeleted = false);
    void Add(TModel model);
}