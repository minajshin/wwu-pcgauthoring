using System.ComponentModel.DataAnnotations;

namespace PCGAuthoring.Models
{
    public class ItemAssignment
    {
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int ItemID { get; set; }
        
        [Range(0, 10)]
        public int MinAssignedNum { get; set; }

        [Range(0, 10)]
        public int MaxAssignedNum { get; set; }


        // Navigational property
        public Room Room { get; set; }
        public Item Item { get; set; }
    }
}