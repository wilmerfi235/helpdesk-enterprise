using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Events;
using HelpDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Aggregates;

public class TicketAggregate
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public int Id { get; private set; }
    public string TicketNumber { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TicketStatus Status { get; private set; }
    public TicketPriority Priority { get; private set; }
    public Guid? AssignedEmployeeId { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private TicketAggregate() { }

    public static TicketAggregate Create(string ticketNumber, string title, string description, TicketPriority priority, int customerId) // Se agrega customerId si aplica a la creación
    {
        var aggregate = new TicketAggregate
        {
            TicketNumber = ticketNumber,
            Title = title,
            Description = description,
            Priority = priority,
            Status = TicketStatus.Open
        };

        //Se pasan los 5 argumentos en el orden exacto esperado por el constructor
        aggregate._domainEvents.Add(new TicketCreatedEvent(
            aggregate.Id,          // int
            aggregate.TicketNumber, // string
            aggregate.Title,        // string
            customerId,             // int
            aggregate.Priority     // TicketPriority
        ));

        return aggregate;
    }

    public void StartProgress()
    {
        if (Status != TicketStatus.Open)
            throw new InvalidTicketStateException("Solo se puede iniciar un ticket en estado abierto.");

        Status = TicketStatus.InProgress;
        _domainEvents.Add(new TicketStatusChangedEvent(Id, Status));
    }

    public void Resolve()
    {
        if (Status != TicketStatus.InProgress)
            throw new InvalidTicketStateException("Solo se puede resolver un ticket en progreso.");

        Status = TicketStatus.Resolved;
        _domainEvents.Add(new TicketStatusChangedEvent(Id, Status));
    }

    public void Close(string? reason = null)
    {
        if (Status != TicketStatus.Resolved && Status != TicketStatus.InProgress) // Según las reglas de negocio
            throw new InvalidOperationException("Solo se pueden cerrar tickets resueltos o en progreso.");

        Status = TicketStatus.Closed;
        UpdatedAt = DateTime.UtcNow;

        // Registra el evento de cierre de ticket
        _domainEvents.Add(new TicketClosedEvent(
            Id,
            TicketNumber,
            reason
        ));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
