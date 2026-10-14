using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Events;

/// <summary>
/// 
/// OccurredOn (DateTime): Propiedad obligatoria para saber cuándo ocurrió la acción.
/// 
/// </summary>
public sealed class TicketStatusChangedEvent : IDomainEvent
{
    public int TicketId { get; }
    public TicketStatus NewStatus { get; }
    public DateTime OccurredOn { get; }

    public TicketStatusChangedEvent(int ticketId, TicketStatus newStatus)
    {
        TicketId = ticketId;
        NewStatus = newStatus;
        OccurredOn = DateTime.UtcNow; // Se asigna automáticamente la fecha de ocurrencia
    }
}
