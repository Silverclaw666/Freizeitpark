using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Freizeitpark
{
    public class Visitor : INotifyPropertyChanged
    {
        private Random rand = new Random();
        public Thread _thread;
        public MainWindow mwindow;
        public String name;
        public String status;
        public String currency;
        public int geld;
        public int i;

        public Thread thread {
            get
            {
                return _thread;
            }
        }

        public String Name {
            get { return name; }
            set {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public String Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        public int Geld
        {
            get { return geld; }
            set
            {
                geld = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Geld"));
            }
        }

        public String Currency {
            get { return currency; }
            set {
                currency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currency"));
            }
        }

        public Visitor(String name, String status, int geld ,MainWindow window,String currency = "€") {
            Name = name;
            Status = status;
            Geld = geld;
            Currency = currency;
            mwindow = window;
        }

        public void StartThread() {
            ThreadStart start = new ThreadStart(Start);
            _thread = new Thread(start);
            _thread.Start();
        }

        public void goHome() {
            Status = "geht zum Ausgang";
            Thread.Sleep(2000);
            if (mwindow.besucher.Count == 1) {
                mwindow.Dispatcher.BeginInvoke(new Action(() => this._thread.Abort()));
                mwindow.Dispatcher.BeginInvoke(new Action(() => mwindow.besucher.Remove(mwindow.besucher.First())));
            }
            mwindow.Dispatcher.BeginInvoke(new Action(() => this._thread.Abort()));
            mwindow.Dispatcher.BeginInvoke(new Action(() => mwindow.besucher.Remove(this)));
            mwindow.Dispatcher.BeginInvoke(new Action(() => mwindow.lb_besucher.Content = "Besucher im Park: " + mwindow.besucher.Count));
        }

        public void Start()
        {
            while (true)
            {
                Status = "wandert";
                Thread.Sleep(rand.Next(1000, 5000));
                if (this.Geld == 0) {
                    goHome();
                }
                this.i = rand.Next(0, 5);
                Thread.Sleep(100);
                switch (this.i)
                {
                    case (0):
                        if (this.Geld >= 8)
                        {
                            this.Status = "fährt mit Achterbahn";
                            this.Geld -= 8;
                            mwindow.earnedMoney += 8;
                            mwindow.achterbahn.Money += 8;
                            mwindow.achterbahn.Passagiere += 1;
                            mwindow.Dispatcher.BeginInvoke(new Action(() => mwindow.lb_money.Content = "Geld: " + mwindow.earnedMoney));
                            Thread.Sleep(8000);
                        }
                        break;
                    case (1):
                        this.Status = "macht eine Pause";
                        Thread.Sleep(5000);
                        break;
                    case (2):
                        if (this.Geld >= 15)
                        {
                            this.Status = "isst etwas";
                            this.Geld -= 15;
                            mwindow.earnedMoney += 15;
                            mwindow.restaurant.Money += 15;
                            mwindow.restaurant.Besucher += 1;
                            mwindow.Dispatcher.BeginInvoke(new Action(() => mwindow.lb_money.Content = "Geld: " + mwindow.earnedMoney));
                            Thread.Sleep(10000);
                        }
                        break;
                    case (3):
                        if (this.Geld >= 5)
                        {
                            this.Status = "fährt mit Tagada";
                            this.Geld -= 5;
                            mwindow.earnedMoney += 5;
                            mwindow.tagada.Money += 5;
                            mwindow.tagada.Passagiere += 1;
                            mwindow.Dispatcher.BeginInvoke(new Action(() => mwindow.lb_money.Content = "Geld: " + mwindow.earnedMoney));
                            Thread.Sleep(6000);
                        }
                        break;
                    case (4):
                        {
                            goHome();
                            break;
                        }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)

                PropertyChanged(this, e);

        }
    }
}
