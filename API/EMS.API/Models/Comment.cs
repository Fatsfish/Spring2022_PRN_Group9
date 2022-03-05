using System;
using System.Collections.Generic;

#nullable disable

namespace EMS.API.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public int? CreationUserId { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User CreationUser { get; set; }
        public virtual Event Event { get; set; }
    }
}
