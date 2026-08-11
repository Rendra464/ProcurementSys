using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementSys.Contracts.IntegrationEvents
{
    public record MaterialRequestStatusChangedIntegrationEvent(
    int MaterialRequestId,
    string RequestNumber,
    string NewStatus,
    string RecipientEmail
    ) : IntegrationEvent;
}
