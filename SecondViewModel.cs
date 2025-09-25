using System.ComponentModel;

namespace LitroMetr
{
    public class SecondPageViewModel : INotifyPropertyChanged
    {
        private int _menCount;
        private int _womenCount;
        private double _avgMenWeight = 80; // Средний вес мужчины, кг
        private double _avgWomenWeight = 65; // Средний вес женщины, кг
        private double _additionalWater = 0; // Дополнительная вода
        private bool _oschepkovAdded = false; // Флаг добавления Ощепкова

        public int MenCount
        {
            get { return _menCount; }
            set
            {
                _menCount = value;
                OnPropertyChanged(nameof(MenCount));
                OnPropertyChanged(nameof(TotalCount));
                OnPropertyChanged(nameof(TotalWaterAmountText));
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
            }
        }

        public double AvgMenWeight
        {
            get { return _avgMenWeight; }
            set
            {
                _avgMenWeight = value;
                OnPropertyChanged(nameof(AvgMenWeight));
                OnPropertyChanged(nameof(TotalWaterAmountText));
            }
        }

        public double AvgWomenWeight
        {
            get { return _avgWomenWeight; }
            set
            {
                _avgWomenWeight = value;
                OnPropertyChanged(nameof(AvgWomenWeight));
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
                double menWater = MenCount * AvgMenWeight * 0.035;  // 35 мл на кг для мужчин
                double womenWater = WomenCount * AvgWomenWeight * 0.031;  // 31 мл на кг для женщин
                double totalWater = menWater + womenWater + _additionalWater;

                string waterText = $"{totalWater:F1} л";
                if (_oschepkovAdded)
                {
                    waterText += " (+Ощепков)";
                }

                return waterText;
            }
        }

        public void AddOschepkovWater()
        {
            _additionalWater += 100;
            _oschepkovAdded = true;
            OnPropertyChanged(nameof(TotalWaterAmountText));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
