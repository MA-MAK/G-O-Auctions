using AssessmentService.Models;

namespace AssessmentService.Services
{

  public interface IAssessmentRepository
  {
    public Task<Item> GetItemById(string itemId);
    public Task<IEnumerable<Item>> GetAllRegistredItems();
    public Task UpdateItem(string itemId, string description, int year, decimal assessmentPrice, int category, int condition, int status, string title);

  }
}
