using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Events;

public interface IDomainEvent //: INotification // La interfaz INotification es opcional
{
    /// <summary>
    /// Fecha y hora (en formato UTC) en la que se produjo el evento de dominio.
    /// </summary>
    DateTime OccurredOn { get; }
}
