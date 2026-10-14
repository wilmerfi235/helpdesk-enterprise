using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.DTOs;
using HelpDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Commands.CreateTicket;

public class CreateTicketCommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand command, CancellationToken cancellationToken = default)
    {
        // 1. Obtener la cadena correlativa directamente (string)
        string ticketNumber = await _ticketRepository.GetNextTicketNumberAsync(cancellationToken);

        // 2. Instanciar la entidad Ticket usando su constructor
        var ticket = new Ticket(
            ticketNumber: ticketNumber,
            title: command.Title,
            description: command.Description,
            type: command.Type,
            impact: command.Impact,
            affectedUsers: command.AffectedUsers,
            isCriticalService: command.IsCriticalService,
            customerId: command.CustomerId,
            projectId: command.ProjectId,
            departmentId: command.DepartmentId
        );

        // 3. Persistir los datos
        await _ticketRepository.AddAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 4. Mapear a DTO de respuesta
        return new TicketDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            Type = ticket.Type,
            Impact = ticket.Impact,
            AffectedUsers = ticket.AffectedUsers,
            IsCriticalService = ticket.IsCriticalService,
            CustomerId = ticket.CustomerId,
            ProjectId = ticket.ProjectId,
            DepartmentId = ticket.DepartmentId,
            AssignedEmployeeId = ticket.AssignedEmployeeId,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
    }
}
