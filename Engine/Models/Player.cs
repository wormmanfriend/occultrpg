using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using Engine.Controllers;

namespace Engine.Models
{
    public class Player : BaseNotificationClass
    {
        
        public string Title { get; set; }
        public string Class { get; set; }
        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                OnPropertyChanged("Health");
                OnPropertyChanged(nameof(IsDead));
            }

        }
        private int _health;
        public int Observations 
        { 
            get { return _observations; }
            set { 
                _observations = value;
                if (_observations >= 8)
                {
                    _wisdom = "SEER";
                    OnPropertyChanged(nameof(Wisdom));
                }
                else _wisdom = "GRUB";
                OnPropertyChanged(nameof(Observations));
                }

         }
        private int _observations;
        public string Wisdom
        {
            get { return _wisdom; }
            set
            {
                _wisdom = value;
                OnPropertyChanged(nameof(Wisdom));
            }

        }
        private string _wisdom;
        public int Azoth
        {
            get { return azoth; }
            set
            {
                azoth = value;
                OnPropertyChanged(nameof(Azoth));
            }

        }
        private int azoth;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }

        }
        private string _status;
        //public int wisdom

        public ObservableCollection<Item> Inventory { get; set; }
        public ObservableCollection<Item> WeaponStash { get; set; }
        public List<Item> Attacks { get; set; }
        public ObservableCollection<QuestStatus> Clues { get; set; }

        public bool IsDead => Health <= 0;

        public Player()
        {
            Inventory = new ObservableCollection<Item>();
            WeaponStash = new ObservableCollection<Item>();
            Clues = new ObservableCollection<QuestStatus>();
            Attacks = new List<Item>();
        }
        
        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);
            OnPropertyChanged(nameof(Attacks));
        }

        public void RemoveItemFromInventory(Item item)
        {
            Inventory.Remove(item);
            OnPropertyChanged(nameof(Item));
        }

        public bool HasRequiredItems(List<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ID == item.ItemID) < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
       
    }
}
