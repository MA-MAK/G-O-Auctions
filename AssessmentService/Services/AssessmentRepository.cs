using System.Collections.Generic;
using System.Threading.Tasks;
using AssessmentService.Models;

namespace AssessmentService.Services
{
  
    public class AssessmentRepository : IAssessmentRepository
    {

        private List<Item> items;

        public AssessmentRepository()
        {
            items = new List<Item>();
        }

        public Task<Item> GetItemById(string itemId)
        {
            return Task.FromResult(items.FirstOrDefault(i => i.Id == itemId));
        }

        public Task<IEnumerable<Item>> GetAllRegistredItems()
        {
            return Task.FromResult<IEnumerable<Item>>(items); 
            
        }

        public Task PostItem(Item item)
        {
        
            items.Add(item);
            return Task.CompletedTask;
        }
    }
}
