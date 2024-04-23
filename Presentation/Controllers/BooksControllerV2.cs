﻿using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    //[ApiVersion("2.0", Deprecated = true)]
    [ApiController]
    [Route("api/books")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class BooksControllerV2 : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksControllerV2(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookAsync()
        {
            var books = await _manager.BookService.GetAllBooksAsync(false);
            var booksV2 = books.Select(b => new
            {
                Title = b.Title,
                Id = b.Id
            });
            return Ok(booksV2);
        }
    }
}
