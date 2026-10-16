using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.DTOs;
using HelpDesk.Application.Tickets.Commands.CreateTicket;
using HelpDesk.Domain.Enums;
using Microsoft.VisualBasic;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using HelpDesk.Domain.Entities;

namespace HelpDesk.Application.Tests.Tickets.Commands;

/// <summary>
/// 
/// La clase CreateTicketCommandHandlerTests.cs se encarga de verificar la correcta orquestación al crear un ticket:

/// Obtención del siguiente correlativo(ITicketRepository.GetNextTicketNumberAsync).

/// Instanciación y adición del nuevo Ticket al repositorio(ITicketRepository.AddAsync).

/// Persistencia mediante la unidad de trabajo(IUnitOfWork.SaveChangesAsync).

/// Retorno del DTO mapeado con la información correspondiente.
/// 
/// ****************************************************************************************************************
/// ****************************************************************************************************************
/// 
/// Aspectos clave validados en este test:
/// Obtención del Correlativo: Se simula la devolución de la cadena de texto "TCK-1001" por parte de GetNextTicketNumberAsync.

/// Creación y Mapeo: Se prueba que la entidad Ticket sea instanciada e integrada en AddAsync, y que 
/// los datos coincidan en el objeto retornado TicketDto.

/// Manejo de Transacciones: Se garantiza que SaveChangesAsync solo se ejecute tras agregar exitosamente el ticket al repositorio.
/// 
/// </summary>
public class CreateTicketCommandHandlerTests
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateTicketCommandHandler _handler;

    public CreateTicketCommandHandlerTests()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateTicketCommandHandler(
            _ticketRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTicketAndReturnDto()
    {
        // Arrange
        var command = new CreateTicketCommand(
            Title: "Error en el módulo de facturación",
            Description: "No se calculan correctamente los impuestos",
            Type: TicketType.Incident,
            Impact: TicketImpact.High,
            AffectedUsers: 10,
            IsCriticalService: true,
            CustomerId: 5,
            ProjectId: 2,
            DepartmentId: 1
        );

        var expectedTicketNumber = "TCK-1001";

        _ticketRepositoryMock
            .Setup(r => r.GetNextTicketNumberAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTicketNumber);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // 1. Validar el DTO de respuesta
        Assert.NotNull(result);
        Assert.IsType<TicketDto>(result);
        Assert.Equal(expectedTicketNumber, result.TicketNumber);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(TicketStatus.Open, result.Status);
        Assert.Equal(TicketPriority.Low, result.Priority); // Estado/Prioridad por defecto en el constructor

        // 2. Verificar que se agrego la entidad Ticket al repositorio con los datos del comando
        _ticketRepositoryMock.Verify(r => r.AddAsync(
            It.Is<Ticket>(t =>
                t.TicketNumber == expectedTicketNumber &&
                t.Title == command.Title &&
                t.CustomerId == command.CustomerId &&
                t.DepartmentId == command.DepartmentId),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        // 3. Verificar que se persiguieron los cambios en la BD
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryFails_ShouldNotSaveChangesAsync()
    {
        // Arrange
        var command = new CreateTicketCommand(
            Title: "Error de servidor",
            Description: "Servidor caído",
            Type: TicketType.Incident,
            Impact: TicketImpact.High,
            AffectedUsers: 50,
            IsCriticalService: true,
            CustomerId: 1,
            ProjectId: 1,
            DepartmentId: 1
        );

        _ticketRepositoryMock
            .Setup(r => r.GetNextTicketNumberAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error de conexión con el repositorio"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        // Garantizar que si falla la obtención del correlativo, no se agregue ni se guarde nada
        _ticketRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
