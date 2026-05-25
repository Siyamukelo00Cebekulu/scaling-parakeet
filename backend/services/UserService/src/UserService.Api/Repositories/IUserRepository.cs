using UserService.Api.Models;

namespace UserService.Api.Repositories;

/// <summary>
/// Repository contract for reading and writing users and refresh tokens.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by its identifier.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);

    /// <summary>
    /// Adds a new user entity to the repository.
    /// </summary>
    Task AddAsync(User user, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing user entity.
    /// </summary>
    Task UpdateAsync(User user, CancellationToken ct = default);

    /// <summary>
    /// Persists pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets a refresh token by its token value.
    /// </summary>
    Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default);

    /// <summary>
    /// Adds a refresh token to the repository.
    /// </summary>
    Task AddRefreshTokenAsync(RefreshToken token, CancellationToken ct = default);

    /// <summary>
    /// Marks a refresh token as revoked.
    /// </summary>
    Task RevokeRefreshTokenAsync(RefreshToken token, CancellationToken ct = default);
}
