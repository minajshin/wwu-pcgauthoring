namespace PCGAuthoring.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public ItemType? Type { get; set; }
    }

    public enum ItemType
    {
        FURNITURE,
        APPLIANCE,
        OBJECT
    }
}