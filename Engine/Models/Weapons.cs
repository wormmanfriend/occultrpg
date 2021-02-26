using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Weapons : Item
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public Weapons(int itemTypeID, string name, int price, int minDamge, int maxDamage) 
            : base(itemTypeID, name, price)
        {
            MinDamage = minDamge;
            MaxDamage = maxDamage;
        }

        public new Weapons Clone()
        {
            return new Weapons(ID, Name, Price, MinDamage, MaxDamage);
        }
    }
}
