using AssessmentService.Models;

namespace AssessmentService.Services
{

  public interface IAssessmentRepository
  {
    public Task<Item> GetItemById(string itemId);
    public Task<IEnumerable<Item>> GetAllRegistredItems();
    public Task UpdateItem(Item item);

  }
}
