using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Exceptions;

public class TicketAssignmentException : DomainException
{
    public TicketAssignmentException(string message)
        : base(message)
    {
    }
}
