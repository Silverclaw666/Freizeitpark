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
        String bname;
        String mname = "David Mark Patrick Stefan Anton Maik Niklas Marco Hans Carl Mike Axel Stefan Sven Wolfgang Patrick";
        String wname = "Bettina Elisabeth Ulrike Alexandra Nadine Esther Daniela Nadja Nicole Annika Nathalie Claudia Nicole Linda Michaela";
        String[] mnames;
        String[] wnames;

        public NameGenerator() {
            mnames = mname.Split(' ');
            wnames = wname.Split(' ');
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
    }
}
