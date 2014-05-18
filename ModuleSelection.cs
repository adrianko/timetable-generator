using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
//using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
//using System.Data.OleDb;
using System.Collections;

namespace Timetable_v2
{
    public partial class ModuleSelection : Form
    {
        //declare global variables for form
        SelectionSummary f2;
        public String workingDir = Directory.GetCurrentDirectory();
        public ArrayList modules = new ArrayList();
        bool flag = false;
        private Selection studentSelectedModules = null;
        bool loadFlag = false;
        int mCounter = 0;

        //function returns the relevant timeslot for a time enumeration 0-10
        public String retSlot(int timeEnum)
        {
            String slot = "";
            switch (timeEnum)
            {
                case 0:
                    slot = "Monday 9-11";
                    break;
                case 1:
                    slot = "Monday 11-1pm";
                    break;
                case 2:
                    slot = "Monday 2-4pm";
                    break;
                case 3:
                    slot = "Monday 4-6pm";
                    break;
                case 4:
                    slot = "Tuesday 9-11";
                    break;
                case 5:
                    slot = "Tuesday 11-1pm";
                    break;
                case 6:
                    slot = "Tuesday 2-4pm";
                    break;
                case 7:
                    slot = "Tuesday 4-6pm";
                    break;
                case 8:
                    slot = "Thursday 9-11";
                    break;
                case 9:
                    slot = "Thursday 11-1pm";
                    break;
                case 10:
                    slot = "Thursday 2-4pm";
                    break;
                default:
                    slot = "";
                    break;
            }
            return slot;
        }

        //function checks if spaces available are for a module
        public bool capacityCheck(String moduleCode)
        {
            bool flag2 = false;
            foreach (object temp in modules)
            {
                Module m = (Module)temp;
                String mCode = m.ModuleCode;
                if (mCode == moduleCode)
                {
                    if (m.Capacity != 0)
                    {
                        flag2 = true;
                    }

                }
            }
            if (flag2 == true)
            {
                return true; //spaces available
            }
            else
            {
                return false; //no spaces available
            }
        }

        //function returns module name associated with module code
        public String retParam(String modCode)
        {
            String name2 = "";
            foreach (object temp in modules)
            {
                Module m = (Module)temp;
                if (m.ModuleCode == modCode)
                {
                    name2 = m.ModuleName;
                }
            }
            return name2;
        }

        //functions returns one of the module's properties from the module name 
        public String frmName(String modName, String prop)
        {
            String ret = "";
            foreach (object temp in modules)
            {
                Module m = (Module)temp;
                if (m.ModuleName == modName)
                {
                    switch (prop)
                    {
                        case "prerequisites":
                            ret = m.Prerequisites;
                            break;
                        case "corequisites":
                            ret = m.Corequisites;
                            break;
                        case "code":
                            ret = m.ModuleCode;
                            break;
                        case "lectureslot":
                            ret = m.LectureSlot.ToString();
                            break;
                        case "tutorialslot":
                            ret = m.TutorialSlot.ToString();
                            break;
                        case "semester":
                            ret = m.Semester.ToString();
                            break;
                        case "capacity":
                            ret = m.Capacity.ToString();
                            break;
                        case "description":
                            ret = m.ModuleDescription;
                            break;
                        default:
                            ret = "";
                            break;
                    }
                }
            }
            return ret;
        }

        //checks for timetable clash between two modules. Will check for clash between lecture or tutorial, L1 & L2, L1 & T2, T1 & T2, L2 & T1
        public bool checkTimetableClash(String modName, String otherMod)
        {
            String lSlot = frmName(modName, "lectureslot");
            String tSlot = frmName(modName, "tutorialslot");
            String sem = frmName(modName, "semester");
            bool flag = false;
            foreach (object temp in modules)
            {
                Module m = (Module)temp;
                if (m.ModuleName == otherMod)
                {
                    if (flag == false)
                    {
                        if ((m.LectureSlot.ToString() == lSlot || m.LectureSlot.ToString() == tSlot || m.TutorialSlot.ToString() == tSlot || m.TutorialSlot.ToString() == lSlot) && m.Semester.ToString() == sem)
                        {
                            flag = true;
                        }
                    }
                }
            }
            if (flag == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //shows the relevant graphical rectangle oon the timetable
        public void showTimetableSlot(int enumu, char type, String name, int sem, bool cover)
        {
            //define graphics objects
            Graphics rlines = this.CreateGraphics();
            Graphics timetableNum = this.CreateGraphics();
            Font writeFont = new Font("Arial", 8);
            SolidBrush fontColour = new SolidBrush(Color.Black);
            Pen linePen = new Pen(Color.FromArgb(255, 255, 0, 0));
            Pen gridLinePen = new Pen(Color.FromArgb(50, 139, 139, 139));
            SolidBrush bg = new SolidBrush(Color.FromArgb(255, 240, 240, 240));
            Font typeFont = new Font("Arial", 14);
            Graphics rec = this.CreateGraphics();

            SolidBrush recColor = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            Pen recLineColor = new Pen(Color.FromArgb(0, 0, 0, 0));
            //reset mcounter when greater than 7
            if (mCounter > 7)
            {
                mCounter = 0;
            }

            //depending on mCounter, defines the colour of the box, mcounter is increased everytime a box is added
            switch (mCounter)
            {
                case 0:
                    recColor = new SolidBrush(Color.FromArgb(100, 255, 0, 0));
                    recLineColor = new Pen(Color.FromArgb(255, 255, 0, 0));
                    break;
                case 1:
                    recColor = new SolidBrush(Color.FromArgb(100, 0, 255, 0));
                    recLineColor = new Pen(Color.FromArgb(255, 0, 255, 0));
                    break;
                case 2:
                    recColor = new SolidBrush(Color.FromArgb(100, 0, 0, 255));
                    recLineColor = new Pen(Color.FromArgb(255, 0, 0, 255));
                    break;
                case 3:
                    recColor = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
                    recLineColor = new Pen(Color.FromArgb(255, 255, 255, 0));
                    break;
                case 4:
                    recColor = new SolidBrush(Color.FromArgb(100, 255, 0, 255));
                    recLineColor = new Pen(Color.FromArgb(255, 255, 0, 255));
                    break;
                case 5:
                    recColor = new SolidBrush(Color.FromArgb(100, 0, 255, 255));
                    recLineColor = new Pen(Color.FromArgb(255, 0, 255, 255));
                    break;
                case 6:
                    recColor = new SolidBrush(Color.FromArgb(100, 255, 153, 51));
                    recLineColor = new Pen(Color.FromArgb(255, 255, 153, 51));
                    break;
                case 7:
                    recColor = new SolidBrush(Color.FromArgb(100, 153, 51, 204));
                    recLineColor = new Pen(Color.FromArgb(255, 153, 51, 204));
                    break;
            }
            //check which semester is being written
            bool writeFlag = true;
            if (sem == 1 && comboBoxSelectSemester.Text != "Semester 1")
            {
                writeFlag = false;
            }
            else if (sem == 2 && comboBoxSelectSemester.Text != "Semester 2")
            {
                writeFlag = false;
            }
            //check which box 0-10 is being written
            if (writeFlag == true)
            {
                switch (enumu)
                {
                    case 0:
                        //cover the old box with a grey box matching the background
                        rec.FillRectangle(bg, new Rectangle(60, 480, 120, 50));
                        rec.DrawLine(gridLinePen, new Point(60, 480), new Point(180, 480));
                        rec.DrawLine(gridLinePen, new Point(60, 505), new Point(180, 505));
                        rec.DrawLine(gridLinePen, new Point(60, 530), new Point(180, 530));
                        rec.DrawArc(recLineColor, new Rectangle(60, 480, 120, 50), 6f, 6f);
                        //if the box requires the  old one  to be removed and the newone to be put on top
                        if (cover == false)
                        {
                            //draw rectangle withspecified colour
                            rec.FillRectangle(recColor, new Rectangle(60, 480, 120, 50));
                            //draw darkerlines  around rectangle
                            rlines.DrawLine(recLineColor, new Point(60, 480), new Point(179, 480));
                            rlines.DrawLine(recLineColor, new Point(60, 480), new Point(60, 530));
                            rlines.DrawLine(recLineColor, new Point(60, 530), new Point(179, 530));
                            rlines.DrawLine(recLineColor, new Point(179, 480), new Point(179, 530));
                            rlines.DrawLine(recLineColor, new Point(160, 480), new Point(160, 530));
                            //add a smaller rectangle to wrap text and another for L or T
                            RectangleF modName0 = new RectangleF(60, 480, 100, 50);
                            RectangleF type0 = new RectangleF(160, 495, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName0);
                            //add L or T to one of the smaller boxes
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type0);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type0);
                            }
                        }
                        break;
                    case 1:
                        rec.FillRectangle(bg, new Rectangle(60, 530, 120, 50));
                        rec.DrawLine(gridLinePen, new Point(60, 530), new Point(180, 530));
                        rec.DrawLine(gridLinePen, new Point(60, 555), new Point(180, 555));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(60, 530, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(60, 530), new Point(179, 530));
                            rlines.DrawLine(recLineColor, new Point(60, 530), new Point(60, 579));
                            rlines.DrawLine(recLineColor, new Point(60, 579), new Point(179, 579));
                            rlines.DrawLine(recLineColor, new Point(179, 530), new Point(179, 579));
                            rlines.DrawLine(recLineColor, new Point(160, 530), new Point(160, 579));
                            RectangleF modName1 = new RectangleF(60, 530, 100, 50);
                            RectangleF type1 = new RectangleF(160, 545, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName1);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type1);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type1);
                            }
                        }
                        break;
                    case 2:
                        rec.FillRectangle(bg, new Rectangle(60, 605, 120, 50));
                        rec.DrawLine(gridLinePen, new Point(60, 605), new Point(180, 605));
                        rec.DrawLine(gridLinePen, new Point(60, 630), new Point(180, 630));
                        rec.DrawLine(gridLinePen, new Point(60, 655), new Point(180, 655));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(60, 605, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(60, 605), new Point(60, 655));
                            rlines.DrawLine(recLineColor, new Point(60, 605), new Point(179, 605));
                            rlines.DrawLine(recLineColor, new Point(179, 605), new Point(179, 655));
                            rlines.DrawLine(recLineColor, new Point(60, 655), new Point(179, 655));
                            rlines.DrawLine(recLineColor, new Point(160, 605), new Point(160, 655));
                            RectangleF modName2 = new RectangleF(60, 605, 100, 50);
                            RectangleF type2 = new RectangleF(160, 620, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName2);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type2);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type2);
                            }
                        }
                        break;
                    case 3:
                        rec.FillRectangle(bg, new Rectangle(60, 655, 121, 51));
                        rec.DrawLine(gridLinePen, new Point(60, 655), new Point(180, 655));
                        rec.DrawLine(gridLinePen, new Point(60, 680), new Point(180, 680));
                        rec.DrawLine(gridLinePen, new Point(60, 705), new Point(180, 705));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(60, 655, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(60, 655), new Point(179, 655));
                            rlines.DrawLine(recLineColor, new Point(60, 655), new Point(60, 705));
                            rlines.DrawLine(recLineColor, new Point(60, 705), new Point(179, 705));
                            rlines.DrawLine(recLineColor, new Point(179, 655), new Point(179, 705));
                            rlines.DrawLine(recLineColor, new Point(160, 655), new Point(160, 705));
                            RectangleF modName3 = new RectangleF(60, 655, 100, 50);
                            RectangleF type3 = new RectangleF(160, 670, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName3);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type3);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type3);
                            }
                        }
                        break;
                    case 4:
                        rec.FillRectangle(bg, new Rectangle(180, 480, 120, 50));
                        rec.DrawLine(gridLinePen, new Point(180, 480), new Point(300, 480));
                        rec.DrawLine(gridLinePen, new Point(180, 505), new Point(300, 505));
                        rec.DrawLine(gridLinePen, new Point(180, 530), new Point(300, 530));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(180, 480, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(180, 480), new Point(299, 480));
                            rlines.DrawLine(recLineColor, new Point(180, 480), new Point(180, 530));
                            rlines.DrawLine(recLineColor, new Point(180, 530), new Point(300, 530));
                            rlines.DrawLine(recLineColor, new Point(299, 480), new Point(299, 530));
                            rlines.DrawLine(recLineColor, new Point(280, 480), new Point(280, 530));
                            RectangleF modName4 = new RectangleF(180, 480, 100, 50);
                            RectangleF type4 = new RectangleF(280, 495, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName4);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type4);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type4);
                            }
                        }
                        break;
                    case 5:
                        rec.FillRectangle(bg, new Rectangle(180, 530, 121, 51));
                        rec.DrawLine(gridLinePen, new Point(180, 530), new Point(300, 530));
                        rec.DrawLine(gridLinePen, new Point(180, 555), new Point(300, 555));
                        rec.DrawLine(gridLinePen, new Point(180, 580), new Point(300, 580));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(180, 530, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(180, 530), new Point(180, 580));
                            rlines.DrawLine(recLineColor, new Point(180, 579), new Point(299, 579));
                            rlines.DrawLine(recLineColor, new Point(180, 530), new Point(299, 530));
                            rlines.DrawLine(recLineColor, new Point(299, 530), new Point(299, 579));
                            rlines.DrawLine(recLineColor, new Point(280, 530), new Point(280, 579));
                            RectangleF modName5 = new RectangleF(180, 530, 100, 50);
                            RectangleF type5 = new RectangleF(280, 545, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName5);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type5);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type5);
                            }
                        }
                        break;
                    case 6:
                        rec.FillRectangle(bg, new Rectangle(180, 605, 121, 51));
                        rec.DrawLine(gridLinePen, new Point(180, 605), new Point(300, 605));
                        rec.DrawLine(gridLinePen, new Point(180, 630), new Point(300, 630));
                        rec.DrawLine(gridLinePen, new Point(180, 655), new Point(300, 655));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(180, 605, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(180, 605), new Point(180, 655));
                            rlines.DrawLine(recLineColor, new Point(180, 605), new Point(300, 605));
                            rlines.DrawLine(recLineColor, new Point(300, 605), new Point(300, 655));
                            rlines.DrawLine(recLineColor, new Point(180, 655), new Point(300, 655));
                            rlines.DrawLine(recLineColor, new Point(280, 605), new Point(280, 655));
                            RectangleF modName6 = new RectangleF(180, 605, 100, 50);
                            RectangleF type6 = new RectangleF(280, 620, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName6);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type6);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type6);
                            }
                        }
                        break;
                    case 7:
                        rec.FillRectangle(bg, new Rectangle(180, 655, 121, 51));
                        rec.DrawLine(gridLinePen, new Point(180, 705), new Point(300, 705));
                        rec.DrawLine(gridLinePen, new Point(180, 655), new Point(300, 655));
                        rec.DrawLine(gridLinePen, new Point(180, 680), new Point(300, 680));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(180, 655, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(180, 655), new Point(180, 705));
                            rlines.DrawLine(recLineColor, new Point(180, 705), new Point(300, 705));
                            rlines.DrawLine(recLineColor, new Point(300, 655), new Point(300, 705));
                            rlines.DrawLine(recLineColor, new Point(180, 655), new Point(300, 655));
                            rlines.DrawLine(recLineColor, new Point(280, 655), new Point(280, 704));
                            RectangleF modName7 = new RectangleF(180, 655, 100, 50);
                            RectangleF type7 = new RectangleF(280, 670, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName7);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type7);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type7);
                            }
                        }
                        break;
                    case 8:
                        rec.FillRectangle(bg, new Rectangle(420, 480, 121, 51));
                        rec.DrawLine(gridLinePen, new Point(420, 480), new Point(539, 480));
                        rec.DrawLine(gridLinePen, new Point(420, 505), new Point(539, 505));
                        rec.DrawLine(gridLinePen, new Point(420, 530), new Point(539, 530));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(420, 480, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(420, 480), new Point(420, 530));
                            rlines.DrawLine(recLineColor, new Point(420, 480), new Point(539, 480));
                            rlines.DrawLine(recLineColor, new Point(539, 480), new Point(539, 530));
                            rlines.DrawLine(recLineColor, new Point(420, 530), new Point(539, 530));
                            rlines.DrawLine(recLineColor, new Point(520, 480), new Point(520, 530));
                            RectangleF modName8 = new RectangleF(420, 480, 100, 50);
                            RectangleF type8 = new RectangleF(520, 495, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName8);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type8);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type8);
                            }
                        }
                        break;
                    case 9:
                        rec.FillRectangle(bg, new Rectangle(420, 530, 121, 51));
                        rec.DrawLine(gridLinePen, new Point(420, 530), new Point(540, 530));
                        rec.DrawLine(gridLinePen, new Point(420, 555), new Point(540, 555));
                        rec.DrawLine(gridLinePen, new Point(420, 580), new Point(540, 580));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(420, 530, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(420, 530), new Point(420, 579));
                            rlines.DrawLine(recLineColor, new Point(420, 579), new Point(539, 579));
                            rlines.DrawLine(recLineColor, new Point(420, 530), new Point(539, 530));
                            rlines.DrawLine(recLineColor, new Point(539, 530), new Point(539, 579));
                            rlines.DrawLine(recLineColor, new Point(520, 530), new Point(520, 579));
                            RectangleF modName9 = new RectangleF(420, 530, 100, 50);
                            RectangleF type9 = new RectangleF(520, 545, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName9);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type9);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type9);
                            }
                        }
                        break;
                    case 10:
                        rec.FillRectangle(bg, new Rectangle(420, 605, 121, 51));
                        rec.DrawLine(gridLinePen, new Point(420, 605), new Point(540, 605));
                        rec.DrawLine(gridLinePen, new Point(420, 630), new Point(540, 630));
                        rec.DrawLine(gridLinePen, new Point(420, 655), new Point(540, 655));
                        if (cover == false)
                        {
                            rec.FillRectangle(recColor, new Rectangle(420, 605, 120, 50));
                            rlines.DrawLine(recLineColor, new Point(420, 605), new Point(539, 605));
                            rlines.DrawLine(recLineColor, new Point(420, 605), new Point(420, 655));
                            rlines.DrawLine(recLineColor, new Point(539, 605), new Point(539, 655));
                            rlines.DrawLine(recLineColor, new Point(420, 655), new Point(539, 655));
                            rlines.DrawLine(recLineColor, new Point(520, 605), new Point(520, 655));
                            RectangleF modName10 = new RectangleF(420, 605, 100, 50);
                            RectangleF type10 = new RectangleF(520, 620, 20, 50);
                            timetableNum.DrawString(name, writeFont, fontColour, modName10);
                            if (type == 'L')
                            {
                                timetableNum.DrawString("L", typeFont, fontColour, type10);
                            }
                            else
                            {
                                timetableNum.DrawString("T", typeFont, fontColour, type10);
                            }
                        }
                        break;
                }
            }
        }

        public ModuleSelection()
        {
            InitializeComponent();
            //populate combo box
            comboBoxSelectSemester.Items.Add("Semester 1");
            comboBoxSelectSemester.Items.Add("Semester 2");
            //set combo box to semester 1
            comboBoxSelectSemester.Text = "Semester 1";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(workingDir + @"\modules.xml");
            //read in xml elements to arrays
            //all modulenames in one array, all modules codes in another etc
            XmlNodeList name = xmlDoc.GetElementsByTagName("modulename");
            XmlNodeList code = xmlDoc.GetElementsByTagName("modulecode");
            XmlNodeList tSlot = xmlDoc.GetElementsByTagName("tutorialslot");
            XmlNodeList lSlot = xmlDoc.GetElementsByTagName("lectureslot");
            XmlNodeList cap = xmlDoc.GetElementsByTagName("capacity");
            XmlNodeList pre = xmlDoc.GetElementsByTagName("prerequisites");
            XmlNodeList co = xmlDoc.GetElementsByTagName("corequisites");
            XmlNodeList modDesc = xmlDoc.GetElementsByTagName("moduledescription");
            XmlNodeList sem = xmlDoc.GetElementsByTagName("semester");
            int i = code.Count;
            for (int j = 0; j < i; j++)
                //one number corresponds to one module so in each array the module details for each is at the samenumber. i.e if the module is array position 5 in module name array then the relevant code will be at position 5 in the code array etc
            {
                try
                {
                    //add module name to listbox
                    lbAllModules.Items.Add(name[j].InnerText);
                    //add module details to object and then arraylist using Module class
                    modules.Add(new Module(name[j].InnerText, code[j].InnerText, int.Parse(tSlot[j].InnerText), int.Parse(lSlot[j].InnerText), int.Parse(cap[j].InnerText), pre[j].InnerText, co[j].InnerText, modDesc[j].InnerText, int.Parse(sem[j].InnerText)));
                }
                catch (NullReferenceException ex)
                {
                    //if a modulecannot be added
                    MessageBox.Show(ex.ToString());
                }
            }
            //populate for testing purposes
            //textBoxFirstName.Text = "Adrian";
            //textBoxSurname.Text = "Ko";
            //textBoxStudentID.Text = "w1314507";
            //lbSelectedModules.Items.Add("Object-Oriented Design");
            //lbAllModules.Items.Remove("Object-Oriented Design");
            //lbSelectedModules.Items.Add("C# .Net Framework Programming");
            //lbAllModules.Items.Remove("C# .Net Framework Programming");
            //lbSelectedModules.Items.Add("Requirements Engineering");
            //lbAllModules.Items.Remove("Requirements Engineering");
            //lbSelectedModules.Items.Add("Intelligent Systems");
            //lbAllModules.Items.Remove("Intelligent Systems");
            //lbSelectedModules.Items.Add("Introduction to AI");
            //lbAllModules.Items.Remove("Introduction to AI");
            //lbSelectedModules.Items.Add("Mobile User Interface Development");
            //lbAllModules.Items.Remove("Mobile User Interface Development");
            //lbSelectedModules.Items.Add("Java Mobile Application Development");
            //lbAllModules.Items.Remove("Java Mobile Application Development");
            //lbSelectedModules.Items.Add("Interactive Multimedia");
            //lbAllModules.Items.Remove("Interactive Multimedia");
            //end populate for testing
        }

        //add timetable graphics to the form on load, horizontal lines, time numbers and Monday to Friday
        private void ModuleSelection_Paint(object sender, PaintEventArgs e)
        {
            //so that the graphics are not drawn everytime a click event is invoked
            if (loadFlag == false)
            {
                //create the colours and fonts for the graphics
                Pen linePen = new Pen(Color.FromArgb(50, 139, 139, 139));
                Pen outlinePen = new Pen(Color.FromArgb(50, 155, 155, 155));
                SolidBrush backgroundCover = new SolidBrush(Color.FromArgb(255, 240, 240, 240));
                Graphics lines = this.CreateGraphics();
                lines.FillRectangle(backgroundCover, new Rectangle(13, 445, 623, 270));
                //draw timetable outline
                lines.DrawLine(outlinePen, new Point(13, 445), new Point(13, 715));
                lines.DrawLine(outlinePen, new Point(13, 445), new Point(636, 445));
                lines.DrawLine(outlinePen, new Point(13, 715), new Point(636, 715));
                lines.DrawLine(outlinePen, new Point(636, 445), new Point(636, 715));
                //draw horizontal lines across form
                lines.DrawLine(linePen, new Point(45, 480), new Point(630, 480));
                lines.DrawLine(linePen, new Point(45, 505), new Point(630, 505));
                lines.DrawLine(linePen, new Point(45, 530), new Point(630, 530));
                lines.DrawLine(linePen, new Point(45, 555), new Point(630, 555));
                lines.DrawLine(linePen, new Point(45, 580), new Point(630, 580));
                lines.DrawLine(linePen, new Point(45, 605), new Point(630, 605));
                lines.DrawLine(linePen, new Point(45, 630), new Point(630, 630));
                lines.DrawLine(linePen, new Point(45, 655), new Point(630, 655));
                lines.DrawLine(linePen, new Point(45, 680), new Point(630, 680));
                lines.DrawLine(linePen, new Point(45, 705), new Point(630, 705));
                linePen.Dispose();
                lines.Dispose();
                //add Monday to Friday and 9-18 along left handside
                Graphics timetableNum = this.CreateGraphics();
                Font drawFont = new Font("Arial", 8);
                Font dayFont = new Font("Arial", 14);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                timetableNum.DrawString("Monday", dayFont, drawBrush, 60, 450);
                timetableNum.DrawString("Tuesday", dayFont, drawBrush, 180, 450);
                timetableNum.DrawString("Wednesday", dayFont, drawBrush, 300, 450);
                timetableNum.DrawString("Thursday", dayFont, drawBrush, 420, 450);
                timetableNum.DrawString("Friday", dayFont, drawBrush, 540, 450);
                timetableNum.DrawString("9", drawFont, drawBrush, 25, 473);
                timetableNum.DrawString("10", drawFont, drawBrush, 25, 498);
                timetableNum.DrawString("11", drawFont, drawBrush, 25, 523);
                timetableNum.DrawString("12", drawFont, drawBrush, 25, 548);
                timetableNum.DrawString("13", drawFont, drawBrush, 25, 573);
                timetableNum.DrawString("14", drawFont, drawBrush, 25, 598);
                timetableNum.DrawString("15", drawFont, drawBrush, 25, 623);
                timetableNum.DrawString("16", drawFont, drawBrush, 25, 648);
                timetableNum.DrawString("17", drawFont, drawBrush, 25, 673);
                timetableNum.DrawString("18", drawFont, drawBrush, 25, 698);
                drawFont.Dispose();
                drawBrush.Dispose();
                timetableNum.Dispose();
                loadFlag = true;
            }
        }

        //click event for adding a module to the selected modules listbox
        private void buttonToRight_Click(object sender, EventArgs e)
        {
            bool flagClash = false;
            try
            {
                //check if there are less then 8 modules, if so continue, else, 8 modules already selected
                if (lbSelectedModules.Items.Count < 8)
                {
                    //check if there are any modules already in the list box
                    if (lbSelectedModules.Items.Count != 0)
                    {
                        //loop to check timetable clashes against modules already chosen
                        foreach (String otherMod in lbSelectedModules.Items)
                        {
                            //checks if a clash has already been found
                            if (flagClash == false)
                            {
                                //function that checks the time enumeration and ensures no clash
                                if (checkTimetableClash(lbAllModules.SelectedItem.ToString(), otherMod) == true)
                                {
                                    //set clach to found
                                    flagClash = true;
                                    //tell user which module the chosen clashes with
                                    MessageBox.Show("Timetable Clash with '" + otherMod + "'");
                                }
                            }
                        }
                    }
                    //if there are no timetable clashes
                    if (flagClash == false)
                    {
                        //get the selected module's lecture slot
                        String lec = frmName(lbAllModules.SelectedItem.ToString(), "lectureslot");
                        //get the selected module's semester
                        String sem = frmName(lbAllModules.SelectedItem.ToString(), "semester");
                        //call function to show lecture timetable slot on timetable
                        showTimetableSlot(int.Parse(lec), 'L', lbAllModules.SelectedItem.ToString(), int.Parse(sem), false);
                        //get the selected modules tutorial slot
                        String tut = frmName(lbAllModules.SelectedItem.ToString(), "tutorialslot");
                        //call function to show tutorial timetable slot on timetable
                        showTimetableSlot(int.Parse(tut), 'T', lbAllModules.SelectedItem.ToString(), int.Parse(sem), false);
                        //to allow colour change for next module
                        mCounter++;
                        //add module to slected modules list box
                        lbSelectedModules.Items.Add(lbAllModules.SelectedItem);
                        //prevent listbox selectedindexchanged from throwing an exception as adding a module will invoke an event to run the SelectedIndexChanged event to run again
                        flag = true;
                        //remove module from modules list box 
                        lbAllModules.Items.Remove(lbAllModules.SelectedItem);
                        flag = false;
                        //change selected modules number
                        lblSelectedModulesOut.Text = lbSelectedModules.Items.Count.ToString();
                    }
                }
                else
                {
                    //tell user that they have already chosen 8 modules
                    MessageBox.Show("You have already selected the maximum number of modules.");
                }
            }
            catch
            {
                //modue not selected in listbox, tell user
                MessageBox.Show("Please select a module.");
            }
        }

        //clickevent for removing a module from the selected modules list box
        private void buttonToLeft_Click(object sender, EventArgs e)
        {
            try
            {
                String lec = frmName(lbSelectedModules.SelectedItem.ToString(), "lectureslot");
                String sem = frmName(lbSelectedModules.SelectedItem.ToString(), "semester");
                //same as above except the 5th arguement is true, so only a covering rectangle is placed  over the old one and a new one is not written.
                showTimetableSlot(int.Parse(lec), 'L', lbSelectedModules.SelectedItem.ToString(), int.Parse(sem), true);
                String tut = frmName(lbSelectedModules.SelectedItem.ToString(), "tutorialslot");
                showTimetableSlot(int.Parse(tut), 'T', lbSelectedModules.SelectedItem.ToString(), int.Parse(sem), true);
                //add module to all modules list box
                lbAllModules.Items.Add(lbSelectedModules.SelectedItem);
                flag = true;
                //remove module from selected modules list
                lbSelectedModules.Items.Remove(lbSelectedModules.SelectedItem);
                flag = false;
                lblSelectedModulesOut.Text = lbSelectedModules.Items.Count.ToString();
            }
            catch
            {
                MessageBox.Show("Please select a module.");
            }
        }
        
        //click event for clicking a module in the listbox
        private void lbAllModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            //prevent exception thrown
            if (flag == false)
            {
                //write module selected details to relevant labels
                lblModuleNameOut.Text = lbAllModules.SelectedItem.ToString();
                lblCodeOut.Text = frmName(lbAllModules.SelectedItem.ToString(), "code");
                lblLectureSlotOut.Text = retSlot(int.Parse(frmName(lbAllModules.SelectedItem.ToString(), "lectureslot")));
                lblTutorialSlotOut.Text = retSlot(int.Parse(frmName(lbAllModules.SelectedItem.ToString(), "tutorialslot")));
                lblCapacityOut.Text = frmName(lbAllModules.SelectedItem.ToString(), "capacity");
                lblSemesterOut.Text = frmName(lbAllModules.SelectedItem.ToString(), "semester");
                lblPrerequisiteOut.Text = frmName(lbAllModules.SelectedItem.ToString(), "prerequisites");
                lblCorequisiteOut.Text = frmName(lbAllModules.SelectedItem.ToString(), "corequisites");
                lblModDescOut.Text = frmName(lbAllModules.SelectedItem.ToString(), "description");

            }
        }
        //click event for clicking a module in the selected modules listbox
        private void lbSelectedModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flag == false)
            {
                //write module selected details to relevant labels
                lblModuleNameOut.Text = lbSelectedModules.SelectedItem.ToString();
                lblCodeOut.Text = frmName(lbSelectedModules.SelectedItem.ToString(), "code");
                lblLectureSlotOut.Text = retSlot(int.Parse(frmName(lbSelectedModules.SelectedItem.ToString(), "lectureslot")));
                lblTutorialSlotOut.Text = retSlot(int.Parse(frmName(lbSelectedModules.SelectedItem.ToString(), "tutorialslot")));
                lblCapacityOut.Text = frmName(lbSelectedModules.SelectedItem.ToString(), "capacity");
                lblSemesterOut.Text = frmName(lbSelectedModules.SelectedItem.ToString(), "semester");
                lblPrerequisiteOut.Text = frmName(lbSelectedModules.SelectedItem.ToString(), "prerequisites");
                lblCorequisiteOut.Text = frmName(lbSelectedModules.SelectedItem.ToString(), "corequisites");
                lblModDescOut.Text = frmName(lbSelectedModules.SelectedItem.ToString(), "description");
            }
        }

        //click event that shows the timetable for the selected semester
        private void comboBoxSelectSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            //defeine pens and brushes
            Pen linePen = new Pen(Color.FromArgb(50, 139, 139, 139));
            Pen outlinePen = new Pen(Color.FromArgb(50, 155, 155, 155));
            SolidBrush backgroundCover = new SolidBrush(Color.FromArgb(255, 240, 240, 240));
            //create graphics object
            Graphics lines = this.CreateGraphics();
            //redraw all timetable default lines and background i.e. we start from scratch
            lines.FillRectangle(backgroundCover, new Rectangle(13, 445, 623, 270));
            lines.DrawLine(outlinePen, new Point(13, 445), new Point(13, 715));
            lines.DrawLine(outlinePen, new Point(13, 445), new Point(636, 445));
            lines.DrawLine(outlinePen, new Point(13, 715), new Point(636, 715));
            lines.DrawLine(outlinePen, new Point(636, 445), new Point(636, 715));
            lines.DrawLine(linePen, new Point(45, 480), new Point(630, 480));
            lines.DrawLine(linePen, new Point(45, 505), new Point(630, 505));
            lines.DrawLine(linePen, new Point(45, 530), new Point(630, 530));
            lines.DrawLine(linePen, new Point(45, 555), new Point(630, 555));
            lines.DrawLine(linePen, new Point(45, 580), new Point(630, 580));
            lines.DrawLine(linePen, new Point(45, 605), new Point(630, 605));
            lines.DrawLine(linePen, new Point(45, 630), new Point(630, 630));
            lines.DrawLine(linePen, new Point(45, 655), new Point(630, 655));
            lines.DrawLine(linePen, new Point(45, 680), new Point(630, 680));
            lines.DrawLine(linePen, new Point(45, 705), new Point(630, 705));
            linePen.Dispose();
            lines.Dispose();
            Graphics timetableNum = this.CreateGraphics();
            Font drawFont = new Font("Arial", 8);
            Font dayFont = new Font("Arial", 14);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            //replace all text on timetable
            timetableNum.DrawString("Monday", dayFont, drawBrush, 60, 450);
            timetableNum.DrawString("Tuesday", dayFont, drawBrush, 180, 450);
            timetableNum.DrawString("Wednesday", dayFont, drawBrush, 300, 450);
            timetableNum.DrawString("Thursday", dayFont, drawBrush, 420, 450);
            timetableNum.DrawString("Friday", dayFont, drawBrush, 540, 450);
            timetableNum.DrawString("9", drawFont, drawBrush, 25, 473);
            timetableNum.DrawString("10", drawFont, drawBrush, 25, 498);
            timetableNum.DrawString("11", drawFont, drawBrush, 25, 523);
            timetableNum.DrawString("12", drawFont, drawBrush, 25, 548);
            timetableNum.DrawString("13", drawFont, drawBrush, 25, 573);
            timetableNum.DrawString("14", drawFont, drawBrush, 25, 598);
            timetableNum.DrawString("15", drawFont, drawBrush, 25, 623);
            timetableNum.DrawString("16", drawFont, drawBrush, 25, 648);
            timetableNum.DrawString("17", drawFont, drawBrush, 25, 673);
            timetableNum.DrawString("18", drawFont, drawBrush, 25, 698);
            drawFont.Dispose();
            drawBrush.Dispose();
            timetableNum.Dispose();
            //if the selected semester is 1
            if (comboBoxSelectSemester.SelectedItem.ToString() == "Semester 1")
            {
                //loop through all the modules in the selected moduleslist box
                foreach (String item in lbSelectedModules.Items)
                {
                    String sem = frmName(item, "semester");
                    //only modules on the timetable where the semester is 1
                    if (sem == "1")
                    {
                        //show the relevant rectangles for the lecture and tutorial slot
                        String lec = frmName(item, "lectureslot");
                        showTimetableSlot(int.Parse(lec), 'L', item, int.Parse(sem), false);
                        String tut = frmName(item, "tutorialslot");
                        showTimetableSlot(int.Parse(tut), 'T', item, int.Parse(sem), false);
                        mCounter++;
                    }
                }
            }
            //if the selected semester is 2
            else if (comboBoxSelectSemester.SelectedItem.ToString() == "Semester 2")
            {
                //loop through all the modules in the selected moduleslist box
                foreach (String item in lbSelectedModules.Items)
                {
                    String sem = frmName(item.ToString(), "semester");
                    //only modules on the timetable where the semester is 2
                    if (sem == "2")
                    {
                        //show the relevant rectangles for the lecture and tutorial slot
                        String lec = frmName(item.ToString(), "lectureslot");
                        showTimetableSlot(int.Parse(lec), 'L', item.ToString(), int.Parse(sem), false);
                        String tut = frmName(item, "tutorialslot");
                        showTimetableSlot(int.Parse(tut), 'T', item.ToString(), int.Parse(sem), false);
                        mCounter++;
                    }
                }
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            //define message varaibles
            String prereqmsg = null;
            String coreqmsg = null;
            String capmsg = null;
            //checd if the studentinfo textboxes are empty
            if (textBoxFirstName.Text.Trim().Equals("") || textBoxSurname.Text.Trim().Equals("") || textBoxStudentID.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please enter all personal information");
            }
            else
            {
                //make sure there are 8 modules selected
                if (lbSelectedModules.Items.Count < 8)
                {
                    MessageBox.Show("Please select at least 8 modules");
                }
                else
                {
                    //create a module array
                    String[] items = new String[8];
                    int k = 0;
                    foreach (String selected in lbSelectedModules.Items)
                    {
                        items[k] = selected;
                        k++;
                    }
                    //prerequisite check
                    for (int i = 0; i < 8; i++)
                    {
                        bool flag = false;
                        if (frmName(items[i], "prerequisites") != "none")
                        {
                            //check that a module has the relevant prerequisite in the list
                            String prename = retParam(frmName(items[i], "prerequisites"));
                            foreach (String otherMod in lbSelectedModules.Items)
                            {
                                if (flag == false)
                                {
                                    if (otherMod == prename)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            if (flag == false)
                            {
                                //if the list does not contain the relevant prerequsite module then add to the messages variable
                                prereqmsg += "'" + items[i] + "' requires '" + prename + "' as a prerequisite.\n";
                            }
                        }
                    }
                    //corequisite check
                    for (int i = 0; i < 8; i++)
                    {
                        bool flag = false;
                        if (frmName(items[i], "corequisites") != "none")
                        {
                            String coname = retParam(frmName(items[i], "corequisites"));
                            foreach (String otherMod in lbSelectedModules.Items)
                            {
                                if (flag == false)
                                {
                                    if (otherMod == coname)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            if (flag == false)
                            {
                                //if the list does not contain the relevant corequsite module then add to the messages variable
                                coreqmsg += "'" + items[i] + "' requires '" + coname + "' as a co-requisite.\n";
                            }
                        }
                    }
                    //capacity check
                    for (int i = 0; i < 8; i++)
                    {
                        //check that the module has spaces available
                        if (capacityCheck(frmName(items[i], "code")) == false)
                        {
                            capmsg += "'" + items[i] + "' has no more spaces available.\n";
                        }
                    }
                    bool flag5 = true;
                    //if the the variable is not empty then there are modules with prerequisites missing
                    if (prereqmsg != null)
                    {
                        MessageBox.Show(prereqmsg);
                        flag5 = false;
                    }
                    bool flag6 = true;
                    //if the the variable is not empty then there are modules with corequisites missing
                    if (coreqmsg != null)
                    {
                        MessageBox.Show(coreqmsg);
                        flag6 = false;
                    }
                    bool flag8 = true;
                    ////if the the variable is not empty then there are modules full capacities
                    if (capmsg != null)
                    {
                        MessageBox.Show(capmsg);
                        flag8 = false;
                    }
                    //if all modules have relevant prerequisites, corequisites and no capacities are zero
                    if (flag5 == true && flag6 == true && flag8 == true)
                    {
                        //create a new selection object that stores student info and module selection info
                        studentSelectedModules = new Selection(textBoxFirstName.Text, textBoxSurname.Text, textBoxStudentID.Text, items[0], items[1], items[2], items[3], items[4], items[5], items[6], items[7]);
                        f2 = new SelectionSummary(studentSelectedModules, modules);
                        //show the selection summary form
                        f2.Activate();
                        f2.ShowDialog();
                    }
                }
            }
        }
    }
}