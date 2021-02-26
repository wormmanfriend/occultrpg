using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public Item(int itemTypeID, string name, int price)
        {
            ID = itemTypeID;
            Name = name;
            Price = price;
        }

        public Item Clone()
        {
            return new Item(ID, Name, Price);
        }
    }
}
