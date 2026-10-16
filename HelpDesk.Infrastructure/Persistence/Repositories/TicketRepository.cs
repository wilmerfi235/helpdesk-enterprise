using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Domain.Entities;
using HelpDesk.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Infrastructure.Persistence.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;

    public TicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Project)
            .Include(t => t.Department)
            .Include(t => t.AssignedEmployee)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<string> GetNextTicketNumberAsync(CancellationToken cancellationToken = default)
    {
        var count = await _context.Tickets.CountAsync(cancellationToken) + 1;
        return $"TCK-{DateTime.UtcNow.Year}-{count:D4}";
    }

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        await _context.Tickets.AddAsync(ticket, cancellationToken);
    }

    public Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        _context.Tickets.Update(ticket);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Ticket>> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Project)
            .Include(t => t.Department)
            .Include(t => t.AssignedEmployee)
            .Where(t => t.DepartmentId == departmentId)
            .AsNoTracking() // Recomendado para consultas de lectura (Queries)
            .ToListAsync(cancellationToken);
    }
}
