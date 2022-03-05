using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.Models​
{
    public partial class EventStatus
    {
        public EventStatus()
        {
            Events = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
