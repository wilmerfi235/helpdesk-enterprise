using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Entities;

public class Ticket
{
    public int Id { get; private set; }

    public string TicketNumber { get; private set; } = string.Empty;

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public TicketStatus Status { get; private set; }

    public TicketPriority Priority { get; private set; }

    public TicketType Type { get; private set; }

    public int CustomerId { get; private set; }

    public int ProjectId { get; private set; }

    public Guid? AssignedEmployeeId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    // Relaciones
    public Customer Customer { get; private set; } = null!;

    public Project Project { get; private set; } = null!;

    public Department Department { get; private set; } = null!;

    public Employee? AssignedEmployee { get; private set; }

    public TicketImpact Impact { get; private set; }

    public int AffectedUsers { get; private set; }

    public bool IsCriticalService { get; private set; }

    public int DepartmentId { get; private set; }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ticketNumber"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="type"></param>
    /// <param name="impact"></param>
    /// <param name="affectedUsers"></param>
    /// <param name="isCriticalService"></param>
    /// <param name="customerId"></param>
    /// <param name="projectId"></param>
    public Ticket(
      string ticketNumber,
      string title,
      string description,
      TicketType type,
      TicketImpact impact,
      int affectedUsers,
      bool isCriticalService,
      int customerId,
      int projectId,
      int departmentId)
    {
        TicketNumber = ticketNumber;
        Title = title;
        Description = description;
        Type = type;
        Impact = impact;
        AffectedUsers = affectedUsers;
        IsCriticalService = isCriticalService;
        CustomerId = customerId;
        ProjectId = projectId;
        DepartmentId = departmentId;

        Status = TicketStatus.Open;
        Priority = TicketPriority.Low;
        CreatedAt = DateTime.UtcNow;
    }


    // Comportamiento
    public void AssignTo(Guid employeeId)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException(
                "A closed ticket cannot be assigned.");

        if (employeeId == Guid.Empty)
            throw new ArgumentException(
                "El empleado es obligatorio.",
                nameof(employeeId));

        AssignedEmployeeId = employeeId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePriority(TicketPriority priority)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException(
                "A closed ticket cannot change priority.");

        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void StartProgress()
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException(
                "Only open tickets can start progress.");

        Status = TicketStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resolve()
    {
        if (Status != TicketStatus.InProgress)
            throw new InvalidOperationException(
                "Only tickets in progress can be resolved.");

        Status = TicketStatus.Resolved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status != TicketStatus.Resolved)
            throw new InvalidOperationException(
                "Only resolved tickets can be closed.");

        Status = TicketStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reopen()
    {
        if (Status != TicketStatus.Closed)
            throw new InvalidOperationException(
                "Only closed tickets can be reopened.");

        Status = TicketStatus.Open;
        UpdatedAt = DateTime.UtcNow;
    }
}
