using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Entities;

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    // Relaciones
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
