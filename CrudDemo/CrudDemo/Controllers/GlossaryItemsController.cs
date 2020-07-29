using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudDemo.DTO;
using CrudDemo.Interfaces;
using CrudDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossaryItemsController : ControllerBase
    {
        private readonly IGlossaryItemsService _glossaryService;

        public GlossaryItemsController(IGlossaryItemsService glossaryService)
        {
            _glossaryService = glossaryService;
        }

        /// <summary>
        /// Deletes an item with provided id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Delete /GlossaryItems/1
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Returns No content if succesful delete</response>
        /// <response code="404">If the item with provided id is not found</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _glossaryService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Saves provided glossary item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /GlossaryItems
        ///     {
        ///        "term": "abyssal plain",
        ///        "definition": The ocean floor offshore from the continental margin, usually very flat with a slight slope.
        ///     }
        /// </remarks>
        /// <param name="glossaryItem"></param>
        /// <returns>A newly created glossary item</returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the item properties are not valid</response> 
        /// <response code="409">If term already exists</response> 
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> Post(GlossaryItemAddDto glossaryItem)
        {
            var addedItem = await _glossaryService.AddAsync(glossaryItem);
            return Ok(addedItem);
        }

        /// <summary>
        /// Updates the definition of glossary item with provided id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /GlossaryItems/1
        ///     {
        ///        "definition": The ocean floor offshore from the continental margin, usually very flat with a slight slope.
        ///     }
        /// </remarks>
        /// <param name="glossaryItemForUpdate"></param>
        /// <param name="id"></param>
        /// <returns>Updated glossary item</returns>
        /// <response code="200">Returns the updated glossary item</response>
        /// <response code="404">If the item is not found</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(GlossaryItemUpdateDto glossaryItemForUpdate, int id)
        {
            var updatedItem = await _glossaryService.UpdateAsync(glossaryItemForUpdate, id);
            return Ok(updatedItem);
        }

        /// <summary>
        /// Retrieves the list of glossary items ordered by term name.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the list of all glossary items ordered by term</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var items = await _glossaryService.GetOrderedListAsync();
            return Ok(items);
        }

    }
}
