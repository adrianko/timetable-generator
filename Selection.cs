using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Timetable_v2
{
    public class Selection
    {
        //constructor takes 11 arguements, students first name, last name, id and the 8 modules
        public Selection(String fName, String lName, String iD, String mod1, String mod2, String mod3, String mod4, String mod5, String mod6, String mod7, String mod8)
        {
            this.FName = fName;
            this.LName = lName;
            this.ID = iD;
            this.Mod1 = mod1;
            this.Mod2 = mod2;
            this.Mod3 = mod3;
            this.Mod4 = mod4;
            this.Mod5 = mod5;
            this.Mod6 = mod6;
            this.Mod7 = mod7;
            this.Mod8 = mod8;
        }
        String fName;
        public String FName
        {
            get { return fName; }
            set { fName = value; }
        }
        String lName;
        public String LName
        {
            get { return lName; }
            set { lName = value; }
        }
        String iD;

        public String ID
        {
            get { return iD; }
            set { iD = value; }
        }

        String mod1;

        public String Mod1
        {
            get { return mod1; }
            set { mod1 = value; }
        }

        String mod2;

        public String Mod2
        {
            get { return mod2; }
            set { mod2 = value; }
        }

        String mod3;

        public String Mod3
        {
            get { return mod3; }
            set { mod3 = value; }
        }

        String mod4;

        public String Mod4
        {
            get { return mod4; }
            set { mod4 = value; }
        }

        String mod5;

        public String Mod5
        {
            get { return mod5; }
            set { mod5 = value; }
        }

        String mod6;

        public String Mod6
        {
            get { return mod6; }
            set { mod6 = value; }
        }

        String mod7;

        public String Mod7
        {
            get { return mod7; }
            set { mod7 = value; }
        }

        String mod8;

        public String Mod8
        {
            get { return mod8; }
            set { mod8 = value; }
        }
    }
}
