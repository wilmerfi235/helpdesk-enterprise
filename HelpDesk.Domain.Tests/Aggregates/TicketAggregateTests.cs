using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDesk.Domain.Aggregates;
using HelpDesk.Domain.Events;
using Xunit;

namespace HelpDesk.Domain.Tests.Aggregates;

public class TicketAggregateTests
{
    private TicketAggregate CreateValidTicketAggregate()
    {
        return TicketAggregate.Create(
            ticketNumber: "TCK-2002",
            title: "Error en base de datos",
            description: "Timeout en la conexión principal.",
            priority: TicketPriority.High,
            customerId: 1 // <-- Agregado el parámetro faltante
        );
    }

    [Fact]
    public void StartProgress_ShouldPublishTicketStatusChangedEvent()
    {
        // Arrange
        var ticketAggregate = CreateValidTicketAggregate();
        ticketAggregate.ClearDomainEvents(); // Limpia eventos iniciales

        // Act
        ticketAggregate.StartProgress();

        // Assert
        Assert.Equal(TicketStatus.InProgress, ticketAggregate.Status);
        Assert.Contains(ticketAggregate.DomainEvents, e => e is TicketStatusChangedEvent);

        var domainEvent = ticketAggregate.DomainEvents.OfType<TicketStatusChangedEvent>().Single();
        Assert.Equal(TicketStatus.InProgress, domainEvent.NewStatus);
    }

    [Fact]
    public void Close_ShouldPublishTicketClosedEvent()
    {
        // Arrange
        var ticketAggregate = CreateValidTicketAggregate();
        ticketAggregate.StartProgress();
        ticketAggregate.Resolve();
        ticketAggregate.ClearDomainEvents();

        // Act
        ticketAggregate.Close();

        // Assert
        Assert.Equal(TicketStatus.Closed, ticketAggregate.Status);
        Assert.Contains(ticketAggregate.DomainEvents, e => e is TicketClosedEvent);
    }
}
