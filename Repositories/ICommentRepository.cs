
using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertismentBoard.Repositories;

public interface ICommentRepository
{
    Task CreateComment(Comment model);
    Task<Comment?> GetCommentById(int id);
    Task DeleteComment(Comment comment);
    Task UpdateComment(Comment comment);
    Task<IEnumerable<Comment>> GetCommentsByAdId(int id);
}

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Comment> _comments;
    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
        _comments = context.Set<Comment>();
    }

    public async Task CreateComment(Comment model)
    {
        await _comments.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(Comment comment)
    {
        _comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment?> GetCommentById(int id)
    {
        return await _comments.FindAsync(id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByAdId(int id)
    {
        return await _comments.Where(com => com.AdvertismentId == id).OrderBy(com => com.CreatedAt).ToListAsync();
    }

    public async Task UpdateComment(Comment comment)
    {
        _comments.Update(comment);
        await _context.SaveChangesAsync();
    }
}