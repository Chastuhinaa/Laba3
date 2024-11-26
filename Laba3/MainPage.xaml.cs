using System.Collections.ObjectModel;



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

        private async void DeleteClicked(object sender, EventArgs e)
        {
            if (mainPageLocker)
            {
                bool isConfirmed = await DisplayAlert(
                    "Підтвердження",
                    "Ви впевнені, що хочете видалити цей елемент?",
                    "Так",
                    "Ні"
                );

                if (isConfirmed)
                {
                    fileObject.DeleteArticle();
                    UpdateArticlesView();
                }
            }
            else
            {
                await DisplayAlert("Помилка", "Не залишилося елементів", "ОК");
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

        private async void OpenJsonFileClicked(object sender, EventArgs e)
        {
            try
            {
               
                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Виберіть JSON-файл",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".json" } },
                { DevicePlatform.MacCatalyst, new[] { "json" } },
                { DevicePlatform.iOS, new[] { "json" } },
                { DevicePlatform.Android, new[] { "application/json" } },
            })
                });

                if (fileResult != null)
                {
                    var filePath = fileResult.FullPath;

                    if (fileManager.OpenFile(filePath))
                    {
                        await DisplayAlert("Успішно", "Файл успішно відкрито.", "OK");
                        UpdateArticlesView();
                        mainPageLocker = true;
                    }
                    else
                    {
                        await DisplayAlert("Помилка", "Помилка читання файлу. Виберіть інший.", "OK");
                        mainPageLocker = false;
                    }
                }
                else
                {
                    await DisplayAlert("Скасовано", "Файл не було обрано.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", "Неможливо відкрити файл: " + ex.Message, "OK");
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
