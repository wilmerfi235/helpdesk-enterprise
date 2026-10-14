using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Services;

public class TicketPriorityService : ITicketPriorityService
{
    public TicketPriority CalculatePriority(Ticket ticket)
    {
        // Reglas de negocio

        if (ticket.Impact == TicketImpact.High &&
            ticket.AffectedUsers >= 100 &&
            ticket.IsCriticalService)
        {
            return TicketPriority.Critical;
        }

        if (ticket.Impact == TicketImpact.High &&
            ticket.AffectedUsers >= 50)
        {
            return TicketPriority.High;
        }

        if (ticket.AffectedUsers >= 10)
        {
            return TicketPriority.Medium;
        }

        return TicketPriority.Low;
    }
}
