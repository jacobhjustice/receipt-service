using FluentValidation;
using Receipt.API.DTOs.Requests;
using Receipt.API.DTOs.ViewModels;
using Receipt.Models.Storage.Repositories;

namespace Receipt.API.Logic.Handlers;

public class ReceiptHandler : IReceiptHandler
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

    public Models.Data.Receipt Get(Guid receiptId)
    {
        var receipt = this._receiptRepository.Get(receiptId);
        if (receipt == null)
        {
            throw new KeyNotFoundException();
        }

        return receipt;
    }

    public Models.Data.Receipt Process(ProcessReceiptRequest request)
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

        var purchaseDate = DateOnly.Parse(request.PurchaseDate);                                                              
        var purchaseTime = TimeOnly.Parse(request.PurchaseTime);                                                              
        
        int pointTotal = this.PointsForRetailer(request.Retailer);
        pointTotal += this.PointsForReceiptTotal(request.Total);
        pointTotal += this.PointsForReceiptItems(request.Items);
        pointTotal += this.PointsForPurchaseDate(purchaseDate);
        pointTotal += this.PointsForPurchaseTime(purchaseTime);

        var receipt = new Models.Data.Receipt
        {
            PointsAwarded = pointTotal,
            Retailer = request.Retailer,
            PurchasedAt = new DateTime(purchaseDate.Year, purchaseDate.Month, purchaseDate.Day,
                purchaseTime.Hour, purchaseTime.Minute, purchaseTime.Second),
            Total = request.Total,
            CreatedAt = DateTime.Now,
            DeletedAt = null
        };
        
        this._receiptRepository.Add(receipt);
        this._receiptItemRepository.Add(request.Items.Select(x => new Models.Data.ReceiptItem
        {
            ReceiptId = receipt.Id,
            ShortDescription = x.ShortDescription,
            Price = x.Price,
            CreatedAt = DateTime.Now,
            DeletedAt = null
        }).ToList());

        return receipt;
    }
    
    private int PointsForRetailer(string retailer)
    {
        var points = 0;

        // For every alphanumeric character in the retailer name, add 1 pt
        points += retailer.Count(char.IsLetterOrDigit);

        return points;
    }

    private int PointsForReceiptTotal(decimal receiptTotal)
    {
        var points = 0;
        
        // If the total is a round dollar amount, add 50 pts
        if (receiptTotal % 1 == 0)
        {
            points += 50;
        }

        // If the total is a multiple of 0.25, add 25 pts
        if ((receiptTotal * 4) % 1 == 0)
        {
            points += 25;
        }

        return points;
    }

    private int PointsForReceiptItems(IList<ReceiptItemViewModel> receiptItems)
    {
        var points = 0;
        
        // For every two items on the receipt, add 5 pts
        points += (int) Math.Floor(receiptItems.Count() / 2.0) * 5;

        
        // For all items with a description of length divisible by 3, add double the price as pts, rounded up
        foreach (var item in receiptItems)
        {
            if (item.ShortDescription.Trim().Length % 3 == 0)
            {
                points += (int) Math.Ceiling(item.Price * (decimal) 0.2);
            }
        }
        
        return points;
    }

    private int PointsForPurchaseDate(DateOnly purchaseDate)
    {
        var points = 0;

        // If the day is odd, add 6pts
        if (purchaseDate.Day % 2 == 1)
        {
            points += 6;
        }

        return points;
    }
    
    private int PointsForPurchaseTime(TimeOnly purchaseTime)
    {
        var points = 0;

        // If the time is between 2:00 PM and 4:00 PM, add 10 pts
        if (purchaseTime >= new TimeOnly(14, 0) && purchaseTime <= new TimeOnly(16, 0))
        {
            points += 10;
        }

        return points;
    }
}