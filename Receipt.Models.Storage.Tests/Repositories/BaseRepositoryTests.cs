using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Receipt.Models.Data;
using Receipt.Models.Storage.Repositories;
using Xunit;

namespace Receipt.Models.Storage.Tests.Repositories;

public abstract class BaseRepositoryTests<TModel> where TModel : class, IDataModel, new()
{
    protected abstract BaseRepository<TModel> _repository(ReceiptContext ctx);
    protected abstract TModel TestData(bool isDeleted);
    private ReceiptContext SetupContext()
    {
        return new ReceiptContext();
    }
    
    protected void AddData(ReceiptContext ctx, TModel dataToAdd)
    {
        ctx.Set<TModel>().Add(dataToAdd);
        ctx.SaveChanges();
    }
    
    [Fact]
    public void TestGet()
    {
        var ctx = this.SetupContext();
        ctx.Set<TModel>() = new InternalDbSet<TModel>()
        var testData1 = this.TestData(false);
        this.AddData(ctx, testData1);
        var testData2 = this.TestData(true);
        this.AddData(ctx, testData2);
        var repository = this._repository(ctx);

        var result1 = repository.Get(testData1.Id, false);
        Assert.NotNull(result1);
        Assert.Equal(guid1, result1.Id);
        
        var guid2 = Guid.Parse("4ee1bdbb-2686-492d-97d4-c99c226bb689");
        var result2 = repository.Get(guid2, false);
        Assert.Null(result2);
        result2 = repository.Get(guid2, true);
        Assert.NotNull(result1);
        Assert.Equal(guid2, result2.Id);
        
        var guid3 = Guid.Parse("b4304ade-b861-4048-bb58-e1e216a5e10c");
        var result3 = repository.Get(guid3, false);
        Assert.Null(result3);
    }
    
    [Fact]
    public void TestAdd()
    {
        var ctx = this.SetupContext();
        this._repository(ctx).Add(this.TestData(Guid.NewGuid(), false));
        Assert.Equal(1, ctx.Set<TModel>().Count());
    }
}