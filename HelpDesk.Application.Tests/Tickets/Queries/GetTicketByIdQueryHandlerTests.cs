using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.DTOs;
using HelpDesk.Application.Tickets.Queries.GetTicketById;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tests.Tickets.Queries;

/// <summary>
/// 
/// La clase de pruebas GetTicketByIdQueryHandlerTests.cs se encarga de comprobar que la consulta retorne el DTO mapeado 
/// cuando el ticket existe en el repositorio o null si no es encontrado.
/// 
/// **************************************************************************************************
/// **************************************************************************************************
/// 
/// Escenarios cubiertos en la prueba
/// 1.- Ticket Existente: Se simula la recuperación de una entidad Ticket desde la base de datos y se asegura 
/// que GetTicketByIdQueryHandler retorne la información proyectada correctamente en una instancia de TicketDto.
/// 
/// 2.- Ticket Inexistente: Se simula la devolución de null por parte del repositorio y se confirma que la consulta 
/// retorne null limpiamente sin lanzar excepciones no deseadas.
/// 
/// </summary>
public class GetTicketByIdQueryHandlerTests
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly GetTicketByIdQueryHandler _handler;

    public GetTicketByIdQueryHandlerTests()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _handler = new GetTicketByIdQueryHandler(_ticketRepositoryMock.Object);
    }

    private Ticket CreateTestTicket()
    {
        return new Ticket(
            ticketNumber: "TCK-2001",
            title: "Problema con la red VPN",
            description: "No se logra conectar a los servidores internos",
            type: TicketType.Incident,
            impact: TicketImpact.Medium,
            affectedUsers: 3,
            isCriticalService: false,
            customerId: 10,
            projectId: 5,
            departmentId: 2
        );
    }

    [Fact]
    public async Task Handle_WhenTicketExists_ShouldReturnMappedTicketDto()
    {
        // Arrange
        var ticketId = 1;
        var ticket = CreateTestTicket();
        var query = new GetTicketByIdQuery(ticketId);

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TicketDto>(result);
        Assert.Equal(ticket.TicketNumber, result.TicketNumber);
        Assert.Equal(ticket.Title, result.Title);
        Assert.Equal(ticket.Description, result.Description);
        Assert.Equal(ticket.Status, result.Status);
        Assert.Equal(ticket.Priority, result.Priority);
        Assert.Equal(ticket.Type, result.Type);
        Assert.Equal(ticket.Impact, result.Impact);
        Assert.Equal(ticket.CustomerId, result.CustomerId);
        Assert.Equal(ticket.ProjectId, result.ProjectId);
        Assert.Equal(ticket.DepartmentId, result.DepartmentId);

        _ticketRepositoryMock.Verify(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var ticketId = 99;
        var query = new GetTicketByIdQuery(ticketId);

        _ticketRepositoryMock
            .Setup(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _ticketRepositoryMock.Verify(r => r.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
