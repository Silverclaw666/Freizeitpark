using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace Freizeitpark
{
    public class Restaurant : INotifyPropertyChanged
    {
        public int restaurant_money;
        public int besucher;
        public String gericht;

        public String Gericht {
            get { return gericht; }
            set {
                gericht = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Gericht"));
            }
        }

        public int Besucher {
            get { return besucher; }
            set {
                besucher = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Besucher"));
            }
        }

        public int Money
        {
            get { return restaurant_money; }
            set
            {
                restaurant_money = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Money"));
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
