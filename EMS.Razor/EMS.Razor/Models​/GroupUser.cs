using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.Razor.Models​
{
    public partial class GroupUser
    {
        public int? UserId { get; set; }
        public int? GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
