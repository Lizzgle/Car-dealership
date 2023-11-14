using CarShop.Domain.Models;
using Microsoft.AspNetCore.Http;
using CarShop.Extensions;
using System.Text.Json.Serialization;
using CarShop.Domain.Entities;

namespace CarShop.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services
                                    .GetRequiredService<IHttpContextAccessor>()
                                    .HttpContext?.Session;

            SessionCart cart = session?.Get<SessionCart>("Cart")
                                                    ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddToCart(Car product)
        {
            base.AddToCart(product);
            Session?.Set("Cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
