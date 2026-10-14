using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Services;

public class TicketAssignmentService : ITicketAssignmentService
{
    /// <summary>
    /// 
    /// Objetivo: Ejecutar la asignación efectiva del ticket al empleado.
    /// Comportamiento (Con Efectos Secundarios): Sí modifica el estado del dominio:
    ///
    /// Utiliza internamente CanAssign para validar las reglas.
    /// Si CanAssign devuelve false, lanza una excepción de dominio(DomainException).
    /// Si las reglas se cumplen, asigna el AssignedEmployeeId, cambia el estado del ticket(ej.a InProgress), actualiza la fecha 
    /// UpdatedAt y emite el evento de dominio TicketAssignedEvent.
    /// 
    /// 
    /// </summary>
    /// <param name="ticket"></param>
    /// <param name="employee"></param>
    /// <exception cref="DomainException"></exception>
    public void Assign(Ticket ticket, Employee employee)
    {
        if (!CanAssign(ticket, employee))
            throw new DomainException("El empleado no cumple con las reglas para ser asignado a este ticket.");

        // Ejecuta el comportamiento propio de la entidad Ticket
        ticket.AssignTo(employee.Id);
    }

    /// <summary>
    /// 
    /// Objetivo: Preguntar si se cumple la regla de negocio para asignar el ticket.
    /// Comportamiento (Idempotente / Sin Efectos Secundarios): No modifica nada en el estado del ticket ni del empleado.
    /// 
    /// </summary>
    /// <param name="ticket"></param>
    /// <param name="employee"></param>
    /// <returns></returns>
    public bool CanAssign(Ticket ticket, Employee employee)
    {
        if (!employee.IsActive)
            return false;

        if (ticket.DepartmentId != employee.DepartmentId)
            return false;

        return true;
    }
}
