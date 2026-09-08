using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementSys.Domain.Enums
{
    public enum ProcurementRequestStatus
    {
        PendingVendorSelection = 1,
        InApproval = 2,
        Approved = 3,
        Rejected = 4
    }
}
