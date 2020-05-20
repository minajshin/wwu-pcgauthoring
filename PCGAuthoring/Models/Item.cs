﻿namespace PCGAuthoring.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType? Type { get; set; }
    }

    public enum ItemType
    {
        FURNITURE,
        APPLIANCE,
        OBJECT
    }
}