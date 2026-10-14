using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    // Relaciones
    public ICollection<Project> Projects { get; set; } = new List<Project>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
