using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LitroMetr
{
    public partial class SecondPage : Page
    {
        public SecondPageViewModel ViewModel { get; private set; }

        public SecondPage()
        {
            InitializeComponent();
            ViewModel = new SecondPageViewModel();
            this.DataContext = ViewModel;

            // Подписываемся на событие PreviewKeyDown для обработки горячих клавиш
            this.PreviewKeyDown += SecondPage_PreviewKeyDown;
        }

        private void SecondPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем нажатие Ctrl+O
            if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ViewModel.AddOschepkovWater();
                e.Handled = true;
            }
        }

        private void Individual_Click(object sender, RoutedEventArgs e)
        {
            // Получаем родительское окно
            Window parentWindow = Window.GetWindow(this);

            // Если это MainWindow, возвращаемся к основному содержимому
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
