public class Invoice
{
    public int Id { get; set; }
    public string Customer { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public int Fee { get; set; }
    public Status Status { get; set; } 

    // Constructor
    /*
    public Invoice(int id, string customer, decimal amount, DateTime dueDate, int fee, Status status)
    {
        Id = id;
        Customer = customer;
        Amount = amount;
        DueDate = dueDate;
        Fee = fee;
        Status = status;
    }
    */
}

public enum Status
{
    Pending,
    Paid
}

