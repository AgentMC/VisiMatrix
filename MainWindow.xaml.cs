using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using VisiMatrix.FileFormat;

namespace VisiMatrix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private long _matrixLocation;
        private string _fileName;

        private static MatrixHolder Holder
        {
            get { return (MatrixHolder) Application.Current.Resources["Holder"]; }
        }

        private void BtnRotate_Click(object sender, RoutedEventArgs e)
        {
            Holder.Rotate(Fixed.Parse(BoxRotate));
        }

        private void BtnTranslate_Click(object sender, RoutedEventArgs e)
        {
            Holder.Translate(Fixed.Parse(BoxTranslateX), Fixed.Parse(BoxTranslateY));
        }

        private void BtnSkew_Click(object sender, RoutedEventArgs e)
        {
            Holder.Skew(Fixed.Parse(BoxSkewX), Fixed.Parse(BoxSkewY));
        }

        private void BtnScale_Click(object sender, RoutedEventArgs e)
        {
            Holder.Scale(Fixed.Parse(BoxScaleX), Fixed.Parse(BoxScaleY));
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Holder.Set(new Matrix());
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog {Multiselect = false, Filter = "MP4 files|*.mp4", Title = "Open an MP4 file"};
            if (open.ShowDialog(this) == true)
            {
                using (var stream = new BigEndianBinaryReader(open.OpenFile(), Encoding.Default))
                {
                    Atom.SeekToChild(stream, "moov");
                    Atom.SeekToChild(stream, "trak");
                    Atom.SeekToChild(stream, "tkhd");
                    var header = new TrackHeader(stream);
                    BoxWidth.Text = (header.Width/5).ToString(CultureInfo.CurrentUICulture);
                    BoxHeight.Text = (header.Heigth/5).ToString(CultureInfo.CurrentUICulture);
                    Holder.Set(header.Matrix.ToMatrix());
                    _matrixLocation = header.Matrix.Location;
                    _fileName = open.FileName;
                }
                if (OptPro.IsChecked == true)
                {
                    Media.Stop();
                    Media.Close();
                    BlueForeground.Visibility = Visibility.Hidden;
                    Media.Source = new Uri(_fileName);
                    Media.Play();
                }
            }

        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Media.Stop();
            Media.Close();
            BlueForeground.Visibility = Visibility.Visible;
            Label label;
            try
            {
                using (var stream = new BinaryWriter(new FileStream(_fileName, FileMode.Open)))
                {
                    stream.BaseStream.Seek(_matrixLocation, SeekOrigin.Begin);
                    var appleMatrix = new AppleMatrix(Holder.Matrix);
                    appleMatrix.Write(stream);
                }
                label = SaveLabel;
            }
            catch
            {
                label = SaveFailLabel;
            }
            label.Visibility = Visibility.Visible;
            await Task.Delay(3000);
            label.Visibility = Visibility.Hidden;
        }

        private void OptPro_Checked(object sender, RoutedEventArgs e)
        {
            if(!IsLoaded) return;

            var proVisible = OptPro.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            var liteVisible = OptPro.IsChecked == false ? Visibility.Visible : Visibility.Collapsed;

            TransformLabel1.Visibility = proVisible;
            BoxM11.Visibility = proVisible;
            BoxM12.Visibility = proVisible;
            BoxM21.Visibility = proVisible;
            BoxM22.Visibility = proVisible;
            BoxTx.Visibility = proVisible;
            BoxTy.Visibility = proVisible;
            TransformLabel2.Visibility = proVisible;
            BoxRotate.Visibility = proVisible;
            BtnRotate.Visibility = proVisible;
            ScaleBoxes.Visibility = proVisible;
            BtnScale.Visibility = proVisible;
            SkewBoxes.Visibility = proVisible;
            BtnSkew.Visibility = proVisible;
            TranslateBoxes.Visibility = proVisible;
            BtnTranslate.Visibility = proVisible;

            BtnLiteRot90.Visibility = liteVisible;
            BtnLiteRot180.Visibility = liteVisible;
            BtnLiteRot270.Visibility = liteVisible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OptLite.IsChecked = true;
        }

        private void BtnLiteRot90_Click(object sender, RoutedEventArgs e)
        {
            BtnReset_Click(sender, e);
            Holder.Rotate(90);
        }

        private void BtnLiteRot180_Click(object sender, RoutedEventArgs e)
        {
            BtnReset_Click(sender, e);
            Holder.Rotate(180);
        }

        private void BtnLiteRot270_Click(object sender, RoutedEventArgs e)
        {
            BtnReset_Click(sender, e);
            Holder.Rotate(270);
        }
    }
}
