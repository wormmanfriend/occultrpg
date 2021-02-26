using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Engine.Factories;

namespace Engine.Models
{
    public class Location
    {
        public int Xcord { get; set; }
        public int Ycord { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ImageName { get; set; }
        public string Button { get; set; }

        public List<Quest> QuestsHere { get; set; } = new List<Quest>();
        public List<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();
        public Trader TraderHere { get; set; }

        public void AddMonster(int monsterid, int encounterchance)
        {
            if(MonstersHere.Exists(m => m.MonsterID == monsterid))
            {
                MonstersHere.First(m => m.MonsterID == monsterid).EncounterChance = encounterchance;
            }
            else
            {
                MonstersHere.Add(new MonsterEncounter(monsterid, encounterchance));
            }
        }

        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }
            //total percantage chances for monster encounters
            int totalChances = MonstersHere.Sum(m => m.EncounterChance);
            //picking a random number between 1 and the total
            int randomNumber = RNG.NumberBetween(1, totalChances);
            //
            int runningTotal = 0;

            foreach(MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.EncounterChance;

                if(randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterID);
                }
            }

            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);

        }
    }
}
