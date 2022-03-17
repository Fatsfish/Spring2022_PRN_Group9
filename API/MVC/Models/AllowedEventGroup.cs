using System;
using System.Collections.Generic;

#nullable disable

namespace MVC.Models
{
    public partial class AllowedEventGroup
    {
        public int? EventId { get; set; }
        public int? GroupId { get; set; }
        public int Id { get; set; }

        public virtual Event Event { get; set; }
        public virtual Group Group { get; set; }
    }
}
