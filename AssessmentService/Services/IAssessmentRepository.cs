using AssessmentService.Models;

namespace AssessmentService.Services
{

  public interface IAssessmentRepository
  {
    public Task<Item> GetItemById(string ItemId);
    public Task<IEnumerable<Item>> GetAllRegistredItems();
    public Task PostItem(Item Item);

  }
}
