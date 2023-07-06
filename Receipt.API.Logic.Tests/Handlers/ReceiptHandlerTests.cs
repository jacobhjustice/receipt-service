using FluentValidation;
using FluentValidation.Results;
using Moq;
using Receipt.API.DTOs.Requests;
using Receipt.API.Logic.Handlers;
using Receipt.Models.Storage.Repositories;
using Xunit;

namespace Receipt.API.Logic.Tests.Handlers;

public class ReceiptHandlerTests
{
    private readonly Mock<IRepository<Models.Data.Receipt>> _receiptRepository;
    private readonly Mock<IRepository<Models.Data.ReceiptItem>> _receiptItemRepository;
    private readonly Mock<IValidator<ProcessReceiptRequest>> _processReceiptRequestValidator;

    public ReceiptHandlerTests()
    {
        this._receiptRepository = new Mock<IRepository<Models.Data.Receipt>>();
        this._receiptItemRepository = new Mock<IRepository<Models.Data.ReceiptItem>>();
        this._processReceiptRequestValidator = new Mock<IValidator<ProcessReceiptRequest>>();
    }

    private ReceiptHandler _handler() => new ReceiptHandler(this._receiptRepository.Object,
        this._receiptItemRepository.Object, this._processReceiptRequestValidator.Object);
    
    [Fact]
    public void ProcessTestFailure_Null()
    {
        Assert.Throws<ArgumentNullException>(() => _handler().Process(null));
    }
    
    [Fact]
    public void ProcessTestFailure_Invalid()
    {
        var request = new ProcessReceiptRequest();
        this._processReceiptRequestValidator.Setup(x => x.Validate(request))
            .Returns(new ValidationResult(new List<ValidationFailure>() {new ValidationFailure()}));
        Assert.Throws<InvalidDataException>(() => _handler().Process(request));
    }
}