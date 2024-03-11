using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogFilterAttribute))]


    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
        {
            var pagedResult = await _manager.BookService.GetAllBooksAsync(bookParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            #region MyRegion
            //if (bookDto is null)
            //{
            //    return BadRequest(); //400        // service filter ekledik
            //}
            #endregion

            var book = await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate book)
        {
            #region MyRegion
            //if (book is null)
            //{
            //    return BadRequest(); //400
            //}

            //if (!ModelState.IsValid)
            //{
            //    return UnprocessableEntity(ModelState);  //422
            //}
            #endregion

            await _manager.BookService.UpdateOneBookAsync(id, book, false);

            return NoContent(); //204

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            try
            {
                await _manager.BookService.DeleteOneBookAsync(id, false);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            #region MyRegion
            //// check entity
            //var bookDto = _manager.BookService.GetOneBookById(id, true);

            //if (entity is null)
            //    return NotFound();  //404
            #endregion


            if (bookPatch is null)
            {
                return BadRequest(); //400
            }

            var result = await _manager.BookService.GetOneBookForPatchAsycn(id, false);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);
            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }


            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

            return NoContent();     //204
        }

    }

}