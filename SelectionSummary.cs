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
    public partial class SelectionSummary : Form
    {
        //define objects and variables to be used
        Selection select;
        ArrayList modules;
        String workingDir = Directory.GetCurrentDirectory();
        ModuleSelection ms = new ModuleSelection();
        bool flag = false;

        //constructor that reads in module selection object and the arraylist containing all the module objects
        public SelectionSummary(Selection sel, ArrayList al)
        {
            InitializeComponent();
            this.select = sel;
            this.modules = al;
        }

        //function that returns a specified modules element data
        public String retParam(String modName, String prop)
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

        //function that writes out the students' specified modules to an xml file <id>_timetable.xml stored in the bin/Debug folder
        public void writeXML()
        {
            String[] modArray = new String[8] { select.Mod1, select.Mod2, select.Mod3, select.Mod4, select.Mod5, select.Mod6, select.Mod7, select.Mod8 };
            XmlTextWriter timetable = new XmlTextWriter(workingDir + @"\"+select.ID + "_timetable.xml", null);
            timetable.Formatting = Formatting.Indented;
            timetable.WriteStartDocument();
            timetable.WriteStartElement("timetable", "");
            timetable.WriteStartElement("studentname", "");
            timetable.WriteString(select.FName+" "+select.LName);
            timetable.WriteEndElement();
            timetable.WriteStartElement("studentid", "");
            timetable.WriteString(select.ID);
            timetable.WriteEndElement();
            for (int i = 0; i < 8; i++)
            {
                timetable.WriteStartElement("module", "");
                timetable.WriteStartElement("modulename", "");
                timetable.WriteString(modArray[i]);
                timetable.WriteEndElement();
                timetable.WriteStartElement("modulecode", "");
                timetable.WriteString(retParam(modArray[i], "code"));
                timetable.WriteEndElement();
                timetable.WriteStartElement("semester", "");
                timetable.WriteString(retParam(modArray[i], "semester"));
                timetable.WriteEndElement();
                timetable.WriteStartElement("lectureslot", "");
                timetable.WriteString(retParam(modArray[i], "lectureslot"));
                timetable.WriteEndElement();
                timetable.WriteStartElement("tutorialslot", "");
                timetable.WriteString(retParam(modArray[i], "tutorialslot"));
                timetable.WriteEndElement();
                timetable.WriteEndElement();
            }
            timetable.WriteEndElement();
            timetable.WriteEndDocument();
            timetable.Close();
            //update the capacities in the modules.xml file
            File.Delete(workingDir + @"\modules.xml");
            XmlTextWriter updateModule = new XmlTextWriter("modules.xml", null);
            updateModule.Formatting = Formatting.Indented;
            updateModule.WriteStartDocument();
            updateModule.WriteStartElement("modules", "");
            foreach (object temp in modules)
            {
                Module m = (Module)temp;
                updateModule.WriteStartElement("module", "");
                updateModule.WriteStartElement("modulename", "");
                updateModule.WriteString(m.ModuleName);
                updateModule.WriteEndElement();
                updateModule.WriteStartElement("modulecode", "");
                updateModule.WriteString(m.ModuleCode);
                updateModule.WriteEndElement();
                updateModule.WriteStartElement("lectureslot", "");
                updateModule.WriteString(m.LectureSlot.ToString());
                updateModule.WriteEndElement();
                updateModule.WriteStartElement("tutorialslot", "");
                updateModule.WriteString(m.TutorialSlot.ToString());
                updateModule.WriteEndElement();
                int newcapacity;
                //if module is in the selection then decrease capacity by 1
                if (modArray.Contains(m.ModuleName))
                {
                    newcapacity = m.Capacity - 1;
                }
                else
                {
                    newcapacity = m.Capacity;
                }
                updateModule.WriteStartElement("capacity", "");
                updateModule.WriteString(newcapacity.ToString());
                updateModule.WriteEndElement();
                updateModule.WriteStartElement("prerequisites", "");
                updateModule.WriteString(m.Prerequisites);
                updateModule.WriteEndElement();
                updateModule.WriteStartElement("corequisites", "");
                updateModule.WriteString(m.Corequisites);
                updateModule.WriteEndElement();
                updateModule.WriteStartElement("moduledescription", "");
                updateModule.WriteString(m.ModuleDescription);
                updateModule.WriteEndElement();
                updateModule.WriteStartElement("semester", "");
                updateModule.WriteString(m.Semester.ToString());
                updateModule.WriteEndElement();
                updateModule.WriteEndElement();
            }
            updateModule.WriteEndElement();
            updateModule.WriteEndDocument();
            updateModule.Close();
            MessageBox.Show("Timetable successfully written to XML file.\nYou may now exit the program.");
        }
        
        private void btGetTimetable_Click(object sender, EventArgs e)
        {
            //write out the timetable xml file
            writeXML();     
        }

        private void SelectionSummary_Paint(object sender, PaintEventArgs e)
        {
            //so that the graphics are not  drawn everytime a click event is invoked
            if (flag == false)
            {
                //draw the graphics
                Graphics summary = this.CreateGraphics();
                Font drawFont = new Font("Arial", 8);
                Font tableHeader = new Font("Arial", 8, FontStyle.Bold);
                Font titleFont = new Font("Arial", 14);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                String[] modArray = new String[8] { select.Mod1, select.Mod2, select.Mod3, select.Mod4, select.Mod5, select.Mod6, select.Mod7, select.Mod8 };
                summary.DrawString("Module Name", tableHeader, drawBrush, 30, 25);
                summary.DrawString("Code", tableHeader, drawBrush, 270, 25);
                summary.DrawString("Semester", tableHeader, drawBrush, 350, 25);
                summary.DrawString("Lecture Slot", tableHeader, drawBrush, 420, 25);
                summary.DrawString("Tutorial Slot", tableHeader, drawBrush, 540, 25);
                int j = 50;
                for (int i = 0; i < 8; i++)
                {
                    int g = i + 1;
                    summary.DrawString(g + ") " + modArray[i], drawFont, drawBrush, 20, j);
                    summary.DrawString(retParam(modArray[i], "code"), drawFont, drawBrush, 270, j);
                    summary.DrawString("SEM" + retParam(modArray[i], "semester"), drawFont, drawBrush, 350, j);
                    summary.DrawString(ms.retSlot(int.Parse(retParam(modArray[i], "lectureslot"))), drawFont, drawBrush, 420, j);
                    summary.DrawString(ms.retSlot(int.Parse(retParam(modArray[i], "tutorialslot"))), drawFont, drawBrush, 540, j);
                    j = j + 25;
                }
                drawFont.Dispose();
                drawBrush.Dispose();
                summary.Dispose();
                flag = true;
            }
        }
    }
}