using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketType Type { get; set; }
    public TicketImpact Impact { get; set; }
    public int AffectedUsers { get; set; }
    public bool IsCriticalService { get; set; }
    public int CustomerId { get; set; }
    public int ProjectId { get; set; }
    public int DepartmentId { get; set; }
    public Guid? AssignedEmployeeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
