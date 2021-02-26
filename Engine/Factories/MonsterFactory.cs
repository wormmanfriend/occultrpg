using System;
using System.Collections.Generic;
using System.Text;
using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            switch (monsterID)
            {
                case 1:
                    Monster SHAMAN = new Monster("WANDERER", "shamanIdle.gif", 20, 20, 14,0, 6, 0);

                    AddMonsterLoot(SHAMAN, 9001, 100);
                    AddMonsterLoot(SHAMAN, 9002, 100);
                    AddMonsterLoot(SHAMAN, 9003, 100);
                   

                    return SHAMAN;
                default:
                    throw new ArgumentException(string.Format("id {0} not recognized", monsterID));
            }
        }

        private static void AddMonsterLoot(Monster monster, int itemID, int percentage)
        {
            if (RNG.NumberBetween(1,100) <= percentage)
            {
                monster.Inventory.Add(new ItemQuantity(itemID, 1));
            }
        }
    }
}
