using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using Engine.Models;
using Engine.Factories;
using Engine.EventArgs;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Diagnostics;


namespace Engine.Controllers
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        private Location _currentLocationz;
        private Monster currentMonster; 
        public Player CurrentPlayer { get; set; }
        public Quest QuestInfo { get; set; }
        public Weapons CurrentWeapon { get; set; }
        public Item SelectedItem { get; set; }
        public World CurrentWorld { get; set; }
        private Trader currentTrader;
        public Trader CurrentTrader
        {
            get { return currentTrader; }
            set { currentTrader = value; OnPropertyChanged(nameof(CurrentTrader)); OnPropertyChanged(nameof(HasTrader)); }
        }
        public Monster CurrentMonster
        {
            get { return currentMonster; }
            set
            {
                currentMonster = value;
                OnPropertyChanged(nameof(CurrentMonster)); OnPropertyChanged(nameof(HasMonster));
                if (CurrentMonster != null)
                {
                    MonsterMessages();
                }
            }
        }
        public Location CurrentLocation
        {
            get { return _currentLocationz; }
            set
            {
                _currentLocationz = value;
                OnPropertyChanged(nameof(IsSafe));
                OnPropertyChanged(nameof(IsFighting));
                OnPropertyChanged("CurrentLocation");
                OnPropertyChanged("LocationToNorth");
                OnPropertyChanged("LocationToWest");
                OnPropertyChanged("LocationToEast");
                OnPropertyChanged("LocationToSouth");
                OnPropertyChanged(nameof(ButtonVisibility));
              
                //GiveQuestsHere();
                GiveMonsterHere();
                ExplorationSwitches();


            }
        }
        private string leftButton = "";
        public string LeftButton
        {
            get { return leftButton; }
            set { leftButton = value; ; OnPropertyChanged(nameof(LeftButton)); }
        }
        private string rightButton = "";
        public string RightButton
        {
            get { return rightButton; }
            set { rightButton = value; OnPropertyChanged(nameof(RightButton)); }
        }
        public GameSession()
        {
            CurrentPlayer = new Models.Player //object initialization, getting
            {
                Class = "|░|DIVINER|░|",
                //Wisdom = "a",                //other levels eg. seer, prophet, psion  
                Observations = 0,
                Health = 50,
                Azoth = 4,
                Status = "..."
            };
            ItemInfo = new InfoTabItem("","","","",false);

            /*CurrentLocation = new Location();
            CurrentLocation.Name = "    CABIN\n--------------";
            CurrentLocation.Desc = "Deep in a common woods, your cabin. A stained, putrid mess. \n\nFilled with ritual ingredients, found artifacts, and human carcasses, it's your occultist hoarder's workshop.";
            CurrentLocation.ImageName = "/WPFUI;component/image/locations/cabinpico.png";
            CurrentLocation.Xcord = 0;
            CurrentLocation.Ycord = -1;*/

            //^^old clunky way of calling location variables, new easy way of swapping out location variable sets below

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, -1);

            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2001));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2002));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2004));

            CurrentPlayer.WeaponStash.Add(ItemFactory.CreateItem(1001));

            CurrentPlayer.Attacks.Add(ItemFactory.CreateItem(1001));

            CurrentPlayer.Attacks.Add(ItemFactory.CreateItem(3002));
            CurrentPlayer.Attacks.Add(ItemFactory.CreateItem(3003));
            CurrentPlayer.Attacks.Add(ItemFactory.CreateItem(3001));






        }
        //GAMEMESSAGES
        private void MonsterMessages()
        {
        
        }
        string TextBreak = $".............\n";
        public bool Seer = false;
        public bool HasTrader => CurrentTrader != null;

        public void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }

        //QUEST METHOD
        private void GiveQuestsHere()
        {
            foreach (Quest quest in CurrentLocation.QuestsHere)
            {
                if (!CurrentPlayer.Clues.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Clues.Add(new QuestStatus(quest, quest.ID, quest.Name));
                }
            }
        }

        //MONSTER CHECK
        public bool FightingShaman=false;
        public bool FightingShamanExit = false;
        public bool HasMonster => CurrentMonster != null;
        public bool IsSafe
        {
            get
            {
                if(CurrentLocation.Xcord == 1 && CurrentLocation.Ycord == 0 && FightingShaman == true)
                {
                    return false;
                }
                else return true;
            }
            
        }
        public bool IsFighting
        {
            get
            {
                if (CurrentLocation.Xcord == 1 && CurrentLocation.Ycord == 0 && FightingShaman == true|| CurrentLocation.Xcord == -1 && CurrentLocation.Ycord == 0 && trading)
                {
                    return true;
                }
                else return false;
            }
        }
        public bool YesNoChoice
        {
            get
            {
                if (CurrentLocation.Xcord == 1 && CurrentLocation.Ycord == 0 && BrainSearching ||CurrentLocation.Xcord==-1 && CurrentLocation.Ycord==0 && trading)
                {
                    return true;
                }
                else return false;
            }
        }

        private void GiveMonsterHere()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        bool BrainSearching = false;
        int InitialMonsterHP = 0;

        //COMBAT
        bool ShamanDefeated = false;
        bool DamageBonus = false;
        bool ItemUsed = false;
        public void Attack()
        {
            if(CurrentWeapon == null)
            {
                return;
            }

            int DamageToEnemy = RNG.NumberBetween(CurrentWeapon.MinDamage, CurrentWeapon.MaxDamage);
            InitialMonsterHP = CurrentMonster.Hp;

            //DRINK BLOOD
            if (CurrentWeapon.ID == 3001)
            {
                if (CurrentPlayer.Inventory.Any(o => o.ID ==2004))
                {
                    RaiseMessage("You drink the blood in your vial and chant. You're rejuvenated. (+)\n");
                    RaiseMessage(TextBreak);
                    CurrentPlayer.Health += 10;
                    if (CurrentPlayer.Health >= 50)
                    {
                        CurrentPlayer.Health = 60;
                    }
                    var itemToRemove = CurrentPlayer.Inventory.SingleOrDefault(p => p.ID == 2004);
                    CurrentPlayer.Inventory.Remove(itemToRemove);
                    CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2006));
                    DamageBonus = true;
                    ItemUsed = true;
                    if (ItemInfo.Name == "Vial of Blood")
                    {
                        GetInfoGlassVial();
                        ShowInfo();
                    }


                }
                else
                {
                    RaiseMessage("You don't have blood.\n");
                    RaiseMessage(TextBreak);
                }
            }
            //DRAW BLOOD
            if (CurrentWeapon.ID == 3003)
            {
                if (CurrentPlayer.Inventory.Any(o => o.ID == 2006))
                {
                    RaiseMessage("You cut yourself and collect the blood in a glass vial.\n");
                    RaiseMessage("--| 11 damage taken |--\n");
                    RaiseMessage(TextBreak);
                    CurrentPlayer.Health -= 11;

                    var itemToRemove = CurrentPlayer.Inventory.SingleOrDefault(p => p.ID == 2006);
                    CurrentPlayer.Inventory.Remove(itemToRemove);
                    CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2004));
                    
                    if(ItemInfo.Name=="Glass Vial")
                    {
                        GetInfoVialOfBlood();
                        ShowInfo();
                    }
                }
                else
                {
                    RaiseMessage("You cut yourself but can't store the blood. It goes to waste.\n");
                    RaiseMessage("--| 11 damage taken |--\n");
                    RaiseMessage(TextBreak);
                    CurrentPlayer.Health -= 11;
                }
            }
            //MANIPULATE BLOOD
            if(CurrentWeapon.ID == 3002)
            {
                if (CurrentPlayer.Inventory.Any(o => o.ID == 2004) && CurrentPlayer.Inventory.Any(o => o.ID == 2005))
                {
                    RaiseMessage($"");
                }
                else
                {
                    if (CurrentPlayer.Inventory.Any(o => o.ID == 2004))
                    {
                        RaiseMessage("You don't have enough blood.\n");
                        RaiseMessage(TextBreak);
                    }
                    else
                    {
                        RaiseMessage("You don't have blood.\n");
                        RaiseMessage(TextBreak);
                    }
                    
                }
            }
            //MATTOCK ATTACK
            if (CurrentWeapon.ID == 1001)
            {
                if(DamageToEnemy >= 12)
                {
                    RaiseMessage($"You swing the tool.\n");

                    if (DamageBonus)
                    {
                        RaiseMessage($"++| {DamageToEnemy} (+3) damage dealt |++\n");
                    }
                    else { RaiseMessage($"++| {DamageToEnemy} damage dealt |++\n"); }
                    RaiseMessage(TextBreak);
                    if (DamageBonus)
                    {
                        CurrentMonster.Hp -= DamageToEnemy + 3;
                    }
                    else { CurrentMonster.Hp -= DamageToEnemy; }
                    DamageBonus = false;
                }
                else if(DamageToEnemy>8)
                {
                    RaiseMessage($"You swing the tool.\n");
                    if (DamageBonus)
                    {
                        RaiseMessage($"++| {DamageToEnemy} (+3) damage dealt |++\n");
                    }
                    else { RaiseMessage($"++| {DamageToEnemy} damage dealt |++\n"); }
                    RaiseMessage(TextBreak);
                    if (DamageBonus)
                    {
                        CurrentMonster.Hp -= DamageToEnemy + 3;
                    }
                    else { CurrentMonster.Hp -= DamageToEnemy; }
                    DamageBonus = false;
                }
                else
                {
                    RaiseMessage($"You swing the tool. You're distracted.\n");
                    if (DamageBonus)
                    {
                        RaiseMessage($"++| {DamageToEnemy} (+3) damage dealt |++\n");
                    }
                    else { RaiseMessage($"++| {DamageToEnemy} damage dealt |++\n"); }
                    RaiseMessage(TextBreak);
                    if (DamageBonus)
                    {
                        CurrentMonster.Hp -= DamageToEnemy + 3;
                    }
                    else { CurrentMonster.Hp -= DamageToEnemy; }
                    DamageBonus = false;
                } 
            }
            if(CurrentMonster.Hp <= 0)
            {
                CurrentMonster.Hp = 0;
            }
        }

        public void MonsterAttack()
        {
            //OnPropertyChanged(nameof(IsDead));
            int DamageToPlayer = RNG.NumberBetween(CurrentMonster.MinDmg, CurrentMonster.MaxDmg);
            if (!CurrentMonster.Hp.Equals(InitialMonsterHP) || ItemUsed)
            {
                ItemUsed = false;
                if (CurrentMonster.Name == "WANDERER")
                {
                    if (currentMonster.Hp <= 0)
                    {
                        CurrentMonster.ImageName = "/WPFUI;component/image/monsters/shamanDead.gif";
                        OnPropertyChanged(nameof(CurrentMonster));
                        if (CurrentMonster.Name == "WANDERER")
                        {
                            RaiseMessage("--------------------------------------------------------------------------\n");
                            RaiseMessage("The WANDERER is dead. You observe their last movements and bloodflow.\n");
                            CurrentPlayer.Observations += CurrentMonster.RewardXp;
                            RaiseMessage($"You note {CurrentMonster.RewardXp} observations.\n");

                            CheckLevelUp();

                            foreach (ItemQuantity itemQuantity in CurrentMonster.Inventory)
                            {
                                Item item = ItemFactory.CreateItem(itemQuantity.ItemID);
                                RaiseMessage($"Harvested [{item.Name}].");
                                CurrentPlayer.Inventory.Add(item);
                            }
                            if (CurrentPlayer.Inventory.Any(p => p.ID == 2006))
                            {
                                CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2004));
                                RaiseMessage("\nCollected blood.");
                                var itemToRemove = CurrentPlayer.Inventory.SingleOrDefault(p => p.ID == 2006);
                                CurrentPlayer.Inventory.Remove(itemToRemove);
                                if (ItemInfo.Name == "Glass Vial")
                                {
                                    GetInfoVialOfBlood();
                                    ItemInfo.ItemIsInteractable = true;
                                    ShowInfo();
                                }
                            }
                            RaiseMessage("\nAttempt sacrificial BRAIN HARVEST?\n");
                            RaiseMessage(TextBreak);
                            RightButton = "No";
                            LeftButton = "Yes";
                            BrainSearching = true;
                            OnPropertyChanged(nameof(YesNoChoice));
                            ShamanDefeated = true;

                        }
                    }
                    else if(currentMonster.Hp <= 5)
                    {
                        if (DamageToPlayer > 8)
                        {
                            RaiseMessage("The WANDERER stabs you with a dirty looking dagger.\n");
                        }
                        else if(DamageToPlayer>0)
                        {
                            RaiseMessage("The WANDERER hits you with a wooden drumstick.\n");
                        }
                        else
                        {
                            RaiseMessage("The WANDERER stumbles around.");
                        }
                        RaiseMessage($"--| {DamageToPlayer} damage taken |--\n");
                        RaiseMessage(TextBreak);
                        CurrentPlayer.Health -= DamageToPlayer;
                    }
                    else if(currentMonster.Hp <= 19)
                    {
                        RaiseMessage("The WANDERER says some words you don't understand.\n");
                        RaiseMessage(TextBreak);
                    }
                }
            }
            
        }

        public void ClickLeft()
        {
            if (BrainSearching && CurrentMonster.Name == "WANDERER")
            {
                int PercentCalmThought = RNG.NumberBetween(0, 10);
                if (PercentCalmThought > 5)
                {
                    CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2007));
                    CurrentPlayer.Health -= 5;
                    RaiseMessage("You extracted some Calm Thoughts.\n");
                    RaiseMessage("--| 5 damage taken |--\n");
                }
                else
                {
                    CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2003));
                    CurrentPlayer.Health -= 5;
                    RaiseMessage("You extracted some Sentimental Clutter.\n");
                    RaiseMessage("--| 5 damage taken |--\n");
                }
                BrainSearching = false;
                OnPropertyChanged(nameof(YesNoChoice));
                FightingShamanExit = false;
                FightingShaman = false;
                OnPropertyChanged(nameof(IsFighting));
                OnPropertyChanged(nameof(IsSafe));
                OnPropertyChanged(nameof(LocationToWest));
                RaiseMessage(TextBreak);

                CurrentLocation.ImageName = "/WPFUI;component/image/monsters/shamanBrain.gif";
                CurrentLocation.Desc = "";
                CurrentLocation.Name = "WANDERER";
                OnPropertyChanged(nameof(CurrentLocation));
                CurrentLocation.ImageName = "/WPFUI;component/image/locations/cave.gif";
                CurrentLocation.Desc = "A small cave entrance and a corpse.";
                CurrentLocation.Name = "Cave";
            }
        }
        public void ClickRight()
        {
            if (BrainSearching && CurrentMonster.Name == "WANDERER")
            {
                BrainSearching = false;
                OnPropertyChanged(nameof(YesNoChoice));
                FightingShaman = false;
                FightingShamanExit = false;
                OnPropertyChanged(nameof(IsFighting));
                OnPropertyChanged(nameof(IsSafe));
                OnPropertyChanged(nameof(LocationToWest));

                RaiseMessage("You step away from the corpse. It stinks.\n");
                RaiseMessage(TextBreak);

                CurrentLocation.ImageName = "/WPFUI;component/image/locations/cave.gif";
                CurrentLocation.Desc = "A small cave and a corpse.";
                OnPropertyChanged(nameof(CurrentLocation));

                // Process.Start(Application.ResourceAssembly.Location);
                //Application.Current.Shutdown();

            }
        }

        //MOVEMENT
        #region
        public bool leftWoods = false;
        public void MoveNorth()
        {
            if (LocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.Xcord, CurrentLocation.Ycord + 1);
            }

        }
        public void MoveWest()
        {
            if (LocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.Xcord - 1, CurrentLocation.Ycord);
            }
        }
        public void MoveEast()
        {
            if (LocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.Xcord + 1, CurrentLocation.Ycord);
                leftWoods = true;
            }
        }
        public void MoveSouth()
        {
            if (LocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.Xcord, CurrentLocation.Ycord - 1);
            }
        }
        #endregion

        //POTENTIAL LOCATION CHECKS, HIDE BUTTONS
        #region
        public bool LocationToNorth
        {
            get
            {
                if(CurrentLocation.Xcord==-1 && CurrentLocation.Ycord==-1||CurrentLocation.Xcord==1 && CurrentLocation.Ycord == -1)
                {
                    return false;
                }
                
                return CurrentWorld.LocationAt(CurrentLocation.Xcord, CurrentLocation.Ycord + 1) != null;
            }
        }
        public bool LocationToWest
        {
            get
            {
                if (FightingShaman == true)
                {
                    return false;
                    
                }
                return CurrentWorld.LocationAt(CurrentLocation.Xcord - 1, CurrentLocation.Ycord) != null;
            }
        }
        public bool LocationToEast
        {
            get
            {
                if(CurrentLocation.Xcord==0 && CurrentLocation.Ycord==0 && talkedToNomad == false|| CurrentLocation.Xcord==1 && CurrentLocation.Ycord==0 && ShamanDefeated==false)
                {
                    return false;
                }
                return CurrentWorld.LocationAt(CurrentLocation.Xcord + 1, CurrentLocation.Ycord) != null;

            }
        }
        public bool LocationToSouth
        {
            get
            {
                if(CurrentLocation.Xcord==-1 && CurrentLocation.Ycord==0|| CurrentLocation.Xcord==1 && CurrentLocation.Ycord == 0)
                {
                    return false;
                }
                return CurrentWorld.LocationAt(CurrentLocation.Xcord, CurrentLocation.Ycord - 1) != null;
            }
        }
        #endregion

        bool FoundShaman = false;
        public void ExplorationSwitches()
        {
            if (CurrentLocation.Xcord == 1 && CurrentLocation.Ycord == 0 && FoundShaman == false)
            {
                CurrentWorld.LocationAt(1, 0).Desc = "The wanderer is sitting in front of the cave.";
                FoundShaman = true;
            }
            if (talkedToNomad == true)
            {
                CurrentWorld.LocationAt(0, 0).Desc = "The nomad is gone. The woods look the same.";
                CurrentWorld.LocationAt(0,0).ImageName = "/WPFUI;component/image/locations/woods.gif";
            }
        }

        //CONTEXTUAL BUTTONS
        public bool talkedToNomad = false;
        public bool trading = false;

        public async Task ContextualAction()
        {
            //TALK TO NOMAD
            if (CurrentLocation.Xcord == 0 && CurrentLocation.Ycord == 0 && talkedToNomad == false)
            {

                CurrentLocation.Desc = "The nomad talks about a wandering \"devil's priest.\" \n\nThey had clawed feet and carried a furry oval drum. A hood was hiding their face. \n\nThe nomad points East.\n\n[+1 CLUE]";
                CurrentLocation.ImageName = "/WPFUI;component/image/locations/nomad.gif";
                talkedToNomad = true;
                GiveQuestsHere();
                OnPropertyChanged(nameof(GiveQuestsHere));
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(ButtonVisibility));
                OnPropertyChanged(nameof(LocationToEast));

            }
            //START FIGHT SHAMAN
            else if (CurrentLocation.Xcord == 1 && CurrentLocation.Ycord == 0)
            {
                //RaiseMessage("You found the WANDERER in human form.\n");
                // RaiseMessage(TextBreak);
                FightingShaman = true;
                FightingShamanExit = true;
                OnPropertyChanged(nameof(IsSafe));
                OnPropertyChanged(nameof(IsFighting));
                OnPropertyChanged(nameof(LocationToWest));
                OnPropertyChanged(nameof(ButtonVisibility));
            }
            //APOTHECARY INTERACT
            if(CurrentLocation.Xcord==-1 && CurrentLocation.Ycord == 0)
            {
                trading = true;
                LeftButton = "Trade";
                RightButton = "Question";
                FightingShaman = true;
                OnPropertyChanged(nameof(IsFighting));
                OnPropertyChanged(nameof(YesNoChoice));
                
            }
            //CRAFT
            if(CurrentLocation.Xcord==-1 && CurrentLocation.Ycord == -1)
            {
                //SHAMANITEM
                if (CurrentPlayer.Clues.Any(clue=>clue.ID==1))
                {
                    if (!CurrentPlayer.Clues.First(clue => clue.ID == 1).IsCompleted)
                    {
                        var ShamanQuest = QuestFactory.GetQuest(1);
                        if (CurrentPlayer.HasRequiredItems(ShamanQuest.ItemsToComplete))
                        {
                            foreach (ItemQuantity item in ShamanQuest.ItemsToComplete)
                            {
                                for (int i = 0; i < item.Quantity; i++)
                                {
                                    CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(ite => ite.ID == item.ItemID));
                                }
                            }

                            CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(6001));

                            CurrentPlayer.Clues.First().IsCompleted = true;
                            if (ItemInfo.Name == "Therianthrope")
                            {
                                ItemInfo.Desc = "The materials harvested from the wanderer were used and the guide is complete.";
                                ShowInfo();
                            }
                            CurrentPlayer.Status = "(x) GUIDING SPIRIT...   ...";
                            RaiseMessage("You combine what you harvested from the wanderer into a guide for your ritual. \nThe spirit is tortured but subservient.\n");
                            RaiseMessage("You note [2] observations.\n");
                            CurrentPlayer.Observations += 1;
                            RaiseMessage(TextBreak);
                            await Task.Delay(1000);
                            CheckLevelUp();
                        }
                    }
                    else
                    {
                        RaiseMessage("You don't have enough ingredients.\n");
                        RaiseMessage(TextBreak);
                    }
                }
                else
                {
                    RaiseMessage("You don't have enough ingredients.\n");
                    RaiseMessage(TextBreak);
                }
            }
        }

        //INSPECT BUTTON
        bool CalmDeciphered1 = false;
        bool CalmDeciphered2 = false;
        bool ClutterDeciphered = false;
        public bool ItemButtonSwitched = false;
        public void InteractWithItem()
        {
            if (ItemInfo.Name == "Vial of Blood")
            {
                RaiseMessage("You drink the blood in the vial. You feel much better. (+)\n");
                RaiseMessage(TextBreak);
                CurrentPlayer.Health += 10;
                var itemToRemove = CurrentPlayer.Inventory.SingleOrDefault(p => p.ID == 2004);
                CurrentPlayer.Inventory.Remove(itemToRemove);
                CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2006));
                DamageBonus = true;
                ItemButtonSwitched = true;
                GetInfoGlassVial();
                ShowInfo();

            }
            else if(ItemInfo.Name == "Glass Vial")
            {
                RaiseMessage("You cut yourself and collect the blood in a glass vial.\n");
                RaiseMessage("--| 11 damage taken |--\n");
                RaiseMessage(TextBreak);
                CurrentPlayer.Health -= 11;

                var itemToRemove = CurrentPlayer.Inventory.SingleOrDefault(p => p.ID == 2006);
                CurrentPlayer.Inventory.Remove(itemToRemove);
                CurrentPlayer.Inventory.Add(ItemFactory.CreateItem(2004));
                ItemButtonSwitched = true;
                GetInfoVialOfBlood();
                ShowInfo();
            }
            if (ItemInfo.Name == "Tender Memory")
            {
                if(CurrentPlayer.Wisdom == "GRUB")
                {
                    RaiseMessage("You can't remember.\n");        
                    RaiseMessage(TextBreak);
                    
                }
            }
            if (ItemInfo.Name == "Sentimental Clutter")
            {
                if (CurrentPlayer.Wisdom == "GRUB" && !ClutterDeciphered)
                {
                    RaiseMessage("¦Piety. Family. Indulging and devoting yourself. There's people around you but you can't see their faces.¦\n\nYou note [1] observation\n");
                    CurrentPlayer.Observations++;
                    RaiseMessage(TextBreak);
                    ClutterDeciphered = true;
                }
                else
                {
                    RaiseMessage("You don't know how to get more from this thought right now. It hurts to try.\n");
                    CurrentPlayer.Health -= 2;
                    RaiseMessage("--| 2 damage taken |--\n");
                    RaiseMessage(TextBreak);
                }
            }
            if (ItemInfo.Name == "Calm Thoughts")
            {
                if (CurrentPlayer.Wisdom == "GRUB" && !CalmDeciphered1)
                {
                    RaiseMessage("¦You hear rushing water and you're sitting in clay. You're etching symbols into stone and the ceiling is made of stone too.¦ \n\nYou note [1] observation\n");
                    CurrentPlayer.Observations++;
                    RaiseMessage(TextBreak);
                    CalmDeciphered1 = true;
                }
                else if(Seer && !CalmDeciphered2)
                {
                    RaiseMessage("¦It's dark where you are, so much that you've gone blind. You can hear more now, like words in the rushing water.¦ \n\nYou note [2] observations\n");
                    CurrentPlayer.Observations++;
                    RaiseMessage(TextBreak);
                    CalmDeciphered2 = true;
                }
                else 
                { 
                    RaiseMessage("You don't know how to get more from this thought right now. It hurts to try.\n");
                    CurrentPlayer.Health -= 2;
                    RaiseMessage("--| 2 damage taken |--\n");
                    RaiseMessage(TextBreak);
                }
            }
        }
        public bool ButtonVisibility
        {
            get
            {
                //No button at starting location
                if (CurrentLocation.Xcord == 0 && CurrentLocation.Ycord == -1)
                {
                    return false;
                }
                //No button at Ritual Ground
                if(CurrentLocation.Xcord ==1 && CurrentLocation.Ycord == -1)
                {
                    return false;
                }
                //No button after talking to nomad
                else if (talkedToNomad == true && CurrentLocation.Xcord == 0 && CurrentLocation.Ycord == 0)
                {
                    return false;
                }
                //No button after initiating fight with shaman
                else if(CurrentLocation.Xcord==1 && CurrentLocation.Ycord==0 && FightingShaman == true || CurrentLocation.Xcord == 1 && CurrentLocation.Ycord == 0 && ShamanDefeated == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public InfoTabItem ItemInfo { get; set; }
        public bool ItemClickedYes = false;
        public bool ItemClicked
        {
            get
            {
                if (ItemClickedYes)
                {
                    return true;
                }
                else return false;
            }
        }

        //ITEM INFO METHODS
        string ItemImagePath = $"/WPFUI;component/image/items/";
        public void ShowInfo()
        {
            OnPropertyChanged(nameof(ItemClicked));
            OnPropertyChanged(nameof(ItemInfo));
        }
        public void GetInfoRing()
        {
            ItemInfo.Name = "Heretical Ring";
            ItemInfo.ImageName = $"{ItemImagePath}hereticalring.gif";
            ItemInfo.Desc = "Engraved with a dated insignia.";
        }
        public void GetInfoTenderMemory()
        {
            ItemInfo.Name = "Tender Memory";
            ItemInfo.ImageName = $"{ItemImagePath}tendermemory.gif";
            ItemInfo.Desc = "A good feeling."; //  \n\n[(-)]"Yourself and someone else during a difficult time.
            ItemInfo.ButtonContent = "Remember";
        }
        public void GetInfoVialOfBlood()
        {
            ItemInfo.Name = "Vial of Blood";
            ItemInfo.ImageName = $"{ItemImagePath}vialofblood.gif";
            ItemInfo.Desc = "5 mL of human blood.";
            ItemInfo.ButtonContent = "Drink";
        }
        public void GetInfoGlassVial()
        {
            ItemInfo.Name = "Glass Vial";
            ItemInfo.ImageName = $"{ ItemImagePath}glassvial.gif";
            ItemInfo.Desc = "An empty glass container.";
            ItemInfo.ButtonContent = "Draw Blood";
        }
        public void GetInfoMattock()
        {
            ItemInfo.Name = "Mattock";
            ItemInfo.ImageName = $"{ ItemImagePath}mattock.gif";
            ItemInfo.Desc = "A gardening tool. \n\nCombines a pick and an adze.";
        }
        public void GetInfoOccipital()
        {
            ItemInfo.Name = "Occipital Lobe";
            ItemInfo.ImageName = $"{ ItemImagePath}shamansoccipital.gif";
            ItemInfo.Desc = "Perceptive brain lobe.";
        }
        public void GetInfoCalmThoughts ()
        {
            ItemInfo.Name = "Calm Thoughts";
            ItemInfo.ImageName = $"{ ItemImagePath}calmthoughts.gif";
            ItemInfo.Desc = "Peaceful, mundane.";
            ItemInfo.ButtonContent = "Remember";
        }
        public void GetInfoSentimentalClutter()
        {
            ItemInfo.Name = "Sentimental Clutter";
            ItemInfo.ImageName = $"{ ItemImagePath}sentimentalclutter.gif";
            ItemInfo.Desc = "Sweet, mawkish.";
            ItemInfo.ButtonContent = "Remember";
        }
        public void GetInfoTherianthropics()
        {
            ItemInfo.Name = "Therianthropics";
            ItemInfo.ImageName = $"{ ItemImagePath}therianthropics.gif";
            ItemInfo.Desc = "Psychic extract of a feral spiritual connection.";
            ItemInfo.ButtonContent = "Remember";
        }
        public void GetInfoRibcage()
        {
            ItemInfo.Name = "Ribcage";
            ItemInfo.ImageName = $"{ ItemImagePath}ribcage.gif";
            ItemInfo.Desc = "Humane bone.";
            ItemInfo.ButtonContent = "Remember";
        }
        public void GetInfoGuidingSpirit()
        {
            ItemInfo.Name = "Guiding Spirit";
            ItemInfo.ImageName = $"{ItemImagePath}guidingspirit.gif";
            ItemInfo.Desc = "A corporal tether, mangled from soul and meat.";
        }

        public void GetInfoShamanClue()
        {
            if (!CurrentPlayer.Clues.First(clue => clue.ID==1).IsCompleted)
            {
                ItemInfo.Name = "Therianthrope";
                ItemInfo.ImageName = $"{ItemImagePath}shamanclue.gif";
                ItemInfo.Desc = "The nomad described a therianthrope, a useful guide for spiritual ritual.";
            }
            else
            {
                ItemInfo.Name = "Therianthrope";
                ItemInfo.ImageName = $"{ItemImagePath}shamanclue.gif";
                ItemInfo.Desc = "The materials harvested from the shaman were used and the guide is complete.";
            }

        }


        public void CheckLevelUp()
        {
            if (CurrentPlayer.Observations >= 8 && !Seer)
            {
                RaiseMessage("You know more about prediction and natural law. You better understand the human brain and learn new ways to percieve.\n\n++WISDOM++\n");
                RaiseMessage(TextBreak);
                Seer = true;
                if (CurrentPlayer.Health >= 50)
                {
                    CurrentPlayer.Health += 5;
                }
                else { CurrentPlayer.Health += 10; }
            }
        }
    }
}
