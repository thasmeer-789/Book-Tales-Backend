using BookTales.Application.Interfaces.Repositories;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all books - Read-only query
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .ToListAsync();
    }

    // Get multiple books in a single database query
    // Used to avoid N+1 queries during order creation
    public async Task<List<Book>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(b => ids.Contains(b.Id))
            .ToListAsync();
    }

    // Get a single book by ID - Read-only query
    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    // Get a single book for update - tracked entity
    public async Task<Book?> GetByIdForUpdateAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    // Search books - Read-only query
    public async Task<IEnumerable<Book>> SearchAsync(string search)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .Where(b =>
                b.Title.Contains(search) ||
                b.Author.Contains(search) ||
                b.ISBN.Contains(search))
            .ToListAsync();
    }

    // Get books by category - Read-only query
    public async Task<IEnumerable<Book>> GetByCategoryAsync(Guid categoryId)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .Where(b => b.CategoryId == categoryId)
            .ToListAsync();
    }

    // Get paginated books - Read-only query
    public async Task<(IEnumerable<Book> Books, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize)
    {
        var query = _context.Books
            .AsNoTracking()
            .Include(b => b.Category);

        var totalCount = await query.CountAsync();

        var books = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (books, totalCount);
    }

    // Add a new book
    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    // Update an existing book
    public void Update(Book book)
    {
        _context.Books.Update(book);
    }

    // Delete a book
    public void Delete(Book book)
    {
        _context.Books.Remove(book);
    }

    // Save changes to database
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}