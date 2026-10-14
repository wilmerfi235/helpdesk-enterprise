using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.Exceptions;

public class BusinessRuleValidationException : DomainException
{
    public BusinessRuleValidationException(string message)
        : base(message)
    {
    }
}
