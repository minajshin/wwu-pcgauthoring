using System.Collections.Generic;

namespace PCGAuthoring.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }

        public ICollection<ItemAssignment> RoomItems { get; set; }
    }
}
