using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Trader
    {
        public string Name { get; set; }
        public ObservableCollection<Item> Inventory { get; set; }
        public Trader(string name)
        {
            Name = name;
            Inventory = new ObservableCollection<Item>();
        }
        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }
        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }
    }
}
