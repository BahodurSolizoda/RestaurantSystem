namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TableId { get; set; }
    public string Status { get; set; }
}