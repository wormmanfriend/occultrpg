using System;
using System.Collections.Generic;
using System.Text;
using Engine.Models;
using System.Linq;

namespace Engine.Factories
{
    class TraderFactory
    {
        private static readonly List<Trader> traders = new List<Trader>();

        TraderFactory()
        {
            Trader Apothecary = new Trader("Apothecary");
            Apothecary.AddItem(ItemFactory.CreateItem(2006));
        }

        public static Trader GetTraderbyName(string name)
        {
            return traders.FirstOrDefault(tr => tr.Name == name);
        }
        public void AddTraderToList(Trader trader)
        {
            if (traders.Any(tr => tr.Name == trader.Name))
            {
                throw new ArgumentException("You added a trader that already exists !!!!! wep");
            }
            traders.Add(trader);
        }
    }
}
