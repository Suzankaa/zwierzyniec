using Zwierzyniec.Models.Base;

namespace Zwierzyniec.Models.Output
{
    public class ProductResponse : ProductBase
    {
        public required int Id { get; set; }

        public required int Volume { get; set; }

        public required int SoldVolume { get; set; }
    }
}
