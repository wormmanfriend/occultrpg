using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster : BaseNotificationClass
    {
        private int _hp;

        public string Name { get; private set; }
        public string ImageName { get; set; }
        public int MaxHp { get; private set; }
        public int Hp
        {
            get { return _hp; }
            set { _hp = value; OnPropertyChanged(nameof(Hp)); }
        }
        public int MaxDmg { get; set; }
        public int MinDmg { get; set; }
        public int RewardXp { get; private set; }
        public int RewardAzoth { get; private set; }
        public ObservableCollection<ItemQuantity> Inventory { get; set; }

        public Monster(string name, string imagename, int maxhp, int hp, int maxdmg, int mindmg, int rewardxp, int rewardazoth)
        {
            Name = name;
            ImageName = string.Format("/WPFUI;component/image/monsters/{0}", imagename);
            MaxHp = maxhp;
            Hp = hp;
            MaxDmg = maxdmg;
            MinDmg = mindmg;
            RewardXp = rewardxp;
            RewardAzoth = rewardazoth;

            Inventory = new ObservableCollection<ItemQuantity>();
        }
    }
}
