using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.API.Models
{
    public partial class InvitationResponseType
    {
        public InvitationResponseType()
        {
            EventInvitations = new HashSet<EventInvitation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EventInvitation> EventInvitations { get; set; }
    }
}
