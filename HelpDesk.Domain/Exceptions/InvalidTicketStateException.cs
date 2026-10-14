using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Exceptions;

public class InvalidTicketStateException : DomainException
{
    public InvalidTicketStateException(string message)
        : base(message)
    {
    }
}
