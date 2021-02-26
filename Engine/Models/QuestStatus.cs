using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class QuestStatus
    {
        public Quest PlayerQuest { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }

        public QuestStatus(Quest quest, int questid, string questname)
        {
            PlayerQuest = quest;
            ID = questid;
            Name = questname;
            IsCompleted = false;
        }
    }
}
