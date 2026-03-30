using ColorfulKrakenStudio.Models;
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
                PurchaseService purchaseService,
                ILogger<Program> logger) =>
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
                        {
                            logger.LogInformation($"Payment completed for tutorial {tutorialId} by customer {customerId}");
                            await purchaseService.RegisterPurchaseAsync(customerId, tutorialId, amount);
                            logger.LogInformation("Purchase registered successfully");
                        }
                    }
                    return Results.Ok();
                }
                catch (StripeException ex)
                {
                    logger.LogError(ex, "Stripe webhook signature validation failed");
                    return Results.BadRequest();
                }
            });
        }
    }
}
