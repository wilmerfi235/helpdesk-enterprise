using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Entities;

public class TicketComment
{
    public int Id { get; private set; }

    public int TicketId { get; private set; }

    public Guid AuthorId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    // Relaciones
    public Ticket Ticket { get; private set; } = null!;

    public User Author { get; private set; } = null!;
}
