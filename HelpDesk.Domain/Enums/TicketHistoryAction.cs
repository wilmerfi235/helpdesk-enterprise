using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Enums;

public enum TicketHistoryAction
{
    Created = 1,
    Assigned = 2,
    PriorityChanged = 3,
    StatusChanged = 4,
    CommentAdded = 5,
    Resolved = 6,
    Closed = 7,
    Reopened = 8
}
