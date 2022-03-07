using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.Models
{
    public partial class EventTicket
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public int? OwnerId { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }

        public virtual Event Event { get; set; }
        public virtual User Owner { get; set; }
    }
}
