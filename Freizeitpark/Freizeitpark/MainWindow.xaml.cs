using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;

namespace Freizeitpark
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,IEnumerable
    {
        public ObservableCollection<Visitor> besucher = new ObservableCollection<Visitor>();
        private AutoResetEvent drehkreuz = new AutoResetEvent(true);
        System.Timers.Timer timecycle = new System.Timers.Timer(25000);
        System.Timers.Timer countdown = new System.Timers.Timer(1000);
        int count = 0;
        int earnedMoney = 0;
        NameGenerator ng = new NameGenerator();
        public bool ParkOpen = true;
        Random rand = new Random();

        public MainWindow() {
            InitializeComponent();
            lb_personen.DataContext = besucher;
            timecycle.Elapsed += changeTime;
            countdown.Elapsed += countup;
        }

        private void countup(object sender, ElapsedEventArgs e)
        {
            count += 1;
            this.Dispatcher.BeginInvoke(new Action(() => tb_time.Text = count + ""));
        }


        public void changeTime(object sender, ElapsedEventArgs e)
        {
            count = 0;
            if (ParkOpen)
            {
                ParkOpen = false;
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Park wird geschlossen!"));
                foreach (Visitor v in besucher) {
                    earnedMoney += v.Geld;
                    v.Status = "geht nach Hause";
                }
                
                while (besucher.Count != 1)
                {
                    Thread.Sleep(50);
                    this.Dispatcher.BeginInvoke(new Action(() => besucher.First()._thread.Abort()));
                    this.Dispatcher.BeginInvoke(new Action(() => besucher.Remove(besucher.First())));
                    
                }
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Park ist geschlossen!"));
                this.Dispatcher.BeginInvoke(new Action(() => lb_money.Content = "Geld: " + earnedMoney));
            }
            else
            {

                ParkOpen = true;
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Park ist geöffnet!"));
                for (int i = 0; i < 20; i++)
                {
                    this.Dispatcher.BeginInvoke(new Action(() => besucher.Add(new Visitor(ng.GenName(), "wartet in Schlange", rand.Next(30, 150)))));
                    Thread.Sleep(100);
                }
                foreach (Visitor v in besucher) {
                    drehkreuz.WaitOne();
                    v.Status = "geht durch Drehkreuz";
                    Thread.Sleep(1000);
                    drehkreuz.Set();
                    v.StartThread();
                }
            }
        }


        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 20; i++) {
                besucher.Add(new Visitor(ng.GenName(),"tritt ein",rand.Next(30,150)));
                besucher[i].StartThread();
            }
            this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Der Park ist geöffnet!"));
            timecycle.Start();
            countdown.Start();
        }

    }
}
