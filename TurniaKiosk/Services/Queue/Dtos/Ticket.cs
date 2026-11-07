using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queue.Dtos
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string TicketLetter { get; set; } = null!;
        public int TicketNumberSequence { get; set; }
        public DateTime TicketDate { get; set; }
        public bool TicketIsPrinted { get; set; }
    }
}
