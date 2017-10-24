using System;
using System.ComponentModel;
using System.Windows.Media;

namespace VisiMatrix
{
    public class MatrixHolder: INotifyPropertyChanged
    {
        private Matrix _matrix;
        private bool _isDirty;

        public Matrix Matrix
        {
            get { return _matrix; }
            private set { _matrix = value; OnMatrixChanged(); }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                OnPropertyChanged("IsDirty");
            }
        }

        public MatrixHolder()
        {
            Matrix = new Matrix();
        }

        public void Rotate(Double x)
        {
            _matrix.Rotate(x); OnMatrixChanged();
        }

        public void Scale(Double x, Double y)
        {
            _matrix.Scale(x, y); OnMatrixChanged();
        }

        public void Skew(Double x, Double y)
        {
            _matrix.Skew(x, y); OnMatrixChanged();
        }

        public void Translate(Double x, Double y)
        {
            _matrix.Translate(x, y); OnMatrixChanged();
        }

        public void Set(Matrix matrix)
        {
            Matrix = matrix;
            IsDirty = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnMatrixChanged()
        {
            OnPropertyChanged("Matrix");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
