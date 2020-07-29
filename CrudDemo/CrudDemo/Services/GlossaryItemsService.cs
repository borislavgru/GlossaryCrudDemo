using AutoMapper;
using CrudDemo.DTO;
using CrudDemo.Exceptions;
using CrudDemo.Interfaces;
using CrudDemo.Models;
using CrudDemo.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo.Services
{
    public class GlossaryItemsService : IGlossaryItemsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGlossaryRepository _glossaryRepository;

        public GlossaryItemsService(IGlossaryRepository glossaryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _glossaryRepository = glossaryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GlossaryItemDto> AddAsync(GlossaryItemAddDto glossaryItemDto)
        {
            try
            {
                var item = _mapper.Map<GlossaryItemAddDto, GlossaryItem>(glossaryItemDto);
                await _glossaryRepository.AddAsync(item);
                _unitOfWork.Commit();
                var glossaryItemResultDto = _mapper.Map<GlossaryItem, GlossaryItemDto>(item);
                return glossaryItemResultDto;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException as SqlException;
                if (innerException != null && innerException.Number == 2601)
                {
                    throw new DuplicateRecordException($"Term '{glossaryItemDto.Term}' already exists");
                }
                else 
                {
                    throw;
                }
            }
        }

        public async Task<GlossaryItemDto> UpdateAsync(GlossaryItemUpdateDto glossaryUpdateItem, int id)
        {
            GlossaryItem itemForUpdate = await GetItemIfExistsAsync(id);
            itemForUpdate.Definition = glossaryUpdateItem.Definition;
            _unitOfWork.Commit();

            var glossaryItemResultDto = _mapper.Map<GlossaryItem, GlossaryItemDto>(itemForUpdate);
            return glossaryItemResultDto;
        }

        public async Task DeleteAsync(int id)
        {
            GlossaryItem itemForRemoval = await GetItemIfExistsAsync(id);
            _glossaryRepository.Remove(itemForRemoval);
            _unitOfWork.Commit();
        }

        public async Task<List<GlossaryItemDto>> GetOrderedListAsync()
        {
            var items = await _glossaryRepository.GetAllReadyOnlyOrderedAsync();
            var result = _mapper.Map<List<GlossaryItem>, List<GlossaryItemDto>> (items);
            return result;
        }

        private async Task<GlossaryItem> GetItemIfExistsAsync(int id)
        {
            var item = await _glossaryRepository.FindByIdAsync(id);
            if (item == null)
            {
                throw new NonExistingItemException($"Item with id = {id} does not exist");
            }

            return item;
        }
    }
}
