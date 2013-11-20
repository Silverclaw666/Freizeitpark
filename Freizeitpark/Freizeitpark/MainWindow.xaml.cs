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
        private System.Timers.Timer timecycle = new System.Timers.Timer(60000);
        private System.Timers.Timer watch = new System.Timers.Timer(1000);
        private System.Timers.Timer incoming = new System.Timers.Timer();
        private AutoResetEvent drehkreuz = new AutoResetEvent(true);
        public Achterbahn achterbahn = new Achterbahn();
        public Tagada tagada = new Tagada();
        public Restaurant restaurant = new Restaurant();
        public int earnedMoney = 0;
        public int startvisitors;
        public int tage = 0;
        public int second = 0;
        public int minute = 0;
        public int hour = 0;
        public NameGenerator ng = new NameGenerator();
        public Thread t2;
        public bool ParkOpen = true;
        public bool ParkClosing = false;
        public bool ParkReady = false;
        public Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            lb_personen.DataContext = besucher;
            timecycle.Elapsed += changeTime;
            watch.Elapsed += showTime;
            incoming.Elapsed += incomingVisitor;
            achterbahn_expander.DataContext = achterbahn;
            tagada_expander.DataContext = tagada;
            restaurant_expander.DataContext = restaurant;
            lb_cycle.DataContext = this;
        }

        private void incomingVisitor(object sender, ElapsedEventArgs e)
        {
            int incVisitors = rand.Next(1, 10);
            while (incVisitors + besucher.Count > startvisitors) {
                incVisitors = rand.Next(1, 10);
            }
            for (int i = 0; i < incVisitors; i++) {
                Visitor v = new Visitor(ng.GenName(), "kommt an", rand.Next(30, 150),this);
                this.Dispatcher.BeginInvoke(new Action (() =>besucher.Add(v)));
                drehkreuz.WaitOne();
                v.Status = "geht durch Drehkreuz";
                Thread.Sleep(50);
                drehkreuz.Set();
                v.StartThread();
            }
            incoming.Interval = rand.Next(5000, 15000);
        }

        private void showTime(object sender, ElapsedEventArgs e)
        {
            if (second == 59)
            {
                second = 0;
                if (minute == 59)
                {
                    hour++;
                    minute = 0;
                }
                else
                {
                    minute++;
                }
            }
            else {
                second++;
            }
            if (hour == 0 && minute == 0) {
                    this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = second + ""));     
            }
            else if (minute < 10 && hour == 0)
            {
                if (second < 10)
                {
                    this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = "0" + minute + " : " + "0" + second + ""));
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = "0" + minute + " : " + second + ""));
                }
            }
            else if(minute > 10){
                    if (second < 10)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = hour + " : " + minute + " : " + "0" + second + ""));
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = hour + " : " + minute + " : " + second + ""));
                    }
                }  
            else {
                if (minute < 10)
                {
                    if (second < 10)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = hour + " : " + "0" + minute + " : " + "0" + second + ""));
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = hour + " : " + "0" + minute + " : " + second + ""));
                    }
                }
                else {
                    if (second < 10)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = hour + " : " + minute + " : " + "0" + second + ""));
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(new Action(() => lb_time.Content = hour + " : " + minute + " : " + second + ""));
                    }
                }
            }
        }

        private void changeTime(object sender, ElapsedEventArgs e)
        {
            
            if (ParkOpen)
            {
                incoming.Stop();
                this.Dispatcher.BeginInvoke(new Action(() => this.Background = Brushes.Black));
                ParkOpen = false;
                ParkClosing = true;
                Thread.Sleep(3000);
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
                this.Dispatcher.BeginInvoke(new Action (() => this.Background = Brushes.White));
                tage++;
                this.Dispatcher.BeginInvoke(new Action(() => lb_tage.Content = "Tage: " + tage));
                ParkOpen = true;
                ParkClosing = false;
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Park ist geöffnet!"));
                this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Background = Brushes.LawnGreen));
                restaurant.Gericht = ng.GenGericht();
                fillVisitor();
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
            ParkReady = true;
            incoming.Interval = rand.Next(5000, 15000);
            incoming.Start();
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
            for (int i = 0; i < 35; i++)
            {
                this.Dispatcher.BeginInvoke(new Action(() => besucher.Add(new Visitor(ng.GenName(), "kommt an", rand.Next(30, 150), this))));
                Thread.Sleep(20);
                this.Dispatcher.BeginInvoke(new Action(() => lb_besucher.Content = "Besucher im Park: " + besucher.Count));
            }
            this.Dispatcher.BeginInvoke(new Action(() => lb_cycle.Content = "Der Park ist geöffnet!"));
            VisitorDrehkreuz();


        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            Start_Button.IsEnabled = false;
            timecycle.Start();
            watch.Start();
            startvisitors = 50;
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
