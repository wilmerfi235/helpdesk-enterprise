using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Events;

/// <summary>
/// 
/// la clase TicketCreatedEvent debe representar la creación inicial del ticket y contener los datos 
/// clave requeridos para notificar o auditar este evento.
/// 
/// </summary>
public sealed class TicketCreatedEvent : IDomainEvent
{
    public int TicketId { get; }
    public string TicketNumber { get; }
    public string Title { get; }
    public int CustomerId { get; }
    public TicketPriority Priority { get; }
    public DateTime OccurredOn { get; }

    public TicketCreatedEvent(
        int ticketId,
        string ticketNumber,
        string title,
        int customerId,
        TicketPriority priority)
    {
        TicketId = ticketId;
        TicketNumber = ticketNumber;
        Title = title;
        CustomerId = customerId;
        Priority = priority;
        OccurredOn = DateTime.UtcNow;
    }
}
