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
    public partial class MainWindow : Window, IEnumerable
    {
        public ObservableCollection<Visitor> besucher = new ObservableCollection<Visitor>();
        private System.Timers.Timer timecycle = new System.Timers.Timer(30000);
        private AutoResetEvent drehkreuz = new AutoResetEvent(true);
        public Achterbahn achterbahn = new Achterbahn();
        public Tagada tagada = new Tagada();
        public Restaurant restaurant = new Restaurant();
        public int earnedMoney = 0;
        public int startvisitors;
        public NameGenerator ng = new NameGenerator();
        public bool ParkOpen = true;
        public Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            lb_personen.DataContext = besucher;
            timecycle.Elapsed += changeTime;
            achterbahn_expander.DataContext = achterbahn;
            tagada_expander.DataContext = tagada;
            restaurant_expander.DataContext = restaurant;
            lb_cycle.DataContext = this;
        }


        private void changeTime(object sender, ElapsedEventArgs e)
        {
            if (ParkOpen)
            {
                ParkOpen = false;
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Park wird geschlossen!"));
                abortVisitor();
                foreach (Visitor v in besucher)
                {
                    v.Status = "geht nach Hause";
                }

                while (besucher.Count != 1)
                {
                    Thread.Sleep(50);
                    this.Dispatcher.BeginInvoke(new Action(() => besucher.First()._thread.Abort()));
                    this.Dispatcher.BeginInvoke(new Action(() => besucher.Remove(besucher.First())));
                    this.Dispatcher.BeginInvoke(new Action(() => lb_besucher.Content = "Besucher im Park: " + besucher.Count));

                }
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Park ist geschlossen!"));
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Background = Brushes.Red));
            }
            else
            {
                ParkOpen = true;
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Park ist geöffnet!"));
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Background = Brushes.LawnGreen));
                restaurant.Gericht = ng.GenGericht();
                for (int i = 0; i < startvisitors; i++)
                {
                    this.Dispatcher.BeginInvoke(new Action(() => besucher.Add(new Visitor(ng.GenName(), "wartet in Schlange", rand.Next(30, 150), this))));
                    Thread.Sleep(100);
                }
                VisitorDrehkreuz();
            }
        }

        private void VisitorDrehkreuz()
        {
            foreach (Visitor v in besucher)
            {
                drehkreuz.WaitOne();
                v.Status = "geht durch Drehkreuz";
                Thread.Sleep(20);
                drehkreuz.Set();
                v.StartThread();
            }
        }

        private void abortVisitor()
        {
            foreach (Visitor v in besucher)
            {
                v.thread.Abort();
            }
        }

        private void fillVisitor()
        {
            for (int i = 0; i < startvisitors; i++)
            {
                this.Dispatcher.BeginInvoke(new Action(() => besucher.Add(new Visitor(ng.GenName(), "kommt an", rand.Next(30, 150), this))));
                Thread.Sleep(20);
                this.Dispatcher.BeginInvoke(new Action(() => lb_besucher.Content = "Besucher im Park: " + besucher.Count));
            }
            this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Der Park ist geöffnet!"));
            timecycle.Start();
            VisitorDrehkreuz();

        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            Start_Button.IsEnabled = false;
            startvisitors = 20;
            achterbahn.Status = "wartet";
            restaurant.Gericht = ng.GenGericht();
            this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Background = Brushes.LawnGreen));
            ThreadStart start = new ThreadStart(fillVisitor);
            Thread t = new Thread(start);
            t.Start();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            abortVisitor();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }
}
