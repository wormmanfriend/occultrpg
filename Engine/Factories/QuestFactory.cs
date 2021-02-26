using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Engine.Models;

namespace Engine.Factories
{
    static class QuestFactory
    {
        public static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            /*List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity(9001, 1));
            itemsToComplete.Add(new ItemQuantity(9002, 1));
            itemsToComplete.Add(new ItemQuantity(9003, 1));
            rewardItems.Add(new ItemQuantity(001, 1));*/

            //Create the Quest
            _quests.Add(new Quest(1,
                "Therianthrope",
                "Mention of a wandering therianthrope, a \"priest of the devil.\" It was seen with clawed feet and a wolf's snout, carrying a furry oval drum. s.",
                new List<ItemQuantity> { new ItemQuantity(9001, 1), new ItemQuantity(9002,1), new ItemQuantity(9003,1)},
                0,
                0,
                new List<ItemQuantity> {new ItemQuantity(001,1) }));
        }

        internal static Quest GetQuest(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
