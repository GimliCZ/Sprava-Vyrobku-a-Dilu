using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SpravaVyrobkuaDilu.Extensions;
using SpravaVyrobkuaDilu.Core;
using SpravaVyrobkuaDilu.Database.Models;
using SpravaVyrobkuaDilu.Models;

namespace SpravaVyrobkuaDilu
{
    /// <summary>
    /// Interakční logika pro PridatDilWindow.xaml
    /// </summary>
    public partial class PridatDilWindow : Window
    {
        private readonly ObservableDataProvider _observableDataProvider;
        public PridatDilWindow(ObservableDataProvider observableDataProvider)
        {
            InitializeComponent();
            _observableDataProvider = observableDataProvider;
        }

        public int EditedVyrobekId = 0;


        #region Vizual

        private readonly NumberFormatInfo numberFormat = new()
        {

            NumberDecimalSeparator = ".",
            NumberDecimalDigits = 4
        };

        private void Exit_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Exit_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Exit_pressed.png")));
        }

        private void Exit_button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PostAdd();
            Hide();
        }

        private void Exit_button_MouseLeave(object sender, MouseEventArgs e)
        {
            Exit_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Exit_default.png"))); ;
        }

        private void Exit_button_MouseEnter(object sender, MouseEventArgs e)
        {
            Exit_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Exit_howered.png")));
        }

        private void Maximize_button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (WindowState.Normal == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Maximize_howered.png")));
            }
            if (WindowState.Maximized == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/ReturnMaximize_hower.png")));
            }
        }


        private void Maximize_button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (WindowState.Normal == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Maximize_default.png")));
            }
            if (WindowState.Maximized == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/ReturnMaximize_default.png")));
            }
        }

        private void Maximize_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState.Normal == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Maximize_pressed.png")));
            }
            if (WindowState.Maximized == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/ReturnMaximize_pressed.png")));
            }
        }
        readonly CustomTimer customTimer = new();
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var currentstate = WindowState;

            if (e.LeftButton == MouseButtonState.Pressed && Dragfield.IsMouseOver == true)
            {
                DragMove();
                if (currentstate == WindowState.Maximized)
                {

                    // Application.Current.MainWindow.WindowState = WindowState.Normal;
                    Point locationFromScreen = this.Dragfield.PointToScreen(new Point(0, 0));
                    PresentationSource source = PresentationSource.FromVisual(this);
                    System.Windows.Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                    Point temp = e.GetPosition(this);
                    WindowState = WindowState.Normal;
                    Left = targetPoints.X + temp.X / 2;
                    Top = targetPoints.Y + temp.Y / 2;
                    DragMove();
                }
            }
            if (customTimer.Running == false)
            {
                customTimer.Start_timer();
            }
            else
            {
                if (!customTimer.Timer_enlapsed(0.3))
                {
                    bool changedone = false;
                    if (WindowState.Normal == WindowState && changedone == false)
                    {
                        Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/ReturnMaximize_default.png")));
                        WindowState = WindowState.Maximized;
                        changedone = true;
                    }
                    if (WindowState.Maximized == WindowState && changedone == false)
                    {
                        Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Maximize_default.png")));
                        WindowState = WindowState.Normal;

                    }
                }
                customTimer.Stop_timer();
                customTimer.Start_timer();
            }
        }
        private void Maximize_button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool changedone = false;
            if (WindowState.Normal == WindowState && changedone == false)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/ReturnMaximize_default.png")));
                WindowState = WindowState.Maximized;
                changedone = true;
            }
            if (WindowState.Maximized == WindowState && changedone == false)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Maximize_default.png")));
                WindowState = WindowState.Normal;

            }

        }

        private void Minimize_button_MouseEnter(object sender, MouseEventArgs e)
        {
            Minimize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Minimize_howered.png")));
        }

        private void Minimize_button_MouseLeave(object sender, MouseEventArgs e)
        {
            Minimize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Minimize_default.png")));
        }

        private void Minimize_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Minimize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Minimize_pressed.png")));
        }

        private void Minimize_button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Minimize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Minimize_default.png")));
            WindowState = WindowState.Minimized;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState.Maximized == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/ReturnMaximize_default.png")));
            }

            if (WindowState.Normal == WindowState)
            {
                Maximize_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Maximize_default.png")));
            }
        }
        #endregion


        public void PrepareAdd(VyrobekViewableModel model)
        {
            EditedVyrobekId = model.VyrobekId;
            NazevVyrobek.Text = model.Nazev;
            CenaVyrobek.Text = model.Cena.ToString(numberFormat);
            PopisVyrobek.Text = model.Popis;
        }

        public void PostAdd()
        {
            EditedVyrobekId = 0;
            NazevVyrobek.Text = string.Empty;
            CenaVyrobek.Text = string.Empty;
            PopisVyrobek.Text = string.Empty;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Sanitize inputs by trimming whitespace
                var nazevVyrobek = NazevVyrobek.Text.Trim();
                var cenaVyrobek = CenaVyrobek.Text.Trim();
                var popisVyrobek = PopisVyrobek.Text.Trim();

                // Validation
                if (string.IsNullOrWhiteSpace(nazevVyrobek))
                {
                    MessageBox.Show("Nazev is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                // Parse the decimal using the custom NumberFormatInfo
                if (!decimal.TryParse(cenaVyrobek, NumberStyles.Number, numberFormat, out var cenaVyrobekVerif))
                {
                    MessageBox.Show("Cena must be a valid number with a decimal point.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var NewDil = new DilModel(nazevVyrobek, DecimalExtensions.RoundUp(cenaVyrobekVerif, 4), EditedVyrobekId)
                {
                    Popis = popisVyrobek,
                    Upraveno = DateTime.Now
                };

                if (!await _observableDataProvider.AddDil(NewDil))
                {
                    MessageBox.Show("Error occured during add Dil operation", "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception just occurred: " + ex.Message, "Exception ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CenaVyrobek_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CenaVyrobek.Text.Contains(','))
            {
                CenaVyrobek.Text = CenaVyrobek.Text.Replace(',', '.');
            }
            if (CenaVyrobek.Text.Length > 7 && CenaVyrobek.Text[7] != '.' && !CenaVyrobek.Text.Substring(0, 7).Contains('.'))
            {
                CenaVyrobek.Text = CenaVyrobek.Text.ReplaceAt('.', 7);
                CenaVyrobek.CaretIndex = 8;
            }
            CenaVyrobek.Text = CenaVyrobek.Text.Replace("..", ".0");
        }
    }
}
