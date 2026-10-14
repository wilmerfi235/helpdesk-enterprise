using HelpDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Common.Interfaces;

/// <summary>
/// 
/// Contrato con los métodos para obtener el siguiente correlativo, consultar por identificador y 
/// persistir/actualizar un ticket.
/// 
/// </summary>
public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<string> GetNextTicketNumberAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default);
}