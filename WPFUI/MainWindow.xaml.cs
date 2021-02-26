using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Engine.Controllers;
using Engine.EventArgs;



namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private readonly GameSession gameSession = new GameSession();
        public MainWindow()
        {
            InitializeComponent();
            gameSession.OnMessageRaised += OnGameMessageRaise;
            DataContext = gameSession;

            
        }
        //MOVEMENT
        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            gameSession.MoveNorth();

        }
        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            gameSession.MoveWest();
        }
        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            gameSession.MoveEast();
        }
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            gameSession.MoveSouth();
        }

        private void ContextualActions(object sender, RoutedEventArgs e)
        {
            gameSession.ContextualAction();

            if (gameSession.IsFighting) { gameSession.ShowInfo(); }
        }


        //GAMEMESSAGE
        private void OnGameMessageRaise(object sender, GameMessageEventArgs e)
        {
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd();
           
        }

        //COMBAT
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(); 
        private async void OnClick_Attack(object sender, RoutedEventArgs e)
        {
            timer.Interval = new TimeSpan(0, 0, 0,0,500);
            timer.Tick += timer_Tick;
            timer.Start();
            Button1lol.IsEnabled = false;


            gameSession.Attack();
           
            await Task.Delay(500);

            gameSession.MonsterAttack();
        }
        void timer_Tick(object sender, System.EventArgs e)
        {
            Button1lol.IsEnabled = true;
            timer.Stop();
        }

        private void OnClick_No(object sender, RoutedEventArgs e)
        {
            gameSession.ClickRight();


            gameSession.ShowInfo();
        }
        private void OnClick_Yes(object sender, RoutedEventArgs e)
        {
            gameSession.ClickLeft();

            
            gameSession.ShowInfo();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private async void OnClick_InteractWithItem(object sender, RoutedEventArgs e)
        {
            gameSession.InteractWithItem();

            if (!gameSession.ItemButtonSwitched)
            {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                timer.Tick += timer_TickInspect;
                timer.Start();
                InspectButton.IsEnabled = false;
            }
            gameSession.ItemButtonSwitched = false;

            await Task.Delay(1000);

            gameSession.CheckLevelUp();
        }
        void timer_TickInspect(object sender, System.EventArgs e)
        {
            InspectButton.IsEnabled = true;
            timer.Stop();
        }
        private void OnClick_Info(object sender, RoutedEventArgs e)
        {
            var CurrentName = gameSession.ItemInfo.Name;
            dynamic row = ((Button)sender).DataContext;
            var selectedItem = row.ID;
            var NewName = row.Name;
            
            if (NewName == CurrentName && gameSession.ItemClickedYes==true)
            {
                gameSession.ItemClickedYes = false;
                gameSession.ItemInfo.ItemIsInteractable = false;
                gameSession.ShowInfo();
            }
            else
            {          
                if (selectedItem == 2001)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = false;                  
                    gameSession.GetInfoRing();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 2002)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = true;
                    gameSession.GetInfoTenderMemory();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 2003)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = true;
                    gameSession.GetInfoSentimentalClutter();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 2007)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = true;
                    gameSession.GetInfoCalmThoughts();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 2004)
                {
           
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = true;
                    gameSession.GetInfoVialOfBlood();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 2006)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = true;
                    gameSession.GetInfoGlassVial();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 9001)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = false;
                    gameSession.GetInfoOccipital();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 9002)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = false;
                    gameSession.GetInfoRibcage();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 9003)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = false;
                    gameSession.GetInfoTherianthropics();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 1001)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = false;
                    gameSession.GetInfoMattock();
                    gameSession.ShowInfo();
                }
                else if(selectedItem == 6001)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = false;
                    gameSession.GetInfoGuidingSpirit();
                    gameSession.ShowInfo();
                }
                else if (selectedItem == 1)
                {
                    gameSession.ItemClickedYes = true;
                    gameSession.ItemInfo.ItemIsInteractable = false;
                    gameSession.GetInfoShamanClue();
                    gameSession.ShowInfo();
                }

                if (gameSession.IsFighting)
                {
                    gameSession.ItemInfo.ButtonContent = "...";
                    gameSession.ShowInfo();
                }
                
            }
          
          


        }
    }
}
