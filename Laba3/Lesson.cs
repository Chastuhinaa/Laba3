using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba3
{
    public class Lesson
    {
        public bool IsSelected { get; set; }  
        public string Discipline { get; set; }
        public string Teacher { get; set; }
        public int Audience { get; set; }
        public string Control { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public Lesson(string time, string Date ,string discipline, string teacher, int audience, string Control)
        {
            Time = time;
            this.Date = Date;
            Discipline = discipline;
            Audience = audience;
            this.Control = Control;
            Teacher =  teacher;
        }
    }
}