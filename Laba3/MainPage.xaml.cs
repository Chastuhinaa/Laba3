using Microsoft.Maui.Controls;
using System.Text.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;



namespace Laba3
{
    public partial class MainPage : ContentPage
    {
        private FileManager fileManager;
        private Schelude fileObject;
        public ObservableCollection<Lesson> LessonList { get; set; }

        private bool mainPageLocker = false;

        public MainPage()
        {
            InitializeComponent();

            fileManager = new FileManager();
            fileObject = Schelude.GetInstance();

            UpdateArticlesView();
        }

        private async void SaveClicked(object sender, EventArgs e)
        {
            if (!mainPageLocker || fileObject.Data == null || fileObject.Data.Count == 0)
            {
                await DisplayAlert("Помилка", "Немає даних для збереження або файл не відкрито.", "OK");
                return;
            }

            try
            {
                fileManager.SaveFile();
                await DisplayAlert("Успіх", "Файл успішно збережено.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Помилка під час збереження файлу: {ex.Message}", "OK");
            }
        }


        private void OpenWindow(Page view)
        {
            Window editWindow = new Window(view);
            Application.Current.OpenWindow(editWindow);
        }

        private void EditClicked(object sender, EventArgs e)
        {
            UpdateArticlesView();
            if (mainPageLocker)
            {
                OpenWindow(new EditView());
                UpdateArticlesView();
            }
            else
            {
                DisplayAlert("Помилка", "Неможливо редагувати порожній файл", "ОК");
            }
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            if (mainPageLocker)
            {
                fileObject.DeleteArticle();
                UpdateArticlesView();
            }
            else
            {
                DisplayAlert("Помилка", "Не залишилося елементівt", "ОК");
            }
        }

        private void UpdateArticlesView()
        {
            this.BindingContext = fileObject.Data;
            if (fileObject.Data.Count > 0)
            {
                OnPropertyChanged(nameof(fileObject.Data));
                mainPageLocker = true;
            }
            else
            {
                mainPageLocker = false;
            }
        }

        private void AddClicked(object sender, EventArgs e)
        {
            if (mainPageLocker || this.fileObject.IsOpened())
            {
                OpenWindow(new AddView());
                UpdateArticlesView();
            }
            else
            {
                DisplayAlert("Помилка", "Спочатку відкрийте файл", "OK");
            }
        }

        private void AboutClicked(object sender, EventArgs e)
        {
            OpenWindow(new AboutView());
        }

        private void OpenJsonFileClicked(object sender, EventArgs e)
        {
            try
            {
                if (fileManager.OpenFile("D:\\Laba_3\\Laba3\\jsonka.json"))
                {
                    DisplayAlert("Успішно", "Файл успішно відкрито", "OK");
                    UpdateArticlesView();
                    mainPageLocker = true;
                }
                else
                {
                    DisplayAlert("Помилка", "Помилка читання файлу. Виберіть інший", "OK");
                    mainPageLocker = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", "Неможливо відкрити файл: " + ex.Message, "OK");
            }
        }
        private async void ExitClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Підтвердити вихід", "Ви впевнені, що хочете вийти?", "Так", "Ні");
            if (answer)
            {
                Application.Current.Quit();
            }
        }

        private void SearchBackClicked(object sender, EventArgs e)
        {
            fileObject.UpdateDataToBuffer();
            UpdateArticlesView();
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            
            this.BindingContext = fileObject.Search(searchPicker.SelectedItem?.ToString(), searchEntry.Text);
        }
       
    }
}