using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Tickets.Commands.AssignTicket;

/// <summary>
/// 
/// Recibe TicketId y EmployeeId, rescata ambas entidades de los repositorios, ejecuta TicketAssignmentService.Assign(...) y 
/// persiste el cambio.
/// 
/// </summary>
/// <param name="TicketId"></param>
/// <param name="EmployeeId"></param>
public record AssignTicketCommand(int TicketId, Guid EmployeeId);
