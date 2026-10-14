using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Services;

public interface ITicketPriorityService
{
    TicketPriority CalculatePriority(Ticket ticket);
}
