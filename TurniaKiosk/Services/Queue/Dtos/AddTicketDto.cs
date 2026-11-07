using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queue.Dtos
{
    public class AddTicketDto
    {
        public Guid DeviceId { get; set; }
        public string DeviceKey { get; set; } = null!;
        public Guid Service { get; set; }
    }
}
