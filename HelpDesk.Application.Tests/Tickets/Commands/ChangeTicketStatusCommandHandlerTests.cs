using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.Tickets.Commands.ChangeTicketStatus;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HelpDesk.Application.Tests.Tickets.Commands;

/// <summary>
/// 
/// La clase de pruebas ChangeTicketStatusCommandHandlerTests.cs tiene la responsabilidad de verificar que el caso de uso 
/// ChangeTicketStatusCommandHandler ejecute correctamente las transiciones de estado de un ticket y persista 
/// los cambios a través del repositorio y la unidad de trabajo (IUnitOfWork), o lance excepciones en escenarios no válidos.
/// 
/// Se tiene la estructura e implementación recomendada utilizando xUnit y Moq
/// 
/// *****************************************************************************************************************************
/// *****************************************************************************************************************************
/// Flujos Correctos (Casos de Éxito):
///
/// Transición de Open a InProgress via StartProgress.

// Transición de InProgress a Resolved via Resolve.

/// Transición de Resolved a Closed via Close.

/// Garantiza que en cada cambio de estado exitoso se invoque UpdateAsync en el repositorio y SaveChangesAsync en la unidad de trabajo.

/// Manejo de Errores y Reglas de Negocio:

/// Ticket Inexistente: Lanza KeyNotFoundException y no ejecuta la persistencia.

/// Transición de Estado Inválida: Si el modelo de dominio rechaza la transición
/// (por ejemplo, intentar resolver un ticket que aún no está en progreso), la excepción se propaga 
/// y se asegura que nunca se guarden cambios en el repositorio(Times.Never).
/// 
/// </summary>
public class ChangeTicketStatusCommandHandlerTests
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ChangeTicketStatusCommandHandler _handler;

    public ChangeTicketStatusCommandHandlerTests()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new ChangeTicketStatusCommandHandler(
            _ticketRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    private Ticket CreateTestTicket()
    {
        return new Ticket(
            ticketNumber: "TCK-1001",
            title: "Falla en sistema",
            description: "No se puede iniciar sesión",
            type: TicketType.Incident,
            impact: TicketImpact.Medium,
            affectedUsers: 3,
            isCriticalService: false,
            customerId: 1,
            projectId: 1,
            departmentId: 1
        );
    }

    [Fact]
    public async Task Handle_StartProgressAction_ShouldUpdateTicketStatusToInProgressAndSave()
    {
        // Arrange
        var ticketId = 1;
        var ticket = CreateTestTicket(); // Nace en estado Open
        var command = new ChangeTicketStatusCommand(ticketId, TicketStatusAction.StartProgress);

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(TicketStatus.InProgress, ticket.Status);
        _ticketRepositoryMock.Verify(r => r.UpdateAsync(ticket, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ResolveAction_ShouldUpdateTicketStatusToResolvedAndSave()
    {
        // Arrange
        var ticketId = 1;
        var ticket = CreateTestTicket();
        ticket.StartProgress(); // Cambiar a InProgress primero

        var command = new ChangeTicketStatusCommand(ticketId, TicketStatusAction.Resolve);

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(TicketStatus.Resolved, ticket.Status);
        _ticketRepositoryMock.Verify(r => r.UpdateAsync(ticket, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CloseAction_ShouldUpdateTicketStatusToClosedAndSave()
    {
        // Arrange
        var ticketId = 1;
        var ticket = CreateTestTicket();
        ticket.StartProgress();
        ticket.Resolve(); // Debe estar Resuelto para cerrarse

        var command = new ChangeTicketStatusCommand(ticketId, TicketStatusAction.Close);

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(TicketStatus.Closed, ticket.Status);
        _ticketRepositoryMock.Verify(r => r.UpdateAsync(ticket, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var ticketId = 99;
        var command = new ChangeTicketStatusCommand(ticketId, TicketStatusAction.StartProgress);

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _ticketRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidStateTransition_ShouldThrowInvalidOperationExceptionAndNotSave()
    {
        // Arrange
        var ticketId = 1;
        var ticket = CreateTestTicket(); // El ticket recién creado está en estado 'Open'

        // Intentar Resolver directamente un ticket en estado 'Open' no está permitido por el Dominio
        var command = new ChangeTicketStatusCommand(ticketId, TicketStatusAction.Resolve);

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _ticketRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
