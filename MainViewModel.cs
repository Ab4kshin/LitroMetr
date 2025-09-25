using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LitroMetr
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _myValue;

        public double MyValue
        {
            get => _myValue;
            set
            {
                if (_myValue != value)
                {
                    _myValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
