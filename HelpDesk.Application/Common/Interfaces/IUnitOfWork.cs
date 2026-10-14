using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Common.Interfaces;

/// <summary>
/// 
/// Contrato para coordinar la persistencia y la gestión de transacciones en la base de datos.
/// 
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
