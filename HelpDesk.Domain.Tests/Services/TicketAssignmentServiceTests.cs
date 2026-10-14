using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Exceptions;
using HelpDesk.Domain.Services;
using HelpDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Tests.Services;

/// <summary>
/// 
/// (Servicio de Dominio)
/// Prueba la lógica de negocio al asignar un ticket a un empleado disponible utilizando el método Assign.
/// 
/// </summary>
public class TicketAssignmentServiceTests
{
    private readonly TicketAssignmentService _assignmentService;

    public TicketAssignmentServiceTests()
    {
        _assignmentService = new TicketAssignmentService();
    }

    private Ticket CreateValidTicket()
    {
        return new Ticket(
            ticketNumber: "TCK-3003",
            title: "Revisar VPN",
            description: "El empleado no logra conectarse.",
            type: TicketType.Incident,
            impact: TicketImpact.Low,
            affectedUsers: 1,
            isCriticalService: false,
            customerId: 1,
            projectId: 5,
            departmentId: 3
        );
    }

    [Fact]
    public void AssignTicket_ToAvailableEmployee_ShouldAssignSuccessfully()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // pasar Guid, EmployeeNumber, DepartmentId y Position
        var employee = Employee.Create(
            userId: Guid.NewGuid(),
            employeeNumber: "EMP-001",
            departmentId: ticket.DepartmentId, // Mismo departamento que el ticket
            position: "Soporte Técnico"
        );

        // Act
        _assignmentService.Assign(ticket, employee);

        // Assert
        Assert.Equal(employee.Id, ticket.AssignedEmployeeId);
    }

    [Fact]
    public void AssignTicket_ToUnavailableEmployee_ShouldThrowDomainException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // 1. Crear el empleado con la firma correcta
        var employee = Employee.Create(
            userId: Guid.NewGuid(),
            employeeNumber: "EMP-002",
            departmentId: ticket.DepartmentId,
            position: "Soporte Técnico"
        );

        // 2. Desactivar el empleado para simular que no está disponible / activo
        employee.Deactivate();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _assignmentService.Assign(ticket, employee));

        Assert.Equal("El empleado no cumple con las reglas para ser asignado a este ticket.", exception.Message);
    }
}
