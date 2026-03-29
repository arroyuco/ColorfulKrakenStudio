using Microsoft.AspNetCore.Identity;
using ColorfulKrakenStudio.Models;

namespace ColorfulKrakenStudio.Services;

public class AuthService
{
    private readonly UserManager<Customer> _userManager;
    private readonly SignInManager<Customer> _signInManager;

    public AuthService(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            CreatedAt = DateTime.UtcNow
        };

        return await _userManager.CreateAsync(customer, password);
    }

    public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe)
        => await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

    public async Task LogoutAsync()
        => await _signInManager.SignOutAsync();
}