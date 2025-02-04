using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;

namespace Audioguide.Kurs.Maui
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Tour> _tours = new();

        public MainPage()
        {
            InitializeComponent();
            _tours = LoadTours();
            ToursListView.ItemsSource = _tours;
        }

        /// <summary>
        /// Загрузка списка туров из сборки
        /// Симуляция запросов к апи (чтобы не писать веб-сервер и работать с БД)
        /// Файл tours.json хранится в /Resources/raw/
        /// Не обязательно заполнять, однако необходимо указать, что была использована эта 'фишка'
        /// </summary>
        /// <returns>Список туров, доступных пользователю</returns>
        private ObservableCollection<Tour> LoadTours()
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("tours.json").Result;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            return JsonSerializer.Deserialize<ObservableCollection<Tour>>(json);
        }

        /// <summary>
        /// Хендлер для обработки ввода в поле поиска + реализация поиска
        /// </summary>
        /// <param name="sender">не используется</param>
        /// <param name="e">Аргументы события</param>
        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var query = e.NewTextValue.ToLower();
            try
            {
                _tours.Clear();
                var filtered = LoadTours().Where(t => t.Name.ToLower().Contains(query));

                foreach (Tour tour in filtered)
                    _tours.Add(tour);
            }catch(Exception ex)
            {
                await DisplayAlert("Ошибка!", "Ошибка при поиске по доступным турам!", "Увы...");
            }
        }

        /// <summary>
        /// Функция открытия проигрывателя тура при нажатии на соответствующий пункт меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnTourSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Tour tour)
                await Navigation.PushAsync(new RoutePage(tour));
            ((ListView)sender).SelectedItem = null;
        }
    }
}