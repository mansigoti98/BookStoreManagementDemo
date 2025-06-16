using System.Security.Claims;
using BookStoreManagement_1.Data;
using BookStoreManagement_1.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly BookStoreDbContext _context;

    public AccountController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UnifiedLoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (model.IsAdmin)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (admin != null && PasswordHelper.VerifyPassword(model.Password, admin.PasswordHash, admin.PasswordSalt))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Email, admin.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");
                await HttpContext.SignInAsync(
                    scheme: "AdminScheme",
                    principal: new ClaimsPrincipal(claimsIdentity),
                    properties: new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });

                return RedirectToAction("Dashboard", "Admin");
            }

            ModelState.AddModelError("", "Invalid admin credentials.");
        }
        else
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (customer != null && PasswordHelper.VerifyPassword(model.Password, customer.PasswordHash, customer.PasswordSalt))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                    new Claim(ClaimTypes.Role, "Customer"),
                    new Claim(ClaimTypes.Email, customer.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CustomerScheme");
                await HttpContext.SignInAsync(
                    scheme: "CustomerScheme",
                    principal: new ClaimsPrincipal(claimsIdentity),
                    properties: new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });

                return RedirectToAction("Shop", "Customer");
            }

            ModelState.AddModelError("", "Invalid customer credentials.");
        }

        return View(model);
    }
}
