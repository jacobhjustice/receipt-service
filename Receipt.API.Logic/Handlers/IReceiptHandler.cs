using Receipt.API.DTOs.Requests;

namespace Receipt.API.Logic.Handlers;

public interface IReceiptHandler
{
    Models.Data.Receipt Get(Guid receiptId);
    Models.Data.Receipt Process(ProcessReceiptRequest request);
}