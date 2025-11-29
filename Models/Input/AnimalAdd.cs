using Zwierzyniec.Common.Enums;
using Zwierzyniec.Models.Base;

namespace Zwierzyniec.Models.Input
{
    public class AnimalAdd : AnimalBaseModel
    {
        public required DateTime IntakeDate { get; set; } = DateTime.UtcNow;
    }
}
