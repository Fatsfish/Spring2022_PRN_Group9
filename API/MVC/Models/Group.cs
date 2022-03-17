using System;
using System.Collections.Generic;

#nullable disable

namespace MVC.Models
{
    public partial class Group
    {
        public Group()
        {
            AllowedEventGroups = new HashSet<AllowedEventGroup>();
            GroupUsers = new HashSet<GroupUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AllowedEventGroup> AllowedEventGroups { get; set; }
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
    }
}
