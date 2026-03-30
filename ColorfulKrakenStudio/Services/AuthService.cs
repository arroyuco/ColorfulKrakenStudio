using ColorfulKrakenStudio.Models;
using Microsoft.AspNetCore.Identity;
using Serilog.Core;

namespace ColorfulKrakenStudio.Services;

public class AuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<Customer> _userManager;
    private readonly SignInManager<Customer> _signInManager;

    public AuthService(UserManager<Customer> userManager, SignInManager<Customer> signInManager, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IdentityResult> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        _logger.LogInformation("New registration attempt for {Email}", email);

        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            CreatedAt = DateTime.UtcNow
        };

        _logger.LogInformation("Customer registered successfully: {Email}", email);

        return await _userManager.CreateAsync(customer, password);
    }

    public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe)
        => await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

    public async Task LogoutAsync()
        => await _signInManager.SignOutAsync();
}