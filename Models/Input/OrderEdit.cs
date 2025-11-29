using Zwierzyniec.Models.Base;

namespace Zwierzyniec.Models.Input
{
    public class OrderEdit : OrderBase
    {
        public required int Id { get; set; }
    }
}
