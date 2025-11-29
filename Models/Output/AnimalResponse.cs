using Zwierzyniec.Models.Base;

namespace Zwierzyniec.Models.Output
{
    public class AnimalResponse : AnimalBaseModel
    {
        public required int Id { get; set; }
        public required DateTime IntakeDate { get; set; }
    }
}
