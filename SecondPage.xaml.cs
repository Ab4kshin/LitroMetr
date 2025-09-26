using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace LitroMetr
{
    public partial class SecondPage : Page
    {
        public SecondPageViewModel ViewModel { get; private set; }
        private string projectPath;

        public SecondPage()
        {
            InitializeComponent();
            ViewModel = new SecondPageViewModel();
            this.DataContext = ViewModel;

            // Получаем путь к папке проекта
            projectPath = Directory.GetCurrentDirectory();

            // Подписываемся на изменения
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.PreviewKeyDown += SecondPage_PreviewKeyDown;

            // Инициализируем отображение бутылок
            UpdateBottlesDisplay();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.TotalWaterAmount) ||
                e.PropertyName == nameof(ViewModel.OschepkovAdded))
            {
                UpdateBottlesDisplay();
            }
        }

        private ImageSource LoadImage(string fileName)
        {
            try
            {
                // Пробуем разные пути
                string[] possiblePaths = {
                    Path.Combine(projectPath, "Images", fileName),
                    Path.Combine(projectPath, fileName),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
                };

                foreach (string path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(path, UriKind.Absolute);
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        return image;
                    }
                }

                // Если файл не найден, создаем пустое изображение для отладки
                return CreatePlaceholderImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения {fileName}: {ex.Message}", "Ошибка");
                return CreatePlaceholderImage();
            }
        }

        private ImageSource CreatePlaceholderImage()
        {
            // Создаем простой прямоугольник для отладки
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Blue, new Pen(Brushes.Black, 1), new System.Windows.Rect(0, 0, 60, 100));
                drawingContext.DrawText(
                    new FormattedText("Bottle",
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    System.Windows.FlowDirection.LeftToRight,
                                    new Typeface("Arial"),
                                    12,
                                    Brushes.White),
                    new System.Windows.Point(5, 40));
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap(60, 100, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }

        private void UpdateBottlesDisplay()
        {
            // Очищаем контейнеры
            NormalBottlesContainer.Children.Clear();
            OschepkovContainer.Children.Clear();

            // Добавляем обычные бутылки (1 бутылка = 5 литров)
            int numberOfBottles = (int)ViewModel.TotalWaterAmount / 5;

            for (int i = 0; i < numberOfBottles; i++)
            {
                var image = new Image();
                image.Source = LoadImage("big_bottle.jpg");
                image.Width = 60;
                image.Height = 100;
                image.Margin = new Thickness(5, 0, 5, 0);
                image.Stretch = Stretch.Uniform;

                NormalBottlesContainer.Children.Add(image);
            }

            // Добавляем бутылку Ощепкова если она активирована
            if (ViewModel.OschepkovAdded)
            {
                var oschepkovImage = new Image();
                oschepkovImage.Source = LoadImage("bottle_oshepkov.png");
                oschepkovImage.Width = 80;
                oschepkovImage.Height = 140;
                oschepkovImage.Margin = new Thickness(10, 0, 10, 0);
                oschepkovImage.Stretch = Stretch.Uniform;

                OschepkovContainer.Children.Add(oschepkovImage);
            }

            // Обновляем текстовую информацию
            DebugInfoTextBlock.Text = $"Всего воды: {ViewModel.TotalWaterAmount:F1} л, Бутылок: {numberOfBottles} (по 5л каждая)";
        }

        private void SecondPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ViewModel.ToggleOschepkov();
                e.Handled = true;
            }
        }

        private void Individual_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(null);
                mainWindow.MainFrame.Visibility = Visibility.Hidden;
            }
        }

        private void Group_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Общее количество воды для группы: {ViewModel.TotalWaterAmountText}");
        }

        private void MenSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                ViewModel.MenCount = (int)slider.Value;
            }
        }

        private void WomenSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                ViewModel.WomenCount = (int)slider.Value;
            }
        }
    }
}