using BookStoreManagement_1.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStoreManagement_1.Models
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, BookStoreDbContext dbContext)
        {
            var requestAt = DateTime.UtcNow;

            await _next(context); 

            var responseAt = DateTime.UtcNow;

            string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? role = context.User.FindFirst(ClaimTypes.Role)?.Value;
            string? email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            string? fullName = null;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(role))
            {
                if (role == "Customer")
                {
                    var customer = await dbContext.Customers
                        .Where(c => c.CustomerId == int.Parse(userId))
                        .Select(c => c.FullName)
                        .FirstOrDefaultAsync();

                    fullName = customer;
                }
                else if (role == "Admin")
                {
                    var admin = await dbContext.Admins
                        .Where(a => a.AdminId == int.Parse(userId))
                        .Select(a => a.FullName)
                        .FirstOrDefaultAsync();

                    fullName = admin;
                }
            }

            if (!context.Request.Path.Value.Contains("."))
            {
                var log = new UserLog
                {
                    Username = email ?? "Anonymous",
                    UserFullName = fullName ?? "Anonymous",
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    IPAddress = context.Connection.RemoteIpAddress?.ToString(),
                    RequestAt = requestAt,
                    ResponseAt = responseAt
                };

                dbContext.UserLogs.Add(log);
                await dbContext.SaveChangesAsync();
            }
        }
    }

}