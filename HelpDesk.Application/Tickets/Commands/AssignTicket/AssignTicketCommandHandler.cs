using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Commands.AssignTicket;

public class AssignTicketCommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITicketAssignmentService _assignmentService;
    private readonly IUnitOfWork _unitOfWork;

    public AssignTicketCommandHandler(
        ITicketRepository ticketRepository,
        IEmployeeRepository employeeRepository,
        ITicketAssignmentService assignmentService,
        IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _employeeRepository = employeeRepository;
        _assignmentService = assignmentService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task Handle(AssignTicketCommand command, CancellationToken cancellationToken = default)
    {
        var ticket = await _ticketRepository.GetByIdAsync(command.TicketId, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el ticket con ID: {command.TicketId}");

        var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el empleado con ID: {command.EmployeeId}");

        _assignmentService.Assign(ticket, employee);

        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
