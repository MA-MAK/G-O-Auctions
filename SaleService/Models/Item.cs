namespace SaleService.Models;
public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal StartPrice { get; set; }
    public decimal AssesmentPrice { get; set; }
    public string? Description { get; set; }
    public int Year { get; set; }
    public string Location { get; set; }
    public Customer Seller { get; set; }
    public Category Category { get; set; }
    public Condition Condition { get; set; }
    public Status Status { get; set; }
    public int AuctionId { get; set; }

}
public enum Category
{
    Electronics,
    Fashion,
    Home,
    Garden,
    Sports,
    Toys,
    Vehicles,
    Other
}
public enum Condition
{
    New,
    Good,
    Fair,
    Poor
}
public enum Status
{
    Registered,
    ReadyForAuction,
    Auctioning,
    Sold,
    NotSold,
}
