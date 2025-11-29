using Zwierzyniec.Common.Enums;
using Zwierzyniec.Models.Base;

namespace Zwierzyniec.Models.Output
{
    public class UserResponse : UserBase
    {
        public required int Id { get; set; }
        public required UserRoleEnum UserRole { get; set; }
    }
}
