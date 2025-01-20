using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace testProject
{
    public class ObjectData : INotifyPropertyChanged
    {
        private string? _name;
        private double _horizontalCoordinate;
        private double _verticalCoordinate;
        private double _horizontalSize;
        private double _verticalSize;
        private bool _isDefect;
         
        public string? Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public double Distance
        {
            get => _horizontalCoordinate;
            set { _horizontalCoordinate = value; OnPropertyChanged(); }
        }

        public double Angle
        {
            get => _verticalCoordinate;
            set { _verticalCoordinate = value; OnPropertyChanged(); }
        }

        public double Width
        {
            get => _horizontalSize;
            set { _horizontalSize = value; OnPropertyChanged(); }
        }

        public double Heigth
        {
            get => _verticalSize;
            set { _verticalSize = value; OnPropertyChanged(); }
        }

        public bool IsDefect
        {
            get => _isDefect;
            set { _isDefect = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}