using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.Models​
{
    public partial class Event
    {
        public Event()
        {
            Comments = new HashSet<Comment>();
            EventTickets = new HashSet<EventTicket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int? CreationUserId { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Place { get; set; }
        public bool IsPublic { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public int? StatusId { get; set; }

        public virtual User CreationUser { get; set; }
        public virtual EventStatus Status { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<EventTicket> EventTickets { get; set; }
    }
}
