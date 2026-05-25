# UserService API

This service provides user identity, authentication (JWT + refresh tokens), and customer profile management.

## Features
- JWT authentication with refresh tokens
- Simple in-memory rate limiting middleware
- Profile management (first/last name, phone, address, DOB, notification preferences)
- Repository pattern and DI
- Swagger UI configured for testing with JWT

## Configuration
Update `appsettings.json` or provide environment variables for:

- `ConnectionStrings:UserServiceConnnectionString` — SQL Server connection string
- `Jwt:Key` — symmetric signing key (set to a secure random value in production)
- `Jwt:Issuer`, `Jwt:Audience`, `Jwt:AccessTokenExpirationMinutes`, `Jwt:RefreshTokenExpirationDays`
- `RateLimiting:MaxRequests`, `RateLimiting:WindowSeconds`

## Running
From the `UserService.Api` project folder:

```powershell
dotnet run
```

Open Swagger at `https://localhost:{port}/swagger` to test endpoints. Use the `Authorize` button to paste `Bearer {accessToken}` for protected calls.

## Notes
- Replace the `Jwt:Key` with a secure secret before deploying.
- The current rate limiter is intentionally lightweight — consider using a distributed limiter for multi-instance deployments.
- Refresh tokens are stored in the database and revoked when exchanged.

