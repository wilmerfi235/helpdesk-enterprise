using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Queries.GetTicketById;

public class GetTicketByIdQueryHandler
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketByIdQueryHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<TicketDto?> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken = default)
    {
        var ticket = await _ticketRepository.GetByIdAsync(query.Id, cancellationToken);

        if (ticket == null) return null;

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
