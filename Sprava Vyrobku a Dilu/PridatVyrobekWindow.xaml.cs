using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Sprava_Vyrobku_a_Dilu.Extensions;
using SpravaVyrobkuaDilu.Core;
using SpravaVyrobkuaDilu.Database.Models;
using SpravaVyrobkuaDilu.Models;

namespace SpravaVyrobkuaDilu
{
    /// <summary>
    /// Interakční logika pro PridatVyrobekWindow.xaml
    /// </summary>
    public partial class PridatVyrobekWindow : Window
    {
        private readonly ObservableDataProvider _observableDataProvider;
        public PridatVyrobekWindow(ObservableDataProvider observableDataProvider)
        {
            _observableDataProvider = observableDataProvider;
            InitializeComponent();
        }

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
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Sanitize inputs by trimming whitespace
                var nazevVyrobek = NazevVyrobek.Text.Trim();
                var cenaVyrobek = CenaVyrobek.Text.Trim();
                var popisVyrobek = PopisVyrobek.Text.Trim();
                var poznamkaVyrobek = PoznamkaVyrobek.Text.Trim();

                // Validation
                if (string.IsNullOrWhiteSpace(nazevVyrobek))
                {
                    MessageBox.Show("Nazev is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check for duplicate name
                if (_observableDataProvider.IsPresent(nazevVyrobek))
                {
                    var result = MessageBox.Show("A Vyrobek with this name already exists. Do you want to add it anyway?",
                                                 "Duplicate Found",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                // Parse the decimal using the custom NumberFormatInfo
                if (!decimal.TryParse(cenaVyrobek, NumberStyles.Number, numberFormat, out var cenaVyrobekVerif))
                {
                    MessageBox.Show("Cena must be a valid number with a decimal point.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create new VyrobekModel
                var newVyrobek = new VyrobekModel(nazevVyrobek, DecimalExtensions.RoundUp(cenaVyrobekVerif, 4))
                {
                    Popis = popisVyrobek,
                    Poznamka = poznamkaVyrobek,
                    Upraveno = DateTime.Now
                };
                var NewDil = new DilModel(nazevVyrobek, DecimalExtensions.RoundUp(cenaVyrobekVerif, 4), newVyrobek.VyrobekId)
                {
                    Popis = popisVyrobek,
                    Upraveno = DateTime.Now
                };
                var listDil = new List<DilModel>() { NewDil };

                if (!await _observableDataProvider.AddVyrobekAndDily(newVyrobek, listDil))
                {
                    MessageBox.Show("Error occured during Add Vyrobek And Dily operation", "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
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
