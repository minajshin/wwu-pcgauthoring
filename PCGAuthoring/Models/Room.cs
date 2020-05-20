using System.Collections.Generic;

namespace PCGAuthoring.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ItemAssignment> RoomItems { get; set; }
    }
}
