using ColorfulKrakenStudio.Services;
using Stripe;

namespace ColorfulKrakenStudio.Endpoints
{
    public static class StripeEndpoints
    {
        public static void MapStripeEndpoints(this WebApplication app)
        {
            app.MapPost("/webhook/stripe", async (
                HttpContext httpContext,
                PurchaseService purchaseService) =>
            {
                var json = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                var webhookSecret = app.Configuration["Stripe:WebhookSecret"];

                try
                {
                    // Stripe signature confirmation
                    var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        httpContext.Request.Headers["Stripe-Signature"],
                        webhookSecret);

                    if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                    {
                        var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        var customerId = session?.Metadata["customerId"];
                        var tutorialId = int.Parse(session?.Metadata["tutorialId"] ?? "0");
                        var amount = (session?.AmountTotal ?? 0) / 100m;

                        if (customerId != null && tutorialId > 0)
                            await purchaseService.RegisterPurchaseAsync(customerId, tutorialId, amount);
                    }

                    return Results.Ok();
                }
                catch (StripeException)
                {
                    // Si la firma no es válida devolvemos 400 — Stripe reintentará el webhook
                    return Results.BadRequest();
                }
            });
        }
    }
}
