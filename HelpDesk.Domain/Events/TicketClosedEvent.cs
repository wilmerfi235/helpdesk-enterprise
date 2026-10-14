using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Events;

public sealed class TicketClosedEvent : IDomainEvent
{
    public int TicketId { get; }
    public string TicketNumber { get; }
    public string? Reason { get; }
    public DateTime OccurredOn { get; }

    public TicketClosedEvent(int ticketId, string ticketNumber, string? reason = null)
    {
        TicketId = ticketId;
        TicketNumber = ticketNumber;
        Reason = reason;
        OccurredOn = DateTime.UtcNow;
    }
}
