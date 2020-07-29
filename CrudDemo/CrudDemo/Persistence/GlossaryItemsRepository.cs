using CrudDemo.Models;
using CrudDemo.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo.Persistence
{
    public class GlossaryItemsRepository : IGlossaryRepository
    {
        private readonly GlossaryDBContext _dbContext;

        public GlossaryItemsRepository(GlossaryDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(GlossaryItem glossaryItem)
        {
            await _dbContext.AddAsync(glossaryItem);
        }

        public void Remove(GlossaryItem glossaryItem)
        {
            _dbContext.GlossaryItems.Remove(glossaryItem);
        }

        public void Update(GlossaryItem glossaryItem)
        {
            _dbContext.GlossaryItems.Update(glossaryItem);
        }

        public async Task<GlossaryItem> FindByIdAsync(int id)
        {
            var glossaryItem = _dbContext
                .GlossaryItems
                .FirstOrDefaultAsync(x => x.Id == id);

            return await glossaryItem;
        }

        public Task<GlossaryItem> FindByTermAsync(string term)
        {
            var glossaryItem = _dbContext
                .GlossaryItems
                .FirstOrDefaultAsync(x => x.Term == term);

            return glossaryItem;
        }

        public async Task<List<GlossaryItem>> GetAllReadyOnlyOrderedAsync()
        {
            var itemsTask = _dbContext
                .GlossaryItems
                .OrderBy(x => x.Term)
                .AsNoTracking()
                .ToListAsync();

            return await itemsTask;
        }

        public async Task<List<GlossaryItem>> GetAllAsync()
        {
            var itemsTask = _dbContext
                .GlossaryItems
                .AsNoTracking()
                .ToListAsync();

            return await itemsTask;
        }
    }
}
