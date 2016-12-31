using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace pot_sem2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameHost host;
        private GameClient client;

        public MainWindow()
        {
            InitializeComponent();
            Visualiser.OnTileSelected += (x, y) => 
            {
                if (client != null)
                {
                    client.Select(x, y);
                }
            };
        }

        private void GamesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(sender);
            ListViewItem item = sender as ListViewItem;
            PlayedGame playedGame = item.DataContext as PlayedGame;
            Console.WriteLine(playedGame);
            ReplayWindow window = new ReplayWindow(playedGame);
            window.Show();
            window.StartReplay();
        }

        private void ServerButtonClick(object sender, RoutedEventArgs e)
        {
            if (host == null)
            {
                host = new pot_sem2.GameHost("localhost", int.Parse(ServerPort.Text));
                host.OnStarted += () =>
                {
                    ServerStatus.Dispatcher.Invoke(() =>
                    {
                        ServerStatus.Text = "Running";
                    });
                };
                host.OnStopped += () =>
                {
                    ServerStatus.Dispatcher.Invoke(() =>
                    {
                        ServerStatus.Text = "Stopped";
                    });
                };
                host.Start();
            }
        }

        private void ClientButtonClick(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.Stop();
            }
            client = new GameClient(ClientName.Text, ClientAddress.Text, int.Parse(ClientPort.Text));
            client.OnConnected += () => {
                ClientStatus.Dispatcher.Invoke(() => {
                    ClientStatus.Text = "Connected";
                });
            };
            client.OnDisconnected += () => {
                ClientStatus.Dispatcher.Invoke(() => {
                    ClientStatus.Text = "Disconnected";
                });
            };
            client.OnNewState += (state, player, whiteName, blackName) => {
                Visualiser.Dispatcher.Invoke(() => {
                    Visualiser.SetState(state);
                    FinishTurnButton.IsEnabled = player != Player.NONE && state.PlayerOnTurn == player && !state.IsFinished();
                    StartNewGameButton.IsEnabled = state.IsFinished();
                    RecordButton.IsEnabled = state.IsFinished();
                    WhitePlayerName.Text = whiteName;
                    BlackPlayerName.Text = blackName;
                });
            };
            client.OnNewPlayedGames += (playedGames) => {
                GamesList.Dispatcher.Invoke(() => {
                    GamesList.ItemsSource = playedGames;
                });
            };
            client.Start();
        }

        private void RecordClick(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.Record();
            }
        }

        private void FinishTurnClick(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.FinishTurn();
            }
        }
    }
}
