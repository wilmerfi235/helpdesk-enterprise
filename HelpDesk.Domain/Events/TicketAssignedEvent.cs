using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Events;

/// <summary>
/// 
/// Debe representar la asignación de un ticket a un empleado dentro del Dominio y contener la información clave para auditoría, 
/// notificaciones (por ejemplo, enviar un correo al empleado asignado) o trazabilidad.
/// 
/// </summary>
public sealed class TicketAssignedEvent : IDomainEvent
{
    public int TicketId { get; }
    public string TicketNumber { get; }
    public Guid AssignedEmployeeId { get; }
    public DateTime OccurredOn { get; }

    public TicketAssignedEvent(int ticketId, string ticketNumber, Guid assignedEmployeeId)
    {
        TicketId = ticketId;
        TicketNumber = ticketNumber;
        AssignedEmployeeId = assignedEmployeeId;
        OccurredOn = DateTime.UtcNow;
    }
}
