using Zwierzyniec.Common.Enums;

namespace Zwierzyniec.Models.Base
{
    public class AnimalBaseModel
    {
        public required string Name { get; set; }

        public required int Age { get; set; }

        public required float Price { get; set; }

        public required StatusEnum Stauts { get; set; }

        public required SpeciesEnum Species { get; set; }

        public required GenderEnum Gender { get; set; }

        public required string Description { get; set; }

        public byte[]? Photo { get; set; }
    }
}
