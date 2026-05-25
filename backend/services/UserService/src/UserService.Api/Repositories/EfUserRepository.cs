using Microsoft.EntityFrameworkCore;
using UserService.Api.Data;
using UserService.Api.Models;

namespace UserService.Api.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly UserServiceDbContext _db;

    public EfUserRepository(UserServiceDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _db.Users.AddAsync(user, ct);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Username == username, ct);
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default)
    {
        return await _db.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == token, ct);
    }

    public async Task AddRefreshTokenAsync(RefreshToken token, CancellationToken ct = default)
    {
        await _db.RefreshTokens.AddAsync(token, ct);
    }

    public Task RevokeRefreshTokenAsync(RefreshToken token, CancellationToken ct = default)
    {
        token.IsRevoked = true;
        _db.RefreshTokens.Update(token);
        return Task.CompletedTask;
    }
}
