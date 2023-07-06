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
        var testData1 = this.TestData(false);
        this.AddData(ctx, testData1);
        var testData2 = this.TestData(true);
        this.AddData(ctx, testData2);
        var repository = this._repository(ctx);

        var result1 = repository.Get(testData1.Id, false);
        Assert.NotNull(result1);
        Assert.Equal(testData1.Id, result1.Id);
        
        var result2 = repository.Get(testData2.Id, false);
        Assert.Null(result2);
        result2 = repository.Get(testData2.Id, true);
        Assert.NotNull(result1);
        Assert.Equal(testData2.Id, result2.Id);
        
        var guid3 = Guid.Empty;
        var result3 = repository.Get(guid3, false);
        Assert.Null(result3);
    }
    
    [Fact]
    public void TestAdd()
    {
        var ctx = this.SetupContext();
        var data = this.TestData(false);
        var preExeDate = DateTime.Now;
        this._repository(ctx).Add(data);
        var postExeDate = DateTime.Now;
        Assert.Equal(1, ctx.Set<TModel>().Count());
        Assert.True(data.CreatedAt > preExeDate && data.CreatedAt < postExeDate);
        Assert.NotEqual(data.Id, Guid.Empty);
    }

    [Fact]
    public void TestAddRange()
    {
        var ctx = this.SetupContext();
        var data = new List<TModel>
        {
            this.TestData(false),
            this.TestData(false)
        };
        var preExeDate = DateTime.Now;
        this._repository(ctx).Add(data);
        var postExeDate = DateTime.Now;
        Assert.Equal(2, ctx.Set<TModel>().Count());
        foreach (var model in data)
        {
            Assert.True(model.CreatedAt > preExeDate && model.CreatedAt < postExeDate);
            Assert.NotEqual(model.Id, Guid.Empty);
        }
    }
}