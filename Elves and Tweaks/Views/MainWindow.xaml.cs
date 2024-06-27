using Elves_and_Tweaks.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Elves_and_Tweaks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Window rulesWindow;
        private ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        private const string defaultName = "Player";
        private int counter = 1;
        private const int defaultMana = 0;
        private const int defaultTokens = 0;
        private const int defaultModifiers = 0;

        private TimeOnly timePassed = new TimeOnly();
        public TimeOnly TimePassed
        {
            get => this.timePassed;
            set
            {
                this.timePassed = value;
                this.TimeString = value.Hour.ToString() + ":" + value.Minute.ToString() + ":" + value.Second.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(TimePassed)));
                }
            }
        }

        private string timeString;
        public string TimeString
        { 
            get => this.timeString;
            set
            {
                this.timeString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(TimeString)));
                }
            }
        }

        private int? turn;
        public int? Turn
        {
            get => this.turn;
            set
            {
                if (this.Turn <= 0 && value < this.Turn)
                {
                    return;
                }
                this.turn = value;
                this.TurnString = "Current Turn: " + this.turn.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Turn)));
                }
            }
        }

        private string? turnString;
        public string? TurnString
        {
            get => this.turnString;
            set
            {
                this.turnString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(TurnString)));
                }
            }
        }

        private int diceRoll;
        public int DiceRoll
        {
            get => this.diceRoll;
            set
            {
                this.diceRoll = value;
                this.DiceRollString = "Dice Roll: " + this.diceRoll.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DiceRoll)));
                }
            }
        }

        private string? diceRollString;
        public string? DiceRollString
        {
            get => this.diceRollString;
            set
            {
                this.diceRollString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DiceRollString)));
                }
            }
        }

        private int chanceRoll;
        public int ChanceRoll
        {
            get => this.chanceRoll;
            set
            {
                this.chanceRoll = value;
                this.ChanceRollString = "Chance Roll: " + this.chanceRoll.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ChanceRoll)));
                }
            }
        }

        private string? chanceRollString;
        public string? ChanceRollString
        {
            get => this.chanceRollString;
            set
            {
                this.chanceRollString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ChanceRollString)));
                }
            }
        }

        private int encounterRoll;
        public int EncounterRoll
        {
            get => this.encounterRoll;
            set
            {
                this.encounterRoll = value;
                this.EncounterRollString = "Encounter Roll: " + this.encounterRoll.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(EncounterRoll)));
                }
            }
        }

        private string? encounterRollString;
        public string? EncounterRollString
        {
            get => this.encounterRollString;
            set
            {
                this.encounterRollString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(EncounterRollString)));
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Turn = 0;
            this.DiceRoll = 0;
            this.ChanceRoll = 0;
            this.EncounterRoll = 0;

            Task.Run(() =>
            {
                this.TimePassed = new TimeOnly();
                var oldTime = DateTime.Now;
                while (true)
                {
                    if (oldTime.Second < DateTime.Now.Second)
                    {
                        oldTime = DateTime.Now;
                        var timeSpan = TimeSpan.FromSeconds(1);
                        this.TimePassed = this.TimePassed.Add(timeSpan);
                    }
                }
            });
        }

        private void AddPlayerClick(object sender, RoutedEventArgs e)
        {
            if (Players.Count < 4)
            {
                var newPlayer = new Player(defaultName + counter++.ToString(), defaultMana, defaultTokens, defaultModifiers);
                Players.Add(newPlayer);

                var playerPanel = new StackPanel() { Margin = new Thickness(3, 5, 3, 5) };
                var playerNumberBox = new TextBox() { Text = $"{Players.Count}", Background = Brushes.Transparent, IsEnabled = false };
                var playerNameBox = new TextBox() { BorderThickness = new Thickness(0) };
                var playerManaPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                var playerTokenPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                var playerModifierPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                playerNameBox.TextChanged += NameBoxChanged;

                var playerManaBox = new TextBox() { BorderThickness = new Thickness(0), IsEnabled = false, FontSize = 14 };
                var playerManaAddition = new Button() { Content = "+", Width = 14, Height = 20, BorderThickness = new Thickness(0), Margin = new Thickness(1) };
                var playerManaSubtraction = new Button() { Content = "-", Width = 14, Height = 20, BorderThickness = new Thickness(0), Margin = new Thickness(1) };
                playerManaPanel.Children.Add(playerManaSubtraction);
                playerManaPanel.Children.Add(playerManaBox);
                playerManaPanel.Children.Add(playerManaAddition);

                playerManaAddition.Click += new RoutedEventHandler(ManaAdditionClick);
                playerManaSubtraction.Click += new RoutedEventHandler(ManaSubtractionClick);

                var playerTokenBox = new TextBox() { BorderThickness = new Thickness(0), IsEnabled = false, FontSize = 14 };
                var playerTokenAddition = new Button() { Content = "+", Width = 14, Height = 20, BorderThickness = new Thickness(0), Margin = new Thickness(1) };
                var playerTokenSubtraction = new Button() { Content = "-", Width = 14, Height = 20, BorderThickness = new Thickness(0), Margin = new Thickness(1) };

                playerTokenAddition.Click += new RoutedEventHandler(TokenAdditionClick);
                playerTokenSubtraction.Click += new RoutedEventHandler(TokenSubtractionClick);

                playerManaPanel.Children.Add(playerTokenSubtraction);
                playerManaPanel.Children.Add(playerTokenBox);
                playerManaPanel.Children.Add(playerTokenAddition);

                var playerModifierBox = new TextBox() { BorderThickness = new Thickness(0), IsEnabled = false, FontSize = 14 };
                var playerModifierAddition = new Button() { Content = "+", Width = 14, Height = 20, BorderThickness = new Thickness(0), Margin = new Thickness(1) };
                var playerModifierSubtraction = new Button() { Content = "-", Width = 14, Height = 20, BorderThickness = new Thickness(0), Margin = new Thickness(1) };

                playerModifierAddition.Click += new RoutedEventHandler(ModifierAdditionClick);
                playerModifierSubtraction.Click += new RoutedEventHandler(ModifierSubtractionClick);

                playerModifierPanel.Children.Add(playerModifierSubtraction);
                playerModifierPanel.Children.Add(playerModifierBox);
                playerModifierPanel.Children.Add(playerModifierAddition);
                playerPanel.Children.Add(playerNumberBox);

                Binding nameBinding = new Binding("Name");
                Binding manaBinding = new Binding("ManaString");
                Binding tokenBinding = new Binding("TokensString");
                Binding modifierBinding = new Binding("ModifiersString");

                nameBinding.Source = newPlayer;
                manaBinding.Source = newPlayer;
                tokenBinding.Source = newPlayer;
                modifierBinding.Source = newPlayer;

                playerNameBox.SetBinding(TextBox.TextProperty, nameBinding);
                playerManaBox.SetBinding(TextBox.TextProperty, manaBinding);
                playerTokenBox.SetBinding(TextBox.TextProperty, tokenBinding);
                playerModifierBox.SetBinding(TextBox.TextProperty, modifierBinding);

                var deleteButton = new Button() { Content = "Delete Player" };
                deleteButton.Click += DeletePlayerClick;

                playerPanel.Children.Add(playerNameBox);
                playerPanel.Children.Add(playerManaPanel);
                playerPanel.Children.Add(playerTokenPanel);
                playerPanel.Children.Add(playerModifierPanel);
                playerPanel.Children.Add(deleteButton);

                this.PlayersList.Children.Add(playerPanel);

                if (Players.Count >= 4)
                {
                    Button button = sender as Button;
                    button.IsEnabled = false;
                }
            }
            else
            {
                Button button = sender as Button;
                button.IsEnabled = false;
            }
        }

        private void NameBoxChanged(object sender, TextChangedEventArgs args)
        {
            var nameTextBox = sender as TextBox;
            if (nameTextBox != null && nameTextBox.Parent != null)
            {
                var playerPanel = nameTextBox.Parent as StackPanel;

                foreach (var el in playerPanel.Children)
                {
                    if ((el as TextBox).Text == "1" || (el as TextBox).Text == "2" || (el as TextBox).Text == "3" || (el as TextBox).Text == "4" && (el as TextBox).IsEnabled == false)
                    {
                        var playerId = (el as TextBox).Text;
                        var searchedPlayer = Players[int.Parse(playerId) - 1];
                        if (searchedPlayer != null)
                        {
                            searchedPlayer.Name = (sender as TextBox).Text;
                            return;
                        }
                    }
                }
            }
        }

        private void ManaAdditionClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            StackPanel manaPanel = button.Parent as StackPanel;
            StackPanel playerPanel = manaPanel.Parent as StackPanel;
            foreach (var item in playerPanel.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    if ((item as TextBox).IsEnabled == true)
                    {
                        var playerNameBox = item as TextBox;
                        var playerName = playerNameBox.Text;
                        var searchedPlayer = Players.FirstOrDefault((player) => player.Name == playerName);
                        searchedPlayer.Mana++;
                    }
                }
            }
        }

        private void ManaSubtractionClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            StackPanel manaPanel = button.Parent as StackPanel;
            StackPanel playerPanel = manaPanel.Parent as StackPanel;
            foreach (var item in playerPanel.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    if ((item as TextBox).IsEnabled == true)
                    {
                        var playerNameBox = item as TextBox;
                        var playerName = playerNameBox.Text;
                        var searchedPlayer = Players.FirstOrDefault((player) => player.Name == playerName);
                        if (searchedPlayer.Mana > 0)
                        {
                            searchedPlayer.Mana--;
                        }
                    }
                    
                }
            }
        }

        private void TokenAdditionClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            StackPanel burnPanel = button.Parent as StackPanel;
            StackPanel playerPanel = burnPanel.Parent as StackPanel;
            foreach (var item in playerPanel.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    if ((item as TextBox).IsEnabled == true)
                    {
                        var playerNameBox = item as TextBox;
                        var playerName = playerNameBox.Text;
                        var searchedPlayer = Players.FirstOrDefault((player) => player.Name == playerName);
                        searchedPlayer.Tokens++;
                    }
                }
            }
        }

        private void TokenSubtractionClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            StackPanel burnPanel = button.Parent as StackPanel;
            StackPanel playerPanel = burnPanel.Parent as StackPanel;
            foreach (var item in playerPanel.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    if ((item as TextBox).IsEnabled == true)
                    {
                        var playerNameBox = item as TextBox;
                        var playerName = playerNameBox.Text;
                        var searchedPlayer = Players.FirstOrDefault((player) => player.Name == playerName);
                        if (searchedPlayer.Tokens > 0)
                        {
                            searchedPlayer.Tokens--;
                        }
                    }
                }
            }
        }

        private void ModifierAdditionClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            StackPanel modifierPanel = button.Parent as StackPanel;
            StackPanel playerPanel = modifierPanel.Parent as StackPanel;
            foreach (var item in playerPanel.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    if ((item as TextBox).IsEnabled == true)
                    {
                        var playerNameBox = item as TextBox;
                        var playerName = playerNameBox.Text;
                        var searchedPlayer = Players.FirstOrDefault((player) => player.Name == playerName);
                        searchedPlayer.Modifiers++;
                    }
                }
            }
        }

        private void ModifierSubtractionClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            StackPanel modifierPanel = button.Parent as StackPanel;
            StackPanel playerPanel = modifierPanel.Parent as StackPanel;
            foreach (var item in playerPanel.Children)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    if ((item as TextBox).IsEnabled == true)
                    {
                        var playerNameBox = item as TextBox;
                        var playerName = playerNameBox.Text;
                        var searchedPlayer = Players.FirstOrDefault((player) => player.Name == playerName);
                        if (searchedPlayer.Modifiers > 0)
                        {
                            searchedPlayer.Modifiers--;
                        }
                    }
                }
            }
        }

        private void DeletePlayerClick(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                var playerPanel = deleteButton.Parent as StackPanel;
                var playersList = playerPanel.Parent as StackPanel;
                playersList.Children.Remove(playerPanel);

                foreach (var el in playerPanel.Children)
                {
                    if (el.GetType() == typeof(TextBox))
                    {
                        if ((el as TextBox).IsEnabled == true)
                        {
                            var playerNameBox = el as TextBox;
                            var playerName = playerNameBox.Text;
                            var searchedPlayer = Players.FirstOrDefault((player) => player.Name == playerName);
                            Players.Remove(searchedPlayer);
                        }
                    }
                }

                foreach (var el in playersList.Children)
                {
                    if ((el as Button)?.Content.ToString() == "Add Player")
                    {
                        var addPlayerButton = el as Button;
                        addPlayerButton.IsEnabled = true;
                    }
                }
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void EndTurnClick(object sender, RoutedEventArgs e)
        {
            this.Turn++;
        }

        private void PreviousTurnClick(object sender, RoutedEventArgs e)
        {
            this.Turn--;
        }

        private void ThrowDiceClick(object sender, RoutedEventArgs e)
        {
            this.DiceRoll = Random.Shared.Next(1, 7);
        }

        private void RulesClick(object sender, RoutedEventArgs e)
        {
            string rulesText = File.ReadAllText("Assets/Rules/Rules.txt");
            this.DisplayingPanel.Children.Clear();
            this.DisplayingPanel.Children.Add(new TextBlock() { Text = $"{rulesText}", Background = new SolidColorBrush(Colors.Wheat), TextWrapping = TextWrapping.Wrap, FontSize = 14, Padding = new Thickness(5), Margin = new Thickness(5) });
        }
        private void MapClick(object sender, RoutedEventArgs e)
        {
            string mapText = File.ReadAllText("Assets/Map/MapExplanation.txt");
            this.DisplayingPanel.Children.Clear();
            this.DisplayingPanel.Children.Add(new TextBlock() { Text = $"{mapText}", Background = new SolidColorBrush(Colors.Wheat), TextWrapping = TextWrapping.Wrap, FontSize = 14, Padding = new Thickness(5), Margin = new Thickness(5) });
        }
        private void LoreClick(object sender, RoutedEventArgs e)
        {
            string loreText = File.ReadAllText("Assets/Lore and Factions/Lore.txt");
            this.DisplayingPanel.Children.Clear();
            this.DisplayingPanel.Children.Add(new TextBlock() { Text = $"{loreText}", Background = new SolidColorBrush(Colors.Wheat), TextWrapping = TextWrapping.Wrap, FontSize = 14, Padding = new Thickness(5), Margin = new Thickness(5) });
        }
        private void FactionsClick(object sender, RoutedEventArgs e)
        {
            string factionsText = File.ReadAllText("Assets/Lore and Factions/Factions.txt");
            this.DisplayingPanel.Children.Clear();
            this.DisplayingPanel.Children.Add(new TextBlock() { Text = $"{factionsText}", Background = new SolidColorBrush(Colors.Wheat), TextWrapping = TextWrapping.Wrap, FontSize = 14, Padding = new Thickness(5), Margin = new Thickness(5) });
        }

        private void EventsClick(object sender, RoutedEventArgs e)
        {
            string eventsText = File.ReadAllText("Assets/Events/Events.txt");
            this.DisplayingPanel.Children.Clear();
            this.DisplayingPanel.Children.Add(new TextBlock() { Text = $"{eventsText}", Background = new SolidColorBrush(Colors.Wheat), TextWrapping = TextWrapping.Wrap, FontSize = 14, Padding = new Thickness(5), Margin = new Thickness(5) });
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            this.DisplayingPanel.Children.Clear();
        }

        private void ChanceClick(object sender, RoutedEventArgs e)
        {
            this.ChanceRoll = Random.Shared.Next(1, 41);
        }

        private void ChanceCardsClick(object sender, RoutedEventArgs e)
        {
            var numberOfCards = Directory.GetFiles("../../../Assets/Chance Cards").Length;
            this.DisplayingPanel.Children.Clear();
            for (int i = 1; i < numberOfCards + 1; i++)
            {
                var newCard = new Button() { Content = $"Chance Card {i}" };
                newCard.Click += CardClick;
                this.DisplayingPanel.Children.Add(newCard);
            }
        }

        private void CardClick(object sender, RoutedEventArgs e)
        {
            Button cardButton = sender as Button;
            const string imgsPath = "../../../Assets/Chance Cards";

            if (cardButton != null)
            {
                var chanceCards = Directory.GetFiles(imgsPath);
                var chanceCardPath = chanceCards.FirstOrDefault((path) => path.Substring(29).Replace('_', ' ') == cardButton.Content.ToString() + ".png");
                this.DisplayingPanel.Children.Clear();
                var cardImage = new Image() { Source = new BitmapImage(new Uri(chanceCardPath, UriKind.Relative)), Margin = new Thickness(5) };
                this.DisplayingPanel.Children.Add(cardImage);
            }
        }

        private void CreditsClick(object sender, RoutedEventArgs e)
        {
            string creditsText = File.ReadAllText("Assets/Credits/Credits.txt");
            this.DisplayingPanel.Children.Clear();
            this.DisplayingPanel.Children.Add(new TextBlock() { Text = $"{creditsText}", Background = new SolidColorBrush(Colors.Wheat), TextWrapping = TextWrapping.Wrap, FontSize = 14, Padding = new Thickness(5), Margin = new Thickness(5) });
        }
    }
}