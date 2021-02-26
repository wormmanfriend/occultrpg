using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public List<ItemQuantity> ItemsToComplete { get; set; }

        public int RewardXp { get; set; }
        public int RewardMoney { get; set; }
        public List<ItemQuantity> RewardItems { get; set; }

        public Quest(int id, string name, string desc, List<ItemQuantity> itemsToComplete, int rewardXp, int rewardMoney, List<ItemQuantity> rewardItems)
        {
            ID = id;
            Name = name;
            Desc = desc;
            ItemsToComplete = itemsToComplete;
            RewardXp = rewardXp;
            RewardMoney = rewardMoney;
            RewardItems = rewardItems;
        }
    }
}
