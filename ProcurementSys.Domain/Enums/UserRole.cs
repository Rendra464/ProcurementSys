using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementSys.Domain.Enums
{
    public enum UserRole
    {
        Requester = 1,
        ApproverInternal = 2,
        ApproverVendor = 3,
        Admin = 4,
        ProcurementOfficer = 5
    }
}
