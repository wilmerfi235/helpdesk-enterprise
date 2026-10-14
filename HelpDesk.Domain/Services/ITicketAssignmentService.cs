using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Services;

/// <summary>
/// 
/// IDomainService marca la TIPO o ROL de la interfaz (Marker Interface)
/// ITicketAssignmentService define el CONTRATO ESPECÍFICO de este servicio
/// 
/// </summary>
public interface ITicketAssignmentService : IDomainService
{
    bool CanAssign(Ticket ticket, Employee employee);
    void Assign(Ticket ticket, Employee employee);
}
