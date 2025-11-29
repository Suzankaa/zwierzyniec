using Zwierzyniec.Common.Enums;

namespace Zwierzyniec.Models.Base
{
    public class OrderBase
    {
        public required int UserId { get; set; }

        public required DateTime OrderDate { get; set; }

        public required string Status { get; set; }

        public required PayMethodEnum Paymethod { get; set; }

        public required ShippingMethodEnum ShippingMethodEnum { get; set; }

        public required List<int> ProductIds { get; set; }

    }
}
