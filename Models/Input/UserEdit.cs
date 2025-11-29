using Zwierzyniec.Common.Enums;
using Zwierzyniec.Models.Base;

namespace Zwierzyniec.Models.Input
{
    public class UserEdit : UserBase
    {
        public required int Id { get; set; }

        public required UserRoleEnum UserRole { get; set; }
    }
}
