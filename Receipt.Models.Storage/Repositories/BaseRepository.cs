using System.Linq.Expressions;
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
    
    public TModel? Get(Guid id, bool allowDeleted = false)
    {
        return this.Set.FirstOrDefault(x => x.Id == id && (allowDeleted || x.DeletedAt == null));
    }

    public void Add(TModel model)
    {
        model.CreatedAt = DateTime.Now;
        this.Set.Add(model);
        this._context.SaveChanges();
    }

    public void Add(ICollection<TModel> models)
    {
        foreach (var model in models)
        {
            model.CreatedAt = DateTime.Now;
            this.Set.Add(model);
        }
        
        this._context.SaveChanges();
    }
}