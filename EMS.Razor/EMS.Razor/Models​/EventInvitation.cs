﻿using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.Models​
{
    public partial class EventInvitation
    {
        public int? EventId { get; set; }
        public int? UserId { get; set; }
        public DateTime SentDate { get; set; }
        public int? InvitationResponseId { get; set; }
        public string TextResponse { get; set; }
        public DateTime ResponseDate { get; set; }

        public virtual Event Event { get; set; }
        public virtual InvitationResponseType InvitationResponse { get; set; }
        public virtual User User { get; set; }
    }
}
