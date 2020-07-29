using CrudDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo.Persistence.Interfaces
{
    public interface IGlossaryRepository
    {
        Task<List<GlossaryItem>> GetAllAsync();
        Task<List<GlossaryItem>> GetAllReadyOnlyOrderedAsync();
        Task AddAsync(GlossaryItem glossaryItem);
        void Update(GlossaryItem glossaryItem);
        void Remove(GlossaryItem glossaryItem);
        Task<GlossaryItem> FindByIdAsync(int id);
        Task<GlossaryItem> FindByTermAsync(string term);
    }
}
