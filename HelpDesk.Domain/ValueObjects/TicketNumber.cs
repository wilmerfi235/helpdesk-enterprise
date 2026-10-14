using HelpDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Domain.ValueObjects;

public sealed class TicketNumber
{
    public string Value { get; }

    private TicketNumber(string value)
    {
        Value = value;
    }

    public static TicketNumber Create(int year, int sequence)
    {
        if (year < 2000)
            throw new DomainException("Invalid year.");

        if (sequence <= 0)
            throw new DomainException("Invalid ticket sequence.");

        var value = $"HD-{year}-{sequence:D6}";

        return new TicketNumber(value);
    }

    public override string ToString()
    {
        return Value;
    }
}
