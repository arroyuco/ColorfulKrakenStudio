using ColorfulKrakenStudio.Client.Pages;
using ColorfulKrakenStudio.Components;
using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Services;
using ColorfulKrakenStudio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


//Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null)));

//Identity configuration
builder.Services.AddIdentity<Customer, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;

    options.SignIn.RequireConfirmedEmail = false;
})

.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

//Services inyection
builder.Services.AddScoped<TutorialService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

//Endpoint authentication
app.MapPost("/auth/login", async (
    HttpContext httpContext,
    SignInManager<Customer> signInManager) =>
{
    var form = httpContext.Request.Form;
    var email = form["email"].ToString();
    var password = form["password"].ToString();
    var remember = form["rememberMe"] == "true";

    var result = await signInManager.PasswordSignInAsync(email, password, remember, lockoutOnFailure: false);

    if (result.Succeeded) return Results.Redirect("/");
    if (result.IsLockedOut) return Results.Redirect("/login?error=2");
    if (result.IsNotAllowed) return Results.Redirect("/login?error=3");

    return Results.Redirect("/login?error=1");
});

//Endpoint logout
app.MapPost("/auth/logout", async (
    SignInManager<Customer> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ColorfulKrakenStudio.Client._Imports).Assembly);

app.Run();
