using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Laba3;

public partial class EditView : ContentPage
{
    public string Date { get; set; }
    public int Audience { get; set; }
    public string Time { get; set; }
    public string Discipline { get; set; }
    public string Teacher { get; set; }
    public string Control { get; set; }

    public EditView()
    {
        InitializeComponent();

        Schelude file = Schelude.GetInstance();

        file.FindIndex();

        if (file.Data.Count > 0 && file.index < file.Data.Count)
        {
            this.Date = file.Data[file.index].Date;
            this.Audience = file.Data[file.index].Audience;
            this.Control = file.Data[file.index].Control;
            this.Discipline = file.Data[file.index].Discipline;
            this.Time = file.Data[file.index].Time;
            this.Teacher = file.Data[file.index].Teacher;
        }

        BindingContext = this;
    }

    private async void OkClicked(object sender, EventArgs e)
    {
       
        if (!Regex.IsMatch(Date, @"^([0-2][0-9]|3[0-1])\.(0[1-9]|1[0-2])\.\d{4}$"))
        {
            await DisplayAlert("Помилка", "Дата має бути у форматі дд.мм.рррр (наприклад, 26.11.2024). ", "ОК");
            return;
        }

        
        if (!Regex.IsMatch(Time, @"^([0-1][0-9]|2[0-3]):([0-5][0-9])$"))
        {
            await DisplayAlert("Помилка", "Час має бути у форматі гг:хх (наприклад, 12:46). ", "ОК");
            return;
        }

        
        if (Regex.IsMatch(Discipline, @"\d"))
        {
            await DisplayAlert("Помилка", "Назва дисципліни не може містити цифри", "ОК");
            return;
        }

        
        if (Regex.IsMatch(Teacher, @"\d"))
        {
            await DisplayAlert("Помилка", "Ім'я викладача не може містити цифри", "ОК");
            return;
        }

        
        Schelude file = Schelude.GetInstance();
        file.EditLesson(Date, Time, Discipline, Teacher, Audience, Control);

        await DisplayAlert("Успіх", "Дані успішно збережено", "ОК");
        Application.Current.CloseWindow(this.Window);
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Application.Current.CloseWindow(this.Window);
    }
}
