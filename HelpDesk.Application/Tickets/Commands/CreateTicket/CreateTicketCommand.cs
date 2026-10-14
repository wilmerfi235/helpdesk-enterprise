using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Commands.CreateTicket;

/// <summary>
/// 
/// Recibe datos del formulario/API, instancia el Ticket agregando sus reglas de negocio y lo guarda en el repositorio.
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Type"></param>
/// <param name="Impact"></param>
/// <param name="AffectedUsers"></param>
/// <param name="IsCriticalService"></param>
/// <param name="CustomerId"></param>
/// <param name="ProjectId"></param>
/// <param name="DepartmentId"></param>
public record CreateTicketCommand(
    string Title,
    string Description,
    TicketType Type,
    TicketImpact Impact,
    int AffectedUsers,
    bool IsCriticalService,
    int CustomerId,
    int ProjectId,
    int DepartmentId
);
