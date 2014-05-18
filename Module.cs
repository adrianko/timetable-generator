using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Timetable_v2
{
    public class Module
    {
        //constructor for the class, takes 9 arguements - module data
        public Module(String moduleName, String moduleCode, int tutorialSlot, int lectureSlot, int capacity, String prerequisites, String corequisites, String moduleDescription, int semester)
        {
            this.ModuleName = moduleName;
            this.ModuleCode = moduleCode;
            this.TutorialSlot = tutorialSlot;
            this.LectureSlot = lectureSlot;
            this.Capacity = capacity;
            this.Prerequisites = prerequisites;
            this.Corequisites = corequisites;
            this.ModuleDescription = moduleDescription;
            this.Semester = semester;
        }
        //getters and setters for all the properties
        String moduleName;

        public String ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }
         
        String moduleCode;

        public String ModuleCode
        {
            get { return moduleCode; }
            set { moduleCode = value; }
        }

        int tutorialSlot;

        public int TutorialSlot
        {
            get { return tutorialSlot; }
            set { tutorialSlot = value; }
        }

        int lectureSlot;

        public int LectureSlot
        {
            get { return lectureSlot; }
            set { lectureSlot = value; }
        }

        int capacity;

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        String prerequisites;

        public String Prerequisites
        {
            get { return prerequisites; }
            set { prerequisites = value; }
        }

        String corequisites;

        public String Corequisites
        {
            get { return corequisites; }
            set { corequisites = value; }
        }

        String moduleDescription;

        public String ModuleDescription
        {
            get { return moduleDescription; }
            set { moduleDescription = value; }
        }
        
        int semester;

        public int Semester
        {
            get { return semester; }
            set { semester = value; }
        }
    }
}
