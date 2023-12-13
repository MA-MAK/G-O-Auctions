
public class Sale
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public Customer Seller { get; set; }
    public Customer Buyer { get; set; }
    public Item Item { get; set; }
    public Auction Auction { get; set; }
    public Partner AuctionHouse { get; set; }

    // Constructor
    /*
    public Sale(int id, decimal amount, Customer seller, Customer buyer, Item item, Auction auction, Partner auctionHouse)
    {
        Id = id;
        Amount = amount;
        Seller = seller;
        Buyer = buyer;
        Item = item;
        Auction = auction;
        AuctionHouse = auctionHouse;
    }
    */
}
 