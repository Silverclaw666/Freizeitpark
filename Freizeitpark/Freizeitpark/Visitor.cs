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
        public Thread _thread;
        public String name;
        public String status;
        public String currency;
        public int geld;

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

        public Visitor(String name, String status, int geld , String currency = "€") {
            Name = name;
            Status = status;
            Geld = geld;
            Currency = currency;
        }

        public void StartThread() {
            ThreadStart start = new ThreadStart(Start);
            _thread = new Thread(start);
            _thread.Start();
        }

        private void Start()
        {
            this.Status = "wandert";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
            }
    }
}
