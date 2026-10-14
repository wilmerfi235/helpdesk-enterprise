using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.DTOs;

public class AssignTicketDto
{
    public Guid TicketId { get; set; }
    public Guid EmployeeId { get; set; }
}
