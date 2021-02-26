using System;
using System.Collections.Generic;
using System.Text;
using Engine.Models;
using System.Linq;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        public static List<Item> _standardItems = new List<Item>();

        static ItemFactory()
        {
            //weapons
            _standardItems.Add(new Weapons(1001, "Mattock", 0,7,14));
            _standardItems.Add(new Weapons(1002, "Dirty Dagger", 4, 2, 4));
            _standardItems.Add(new Weapons(1003, "Femur", 5, 15, 19));

            //trading items
            _standardItems.Add(new Item(2001, "Heretical Ring", 3));
            _standardItems.Add(new Item(2002, "Tender Memory", 15));
            _standardItems.Add(new Item(2003, "Sentimental Clutter", 4));
            _standardItems.Add(new Item(2004, "Vial of Blood",5));
            _standardItems.Add(new Item(2005, "Vial of Blood", 5));
            _standardItems.Add(new Item(2006, "Glass Vial", 2));
            _standardItems.Add(new Item(2007, "Calm Thoughts", 6));


            //spells
            _standardItems.Add(new Weapons(3001, "Drink Blood",0,0,0));
            _standardItems.Add(new Weapons(3002, "Blood Divination",0,0,0));
            _standardItems.Add(new Weapons(3003, "Draw Blood", 0, 0, 0));

            //ingredients for endgame items
            //SHAMAN
            _standardItems.Add(new Item(9001, "Occipital Lobe",10));
            _standardItems.Add(new Item(9002, "Ribcage", 6));
            _standardItems.Add(new Item(9003, "Therianthropics", 0));

            //endgame items
            //SHAMAN
            _standardItems.Add(new Item(6001, "Guiding Spirit",0));
           
        }

        public static Item CreateItem(int itemTypeID)
        {
            Item standardItem = _standardItems.FirstOrDefault(item => item.ID == itemTypeID);

            if( standardItem != null)
            {
                if(standardItem is Weapons)
                {
                    return (standardItem as Weapons).Clone();
                }
                return standardItem.Clone();
            }
            return null;
        }
    }
}
