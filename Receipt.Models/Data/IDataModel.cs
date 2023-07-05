namespace Receipt.Models.Data;

public interface IDataModel
{
    Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}