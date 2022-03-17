using System;
using System.Collections.Generic;

#nullable disable

namespace MVC.Models
{
    public partial class GroupUser
    {
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
        public int Id { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
