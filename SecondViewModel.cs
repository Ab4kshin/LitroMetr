using System.ComponentModel;

namespace LitroMetr
{
    public class SecondPageViewModel : INotifyPropertyChanged
    {
        private int _menCount;
        private int _womenCount;
        private double _avgMenWeight = 80;
        private double _avgWomenWeight = 65;
        private bool _oschepkovAdded = false;

        public int MenCount
        {
            get { return _menCount; }
            set
            {
                _menCount = value;
                OnPropertyChanged(nameof(MenCount));
                OnPropertyChanged(nameof(TotalCount));
                OnPropertyChanged(nameof(TotalWaterAmountText));
                OnPropertyChanged(nameof(TotalWaterAmount));
            }
        }

        public int WomenCount
        {
            get { return _womenCount; }
            set
            {
                _womenCount = value;
                OnPropertyChanged(nameof(WomenCount));
                OnPropertyChanged(nameof(TotalCount));
                OnPropertyChanged(nameof(TotalWaterAmountText));
                OnPropertyChanged(nameof(TotalWaterAmount));
            }
        }

        public bool OschepkovAdded
        {
            get { return _oschepkovAdded; }
            set
            {
                _oschepkovAdded = value;
                OnPropertyChanged(nameof(OschepkovAdded));
                OnPropertyChanged(nameof(TotalWaterAmountText));
            }
        }

        public int TotalCount
        {
            get { return MenCount + WomenCount; }
        }

        public string TotalWaterAmountText
        {
            get
            {
                double totalWater = TotalWaterAmount;
                string waterText = $"{totalWater:F1} л";
                if (_oschepkovAdded)
                {
                    waterText += " (+Ощепков)";
                }
                return waterText;
            }
        }

        public double TotalWaterAmount
        {
            get
            {
                double menWater = MenCount * _avgMenWeight * 0.035;
                double womenWater = WomenCount * _avgWomenWeight * 0.031;
                double totalWater = menWater + womenWater;

                // Ощепков добавляет фиксированную бутылку, но не влияет на расчет воды
                return totalWater;
            }
        }

        public void ToggleOschepkov()
        {
            OschepkovAdded = !OschepkovAdded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}