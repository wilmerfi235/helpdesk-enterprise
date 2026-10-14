using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelpDesk.Domain.Tests.Entities;

public class TicketTests
{
    private Ticket CreateValidTicket()
    {
        return new Ticket(
            ticketNumber: "TCK-1001",
            title: "Falla en inicio de sesión",
            description: "El usuario no puede ingresar.",
            type: TicketType.Incident,
            impact: TicketImpact.Medium,
            affectedUsers: 5,
            isCriticalService: false,
            customerId: 1,
            projectId: 10,
            departmentId: 2
        );
    }

    [Fact]
    public void Constructor_ShouldInitializeWithOpenStatusAndLowPriority()
    {
        // Act
        var ticket = CreateValidTicket();

        // Assert
        Assert.Equal(TicketStatus.Open, ticket.Status);
        Assert.Equal(TicketPriority.Low, ticket.Priority);
        Assert.Equal("TCK-1001", ticket.TicketNumber);
        Assert.Null(ticket.AssignedEmployeeId);
    }

    [Fact]
    public void StartProgress_FromOpenStatus_ShouldChangeStatusToInProgress()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        ticket.StartProgress();

        // Assert
        Assert.Equal(TicketStatus.InProgress, ticket.Status);
        Assert.NotNull(ticket.UpdatedAt);
    }

    [Fact]
    public void Resolve_FromInProgressStatus_ShouldChangeStatusToResolved()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.StartProgress();

        // Act
        ticket.Resolve();

        // Assert
        Assert.Equal(TicketStatus.Resolved, ticket.Status);
    }

    [Fact]
    public void Close_FromResolvedStatus_ShouldChangeStatusToClosed()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.StartProgress();
        ticket.Resolve();

        // Act
        ticket.Close();

        // Assert
        Assert.Equal(TicketStatus.Closed, ticket.Status);
    }

    [Fact]
    public void Close_FromOpenStatus_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act & Assert (Solo se pueden cerrar tickets en estado Resolved)
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.Close());
        Assert.Equal("Only resolved tickets can be closed.", exception.Message);
    }

    [Fact]
    public void AssignTo_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ticket.AssignTo(Guid.Empty));
        Assert.Contains("El empleado es obligatorio.", exception.Message);
    }

    [Fact]
    public void AssignTo_WhenTicketIsClosed_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.StartProgress();
        ticket.Resolve();
        ticket.Close();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.AssignTo(Guid.NewGuid()));
        Assert.Equal("A closed ticket cannot be assigned.", exception.Message);
    }
}