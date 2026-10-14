using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.DTOs;

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketType Type { get; set; }
    public TicketImpact Impact { get; set; }
    public int AffectedUsers { get; set; }
    public bool IsCriticalService { get; set; }
    public int CustomerId { get; set; }
    public int ProjectId { get; set; }
    public int DepartmentId { get; set; }
}
