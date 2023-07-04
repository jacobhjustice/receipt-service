using Microsoft.EntityFrameworkCore;
using Receipt.Models.Data;

namespace Receipt.Models.Storage.Repositories;

public abstract class BaseRepository<TModel> : IRepository<TModel> where TModel :  class, IDataModel
{
    private readonly DbContext _context;
    protected DbSet<TModel> Set => this._context.Set<TModel>();
    public BaseRepository(ReceiptContext ctx)
    {
        this._context = ctx;
    }
    
    public TModel? Get(Guid id)
    {
        return this.Set.FirstOrDefault(x => x.Id == id);
    }

    public void Add(TModel model)
    {
        this.Set.Add(model);
        this._context.SaveChanges();
    }
}