using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace pot_sem2
{
    /// <summary>
    /// Interaction logic for ReplayWidow.xaml
    /// </summary>
    public partial class ReplayWindow : Window
    {
        private PlayedGame playedGame;
        private Thread thread;

        public ReplayWindow(PlayedGame playedGame)
        {
            InitializeComponent();
            this.playedGame = playedGame;
            thread = new Thread(RunThread);
        }

        public void StartReplay()
        {
            thread.Start();
        }

        public void RunThread()
        {
            foreach (GameState state in playedGame.Replay)
            {
                Visualiser.Dispatcher.Invoke(() => {
                    Visualiser.SetState(state);
                });
                Thread.Sleep(1000);
            }
        }
    }
}
