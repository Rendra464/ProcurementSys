using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Abstractions.Services;

public interface ICurrentUserService
{
    int? UserId { get; }
    UserRole? Role { get; }
}