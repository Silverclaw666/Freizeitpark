using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Freizeitpark
{
    public class Tagada : INotifyPropertyChanged
    {
        public AutoResetEvent drehkreuz = new AutoResetEvent(true);
        public AutoResetEvent queue = new AutoResetEvent(false);
        public CountdownEvent einsteigen;
        public CountdownEvent aussteigen;
        public int fahrten = 0;
        public int passagiere;
        public int tagada_money;
        public String status;

        public String Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        public int Fahrten
        {
            get { return fahrten; }
            set
            {
                fahrten = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Fahrten"));
            }
        }

        public int Passagiere
        {
            get { return passagiere; }
            set
            {
                passagiere = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Passagiere"));
            }
        }

        public int Money
        {
            get { return tagada_money; }
            set
            {
                tagada_money = value;
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
