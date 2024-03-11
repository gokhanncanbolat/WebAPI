using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookManager(
            IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdAndCheckExits(id, trackChanges);
            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public async Task<(IEnumerable<BookDto> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            if (!bookParameters.ValidPrice)
                throw new PriceOutOfRangeBadRequest();

            var booksWithMetaData = await _manager.Book.GetAllBooksAsync(bookParameters, trackChanges);

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

            return (booksDto, booksWithMetaData.MetaData);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExits(id, trackChanges);

            return _mapper.Map<BookDto>(book);
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entity = await GetOneBookByIdAndCheckExits(id, trackChanges);

            #region MyRegion
            //// check entity
            //var entity = await _manager.Book.GetOneBookByIdAsync(id, true);
            //if (entity is null)
            //{
            //    //string message = $"Book with id : {id} could not found";
            //    //_logger.LogInfo(message);
            //    throw new BookNotFoundException(id);
            //}

            // Mapping
            //entity.Title = book.Title;
            //entity.Price = book.Price;

            // AutoMapping
            #endregion

            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }
        async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> IBookService.GetOneBookForPatchAsycn(int id, bool trackChanges)
        {
            var book = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task<Book> GetOneBookByIdAndCheckExits(int id, bool trackChanges)
        {
            // check entity
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            return entity;
        }

    }
}
