using ColorfulKrakenStudio.Models;
using Microsoft.AspNetCore.Identity;

namespace ColorfulKrakenStudio.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
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

        }
    }
}
