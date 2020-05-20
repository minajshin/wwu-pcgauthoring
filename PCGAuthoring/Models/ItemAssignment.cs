using System.ComponentModel.DataAnnotations;

namespace PCGAuthoring.Models
{
    public class ItemAssignment
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ItemId { get; set; }
        
        [Range(0, 10)]
        public int Min { get; set; }

        [Range(0, 10)]
        public int Max { get; set; }


        // Navigational property
        public Room Room { get; set; }
        public Item Item { get; set; }
    }
}