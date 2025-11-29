using Zwierzyniec.Models.Base;

namespace Zwierzyniec.Models.Output
{
    public class OrderResponse : OrderBase
    {
        public required int Id { get; set; }
    }
}
