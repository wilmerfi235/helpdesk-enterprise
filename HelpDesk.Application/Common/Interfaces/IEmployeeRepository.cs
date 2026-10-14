using HelpDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Application.Common.Interfaces;

/// <summary>
/// 
/// Contrato para la consulta de empleados desde la capa de aplicación/servicios de dominio.
/// 
/// </summary>
public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
