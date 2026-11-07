using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queue.Dtos
{
    public class MarkTicketPrintDto
    {
        public Guid Id { get; set; }
        public string DeviceKey { get; set; } = null!;
    }
}
