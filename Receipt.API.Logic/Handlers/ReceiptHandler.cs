using FluentValidation;
using Receipt.API.DTOs.Requests;
using Receipt.Models.Storage.Repositories;

namespace Receipt.API.Logic.Handlers;

public class ReceiptHandler
{
    private readonly IRepository<Models.Data.Receipt> _receiptRepository;
    private readonly IRepository<Models.Data.ReceiptItem> _receiptItemRepository;
    private readonly IValidator<ProcessReceiptRequest> _processReceiptRequestValidator;

    public ReceiptHandler(IRepository<Models.Data.Receipt> receiptRepository,
        IRepository<Models.Data.ReceiptItem> receiptItemRepository,
        IValidator<ProcessReceiptRequest> processReceiptRequestValidator)
    {
        this._receiptRepository = receiptRepository;
        this._receiptItemRepository = receiptItemRepository;
        this._processReceiptRequestValidator = processReceiptRequestValidator;
    }

    public void Process(ProcessReceiptRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = this._processReceiptRequestValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new InvalidDataException();
        }

        var date = DateOnly.Parse(request.PurchaseDate);                                                              
        var time = TimeOnly.Parse(request.PurchaseTime);                                                              
        var dt = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second); 
        
        int pointTotal = 0;
        pointTotal += request.Retailer.Count(char.IsLetterOrDigit);
        
        if (request.Total % 1 == 0)
        {
            pointTotal += 50;
        }

        if ((request.Total * 4) % 1 == 0)
        {
            pointTotal += 25;
        }

        pointTotal += (int) Math.Floor(request.Items.Count() / 2.0) * 5;

        foreach (var item in request.Items)
        {
            if (item.ShortDescription.Trim().Length % 3 == 0)
            {
                pointTotal += (int) Math.Ceiling(item.Price * (decimal) 0.2);
            }
        }

        if (date.Day % 2 == 1)
        {
            pointTotal += 6;
        }

        if (time >= new TimeOnly(14, 0) && time <= new TimeOnly(16, 0))
        {
            pointTotal += 10;
        }
    }
}