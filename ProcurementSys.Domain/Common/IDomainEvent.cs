using MediatR;

namespace ProcurementSys.Domain.Common;

// Domain event = in-process, sinkron, di-dispatch via MediatR INotification saat SaveChangesAsync.
// Beda dengan Integration Event (lihat ProcurementSys.Contracts) yang cross-service lewat RabbitMQ.
public interface IDomainEvent : INotification { }
