using Zwierzyniec.Common.Enums;

namespace Zwierzyniec.Models.Base
{
    public class ProductBase
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public required float Price { get; set; }

        public int? Discount { get; set; }
    }
}
