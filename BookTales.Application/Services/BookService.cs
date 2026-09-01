using AutoMapper;
using BookTales.Application.DTOs.Book;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;

namespace BookTales.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public BookService(
        IBookRepository bookRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    public async Task<BookResponseDto?> GetByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
            return null;

        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task<IEnumerable<BookResponseDto>> SearchAsync(string search)
    {
        var books = await _bookRepository.SearchAsync(search);

        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    public async Task<IEnumerable<BookResponseDto>> GetByCategoryAsync(
        Guid categoryId)
    {
        var books = await _bookRepository.GetByCategoryAsync(categoryId);

        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    public async Task<(IEnumerable<BookResponseDto> Books, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize)
    {
        var result = await _bookRepository.GetPagedAsync(
            pageNumber,
            pageSize);

        var books = _mapper.Map<IEnumerable<BookResponseDto>>(result.Books);

        return (books, result.TotalCount);
    }

    public async Task<BookResponseDto> CreateAsync(
        CreateBookDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new InvalidOperationException(
                "Book title is required.");

        if (string.IsNullOrWhiteSpace(request.Author))
            throw new InvalidOperationException(
                "Book author is required.");

        if (request.Price <= 0)
            throw new InvalidOperationException(
                "Book price must be greater than zero.");

        if (request.Stock < 0)
            throw new InvalidOperationException(
                "Book stock cannot be negative.");

        if (string.IsNullOrWhiteSpace(request.ISBN))
            throw new InvalidOperationException(
                "ISBN is required.");

        var category = await _categoryRepository.GetByIdAsync(
            request.CategoryId);

        if (category == null)
            throw new InvalidOperationException(
                "Category not found.");

        var book = _mapper.Map<Book>(request);

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();

        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task<bool> UpdateAsync(
    Guid id,
    UpdateBookDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new InvalidOperationException(
                "Book title is required.");

        if (string.IsNullOrWhiteSpace(request.Author))
            throw new InvalidOperationException(
                "Book author is required.");

        if (request.Price <= 0)
            throw new InvalidOperationException(
                "Book price must be greater than zero.");

        if (request.Stock < 0)
            throw new InvalidOperationException(
                "Book stock cannot be negative.");

        if (string.IsNullOrWhiteSpace(request.ISBN))
            throw new InvalidOperationException(
                "ISBN is required.");

        var category = await _categoryRepository.GetByIdAsync(
            request.CategoryId);

        if (category == null)
            throw new InvalidOperationException(
                "Category not found.");

        var book = await _bookRepository.GetByIdForUpdateAsync(id);

        if (book == null)
            return false;

        _mapper.Map(request, book);

        book.CategoryId = request.CategoryId;

        _bookRepository.Update(book);

        await _bookRepository.SaveChangesAsync();

        return true;
    }

    //public async Task<bool> DeleteAsync(Guid id)
    //{
    //    var book = await _bookRepository.GetByIdAsync(id);

    //    if (book == null)
    //        return false;

    //    _bookRepository.Delete(book);

    //    await _bookRepository.SaveChangesAsync();

    //    return true;
    //}

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
            return false;

        try
        {
            _bookRepository.Delete(book);

            await _bookRepository.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("DELETE BOOK ERROR:");
            Console.WriteLine(ex.ToString());

            throw;
        }
    }
}