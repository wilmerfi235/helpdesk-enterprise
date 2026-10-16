using HelpDesk.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Commands.ChangeTicketStatus;

public class ChangeTicketStatusCommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeTicketStatusCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeTicketStatusCommand command, CancellationToken cancellationToken = default)
    {
        var ticket = await _ticketRepository.GetByIdAsync(command.TicketId, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el ticket con ID: {command.TicketId}");

        switch (command.Action)
        {
            case TicketStatusAction.StartProgress:
                ticket.StartProgress();
                break;
            case TicketStatusAction.Resolve:
                ticket.Resolve();
                break;
            case TicketStatusAction.Close:
                ticket.Close();
                break;
            case TicketStatusAction.Reopen:
                ticket.Reopen();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command.Action), "Acción de estado no válida");
        }

        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
