using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LitroMetr
{
    public partial class MainWindow : Window
    {
        private string selectedGender = "Мужчина";
        private double weight = 70;

        public MainWindow()
        {
            InitializeComponent();
            UpdateWaterAmount();
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу группового питья
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new SecondPage());
        }

        private void IndividualButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для кнопки "Индивидуальное питьё"
            MainFrame.Visibility = Visibility.Hidden;
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

                // Обновляем уровень воды в бутылке
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
            // Максимальное количество воды (для веса 120 кг и пола мужской: 120 * 0.035 = 4.2 л)
            double maxWater = 4.2;

            // Рассчитываем пропорцию (от 0 до 1)
            double proportion = waterAmount / maxWater;

            // Ограничиваем значение от 0 до 1
            proportion = Math.Max(0, Math.Min(1, proportion));

            // Устанавливаем высоту контейнера воды
            if (WaterContainer != null)
            {
                // Высота контейнера воды будет пропорциональна количеству воды
                // При proportion=0 высота=0 (вода не видна)
                // При proportion=1 высота=300 (вода заполняет всю бутылку)
                WaterContainer.Height = proportion * 300;
            }
        }
    }
}