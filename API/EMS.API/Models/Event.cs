using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.API.Models
{
    public partial class Event
    {
        public Event()
        {
            AllowedEventGroups = new HashSet<AllowedEventGroup>();
            Comments = new HashSet<Comment>();
            EventInvitations = new HashSet<EventInvitation>();
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
        public string Images { get; set; }

        public virtual User CreationUser { get; set; }
        public virtual EventStatus Status { get; set; }
        public virtual ICollection<AllowedEventGroup> AllowedEventGroups { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<EventInvitation> EventInvitations { get; set; }
        public virtual ICollection<EventTicket> EventTickets { get; set; }
    }
}
