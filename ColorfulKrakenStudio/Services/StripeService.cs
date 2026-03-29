using Stripe;
using Stripe.Checkout;
using ColorfulKrakenStudio.Models;

namespace ColorfulKrakenStudio.Services;

public class StripeService
{
    private readonly IConfiguration _config;

    public StripeService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string> CreateCheckoutSessionAsync(Tutorial tutorial, string customerId, string customerEmail)
    {
        var options = new SessionCreateOptions
        {
            //Unique pay, no suscription
            Mode = "payment",

            CustomerEmail = customerEmail,

            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        UnitAmount = (long)(tutorial.Price * 100), //cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name        = tutorial.Title,
                            Description = $"By {tutorial.Author} · {tutorial.Duration} min",
                        }
                    },
                    Quantity = 1
                }
            },

            // URL to redirect
            SuccessUrl = $"{_config["App:BaseUrl"]}/purchase/success?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{_config["App:BaseUrl"]}/tutorials",

            Metadata = new Dictionary<string, string>
            {
                { "tutorialId", tutorial.Id.ToString() },
                { "customerId", customerId }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }
}