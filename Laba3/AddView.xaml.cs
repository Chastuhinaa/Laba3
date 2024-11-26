using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Laba3
{
    public partial class AddView : ContentPage
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Discipline { get; set; }
        public string AuthorText { get; set; }
        public string Teacher { get; set; }
        public string Control { get; set; }
        public int Audience { get; set; }

        public ObservableCollection<Lesson> Lessons { get; set; }

        public event EventHandler LessonAdded;

        public AddView()
        {
            InitializeComponent();

            BindingContext = this;
            Lessons = new ObservableCollection<Lesson>();
        }

        private async void SubmitClicked(object sender, EventArgs e)
        {
            
            if (!IsValidDate(Date))
            {
                await DisplayAlert("Помилка", "Дата повинна бути у форматі день.місяць.рік (наприклад, 26.11.2024).", "ОК");
                return;
            }

            
            if (!IsValidTime(Time))
            {
                await DisplayAlert("Помилка", "Час повинен бути у форматі години:хвилини (наприклад, 12:46).", "ОК");
                return;
            }

         
            if (!IsValidText(Discipline))
            {
                await DisplayAlert("Помилка", "Назва дисципліни не повинна містити цифри.", "ОК");
                return;
            }

           
            if (!IsValidText(Teacher))
            {
                await DisplayAlert("Помилка", "Ім'я викладача не повинно містити цифри.", "ОК");
                return;
            }

           
            Schelude file = Schelude.GetInstance();
            file.AddLesson(Date, Time, Discipline, Teacher, Audience, Control);
            file.index = file.Data.Count - 1;
            LessonAdded?.Invoke(this, EventArgs.Empty);
            Application.Current.CloseWindow(this.Window);
        }

        
        private bool IsValidDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return false;

            string[] parts = date.Split('.');
            if (parts.Length != 3) return false;

            bool isValidDay = int.TryParse(parts[0], out int day) && day >= 1 && day <= 31;
            bool isValidMonth = int.TryParse(parts[1], out int month) && month >= 1 && month <= 12;
            bool isValidYear = int.TryParse(parts[2], out int year) && year > 0;

            return isValidDay && isValidMonth && isValidYear;
        }

        
        private bool IsValidTime(string time)
        {
            if (string.IsNullOrWhiteSpace(time)) return false;

            string[] parts = time.Split(':');
            if (parts.Length != 2) return false;

            bool isValidHours = int.TryParse(parts[0], out int hours) && hours >= 0 && hours <= 23;
            bool isValidMinutes = int.TryParse(parts[1], out int minutes) && minutes >= 0 && minutes <= 59;

            return isValidHours && isValidMinutes;
        }

        
        private bool IsValidText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            
            return text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }
    }
}
