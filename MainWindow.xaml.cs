using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

namespace LitroMetr
{
    public partial class MainWindow : Window
    {
        private string selectedGender = "Мужчина";
        private double weight = 70;
        private double currentWaterHeight = 0;
        private bool isAnimating = false;

        public MainWindow()
        {
            InitializeComponent();

            // Загружаем изображения
            LoadImages();
            UpdateWaterAmount();
        }

        private void LoadImages()
        {
            try
            {
                // Загружаем изображение воды
                Uri waterUri = new Uri("Images/water.jpg", UriKind.Relative);
                WaterImage.Source = new BitmapImage(waterUri);

                // Загружаем изображение бутылки
                Uri bottleUri = new Uri("Images/bottle.png", UriKind.Relative);
                BottleImage.Source = new BitmapImage(bottleUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображений: {ex.Message}", "Ошибка");
            }
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            IndividualContent.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new SecondPage());
        }

        private void IndividualButton_Click(object sender, RoutedEventArgs e)
        {
            IndividualContent.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(null);
        }

        private void MaleButton_Click(object sender, RoutedEventArgs e)
        {
            selectedGender = "Мужчина";
            UpdateWaterAmount();
        }

        private void FemaleButton_Click(object sender, RoutedEventArgs e)
        {
            selectedGender = "Женщина";
            UpdateWaterAmount();
        }

        private void mySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider)
            {
                weight = slider.Value;
                UpdateWaterAmount();
            }
        }

        private void UpdateWaterAmount()
        {
            double waterAmount = CalculateWaterAmount(selectedGender, weight);
            if (WaterAmountLabel != null)
            {
                WaterAmountLabel.Content = $"{waterAmount:F1} л";
                UpdateWaterLevel(waterAmount);
            }
        }

        private double CalculateWaterAmount(string gender, double weight)
        {
            double coefficient = (gender == "Мужчина") ? 0.035 : 0.031;
            return weight * coefficient;
        }

        private void UpdateWaterLevel(double waterAmount)
        {
            // Максимальное количество воды
            double maxWater = 4.2;
            double proportion = waterAmount / maxWater;
            proportion = Math.Max(0, Math.Min(1, proportion));

            // Новая высота воды
            double targetHeight = proportion * 400;

            // Устанавливаем высоту без анимации для мгновенного отклика
            WaterContainer.Height = targetHeight;
            currentWaterHeight = targetHeight;
        }


        private void SmoothWaterLevelChange(double targetHeight)
        {
            // Если анимация уже выполняется, не запускаем новую
            if (isAnimating)
                return;

            // Разница между текущей и целевой высотой
            double heightDifference = Math.Abs(targetHeight - currentWaterHeight);

            // Если изменение очень маленькое - мгновенно
            if (heightDifference < 5)
            {
                WaterContainer.Height = targetHeight;
                currentWaterHeight = targetHeight;
                return;
            }

            // Запускаем анимацию
            isAnimating = true;

            // Длительность анимации зависит от расстояния
            double durationSeconds = Math.Min(0.8, heightDifference / 400 * 1.5);

            var animation = new DoubleAnimation(
                targetHeight,
                TimeSpan.FromSeconds(durationSeconds))
            {
                EasingFunction = new QuadraticEase()
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };

            animation.Completed += (s, e) =>
            {
                currentWaterHeight = targetHeight;
                isAnimating = false;
            };

            WaterContainer.BeginAnimation(Grid.HeightProperty, animation);
        }
    }
}