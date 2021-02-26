using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Spell : Weapons
    {
        
        public Spell(int itemTypeID, string name, int price, int minDamge, int maxDamage)
            : base(itemTypeID, name, price, minDamge, maxDamage)
        {
            MinDamage = minDamge;
            MaxDamage = maxDamage;
        }
    }
}
