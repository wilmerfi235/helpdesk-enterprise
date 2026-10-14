using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Entities;

public class TicketHistory
{
    public int Id { get; private set; }

    public int TicketId { get; private set; }

    public Guid UserId { get; private set; }

    public string Action { get; private set; } = string.Empty;

    public string? OldValue { get; private set; }

    public string? NewValue { get; private set; }

    public DateTime CreatedAt { get; private set; }

    // Relaciones
    public Ticket Ticket { get; private set; } = null!;

    public User User { get; private set; } = null!;
}
