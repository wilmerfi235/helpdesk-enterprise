using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Commands.ChangeTicketStatus;

public enum TicketStatusAction
{
    StartProgress,
    Resolve,
    Close,
    Reopen
}

/// <summary>
/// 
/// Invoca las transiciones de estado de la entidad (StartProgress, Resolve, Close y Reopen).
/// 
/// </summary>
/// <param name="TicketId"></param>
/// <param name="Action"></param>

public record ChangeTicketStatusCommand(int TicketId, ChangeTicketStatusCommand Action);
