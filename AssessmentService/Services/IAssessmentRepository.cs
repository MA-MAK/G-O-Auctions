using AssessmentService.Models;

namespace AssessmentService.Services
{

  public interface IAssessmentRepository
  {
    public Task<Item> GetAuctionById(int ItemId);
    public Task<IEnumerable<Item>> GetAllRegistredItems();
    public Task PostItem(Auction auction);

  }
}
