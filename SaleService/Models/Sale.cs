namespace SaleService.Models;
public class Sale
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public Customer Seller { get; set; }
    public Customer Buyer { get; set; }
    public string ItemId { get; set; }
}
