using Microsoft.AspNetCore.Localization;

namespace ColorfulKrakenStudio.Endpoints;

public static class LocalizationEndpoints
{
    public static void MapLocalizationEndpoints(this WebApplication app)
    {
        app.MapPost("/culture/set", async (HttpContext httpContext) =>
        {
            var form = httpContext.Request.Form;
            var culture = form["culture"].ToString();
            var redirectUri = form["redirectUri"].ToString();

            if (string.IsNullOrEmpty(culture)) return Results.BadRequest();

            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return Results.Redirect(string.IsNullOrEmpty(redirectUri) ? "/" : redirectUri);
        });
    }
}