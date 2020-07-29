using CrudDemo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo.Interfaces
{
    public interface IGlossaryItemsService
    {
        public Task<GlossaryItemDto> AddAsync(GlossaryItemAddDto glossaryItem);
        public Task<GlossaryItemDto> UpdateAsync(GlossaryItemUpdateDto glossaryItem, int id);
        public Task DeleteAsync(int id);
        public Task<List<GlossaryItemDto>> GetOrderedListAsync();
    }
}
