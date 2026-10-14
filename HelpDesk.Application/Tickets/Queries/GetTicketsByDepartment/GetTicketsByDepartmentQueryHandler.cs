using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Queries.GetTicketsByDepartment;

public class GetTicketsByDepartmentQueryHandler
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketsByDepartmentQueryHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetTicketsByDepartmentQuery query, CancellationToken cancellationToken = default)
    {
        var tickets = await _ticketRepository.GetByDepartmentIdAsync(query.DepartmentId, cancellationToken);

        return tickets.Select(ticket => new TicketDto
        {
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
        });
    }
}
