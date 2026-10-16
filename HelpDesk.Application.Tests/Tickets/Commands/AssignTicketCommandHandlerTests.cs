using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.Tickets.Commands.AssignTicket;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Exceptions;
using HelpDesk.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace HelpDesk.Application.Tests.Tickets.Commands;

public class AssignTicketCommandHandlerTests
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<ITicketAssignmentService> _assignmentServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AssignTicketCommandHandler _handler;

    public AssignTicketCommandHandlerTests()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _assignmentServiceMock = new Mock<ITicketAssignmentService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new AssignTicketCommandHandler(
            _ticketRepositoryMock.Object,
            _employeeRepositoryMock.Object,
            _assignmentServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenTicketAndEmployeeExist_ShouldAssignAndUpdateRepository()
    {
        // Arrange
        var ticketId = 1;
        var employeeId = Guid.NewGuid();
        var command = new AssignTicketCommand(ticketId, employeeId);

        var ticket = new Ticket(
            "TCK-001",
            "Título",
            "Descripción",
            TicketType.Incident,
            TicketImpact.Low,
            1,
            false,
            1,
            1,
            departmentId: 1
        );

        // Corrección de la instanciación de Employee:
        var employee = Employee.Create(
            userId: employeeId,             // Guid
            employeeNumber: "EMP-001",      // string
            departmentId: 1,                // int (coincide con el ticket)
            position: "Soporte Técnico"      // string
        );

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        _employeeRepositoryMock
            .Setup(r => r.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _assignmentServiceMock.Verify(s => s.Assign(ticket, employee), Times.Once);
        _ticketRepositoryMock.Verify(r => r.UpdateAsync(ticket, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new AssignTicketCommand(999, Guid.NewGuid());

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(command.TicketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDomainServiceFails_ShouldNotSaveChangesAsync()
    {
        // Arrange
        var ticketId = 1;
        var employeeId = Guid.NewGuid();
        var command = new AssignTicketCommand(ticketId, employeeId);

        var ticket = new Ticket(
            "TCK-001",
            "Título",
            "Descripción",
            TicketType.Incident,
            TicketImpact.Low,
            1,
            false,
            1,
            1,
            departmentId: 1
        );

        // Corrección: Pasar parámetros válidos para la firma de Employee.Create
        var employee = Employee.Create(
            userId: employeeId,
            employeeNumber: "EMP-002",
            departmentId: 2, // Se asigna departamento 2 para provocar el fallo de la regla de negocio
            position: "Soporte Técnico"
        );

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        _employeeRepositoryMock
            .Setup(r => r.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        _assignmentServiceMock
            .Setup(s => s.Assign(ticket, employee))
            .Throws(new DomainException("El ticket no puede ser asignado a un empleado de otro departamento."));

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() =>
            _handler.Handle(command, CancellationToken.None));

        // Garantiza que al fallar la regla de negocio no persista nada en UnitOfWork
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
