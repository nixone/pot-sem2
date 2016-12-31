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
    /// Interaction logic for replay window
    /// </summary>
    public partial class ReplayWindow : Window
    {
        private PlayedGame playedGame;
        private Thread thread;

        /// <summary>
        /// Creates the window with a specific played game record
        /// </summary>
        /// <param name="playedGame"></param>
        public ReplayWindow(PlayedGame playedGame)
        {
            InitializeComponent();
            this.playedGame = playedGame;
            thread = new Thread(RunThread);
            thread.IsBackground = true;
        }

        /// <summary>
        /// Asynchronously starts the replay
        /// </summary>
        public void StartReplay()
        {
            thread.Start();
        }

        public void RunThread()
        {
            foreach (GameState state in playedGame.GetReplay())
            {
                Visualiser.Dispatcher.Invoke(() => {
                    Visualiser.SetState(state);
                });
                Thread.Sleep(1000);
            }
        }
    }
}
