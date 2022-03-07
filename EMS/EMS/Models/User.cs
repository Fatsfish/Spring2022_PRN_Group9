using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            EventInvitations = new HashSet<EventInvitation>();
            EventTickets = new HashSet<EventTicket>();
            Events = new HashSet<Event>();
            GroupUsers = new HashSet<GroupUser>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<EventInvitation> EventInvitations { get; set; }
        public virtual ICollection<EventTicket> EventTickets { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
