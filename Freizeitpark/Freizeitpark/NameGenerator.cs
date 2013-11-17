using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freizeitpark
{
    public class NameGenerator
    {
        Random rand = new Random();
        String mname = "David Mark Patrick Stefan Anton Maik Niklas Marco Hans Carl Mike Axel Stefan Sven Wolfgang Patrick";
        String wname = "Bettina Elisabeth Ulrike Alexandra Nadine Esther Daniela Nadja Nicole Annika Nathalie Claudia Nicole Linda Michaela";
        String gname = "Schnitzel,Brathuhn,Schweinsbraten,Grillplatte,Käsespätzle,Gulasch";
        String[] mnames;
        String[] wnames;
        String[] gnames;

        public NameGenerator() {
            mnames = mname.Split(' ');
            wnames = wname.Split(' ');
            gnames = gname.Split(',');
        }
        
        public String GenName() {
            if (rand.Next(0, 2) == 0)
            {
                return mnames[rand.Next(mnames.Length)];
            }
            else {
                return wnames[rand.Next(wnames.Length)];
            }
            
        }

        public String GenGericht() {
            return gnames[rand.Next(gnames.Length)];
        }
    }
}
