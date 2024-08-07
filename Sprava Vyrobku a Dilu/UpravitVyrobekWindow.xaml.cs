using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sprava_Vyrobku_a_Dilu.Core;
using Sprava_Vyrobku_a_Dilu.Database.Models;
using Sprava_Vyrobku_a_Dilu.Models;

namespace Sprava_Vyrobku_a_Dilu
{
    /// <summary>
    /// Interakční logika pro UpravitVyrobek.xaml
    /// </summary>
    public partial class UpravitVyrobekWindow : Window
    {
        private ObservableDataProvider _observableDataModel;
        public UpravitVyrobekWindow(ObservableDataProvider observableDataModel)
        {
            InitializeComponent();
            _observableDataModel = observableDataModel;
        }

        #region Vizual

        private NumberFormatInfo numberFormat = new()
        {
            NumberDecimalSeparator = ".",
            NumberDecimalDigits = 4
        };
        public int Controlsize { get; set; } = 12;

        public int Controlsize2 { get; set; } = 9;

        public int Controlsize3 { get; set; } = 18;

        public int Heightfix { get; set; } = 130;

        public int Heightfix2 { get; set; } = 400;

        public int ImageHeightFix { get; set; } = 400;
        public int ImageWeightFix { get; set; } = 710;

        public int EditedId = 0;

        private void Exit_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Exit_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Exit_pressed.png")));
        }

        private void Exit_button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PostEdit();
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
        CustomTimer customTimer = new CustomTimer();
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

            int rozsah_min = 12;//modifikace fontu
            int rozsah_max = 14;
            int rozsah_min2 = 10;
            int rozsah_max2 = 14;
            Controlsize = Convert.ToInt32(e.NewSize.Width - 800) * (rozsah_max - rozsah_min) / (1920 - 800) + rozsah_min;
            Controlsize2 = Convert.ToInt32((e.NewSize.Width - 800) * (rozsah_max2 - rozsah_min2) / (1920 - 800) + rozsah_min2);
            Controlsize3 = Controlsize2 * 2;
            var heightfixtemp = Convert.ToInt32((e.NewSize.Height - 600) * (610 - 130) / (1080 - 600) + 130); //modifikace velikosti čítače naměřených hodnot
            var imageHeightFix = Convert.ToInt32((e.NewSize.Height - 600) * (975 - 500) / (1080 - 600) + 500);
            if (heightfixtemp <= 0)
            {
                Heightfix = 0;
                ImageHeightFix = 0;
                ImageWeightFix = 0;
            }
            else
            {
                ImageHeightFix = imageHeightFix;
                ImageWeightFix = imageHeightFix * 16 / 9;
                Heightfix = heightfixtemp;
                Heightfix2 = heightfixtemp + 270;
            }
        }
        #endregion
        public void PrepareEdit(VyrobekViewableModel model)
        {
            EditedId = model.VyrobekId;
            NazevVyrobek.Text = model.Nazev;
            CenaVyrobek.Text = model.Cena.ToString(numberFormat);
            PopisVyrobek.Text = model.Popis;
            PoznamkaVyrobek.Text = model.Poznamka;
        }

        public void PostEdit()
        {
            EditedId = 0;
            NazevVyrobek.Text = string.Empty;
            CenaVyrobek.Text = string.Empty;
            PopisVyrobek.Text = string.Empty;
            PoznamkaVyrobek.Text = string.Empty;
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
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

                // Create a custom NumberFormatInfo with dot as the decimal separator
                var numberFormat = new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                };

                // Parse the decimal using the custom NumberFormatInfo
                if (!decimal.TryParse(cenaVyrobek, NumberStyles.Number, numberFormat, out var cenaVyrobekVerif))
                {
                    MessageBox.Show("Cena must be a valid number with a decimal point.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create new VyrobekModel
                var newVyrobek = new VyrobekModel(nazevVyrobek, cenaVyrobekVerif)
                {
                    VyrobekId = EditedId,
                    Popis = popisVyrobek,
                    Poznamka = poznamkaVyrobek,
                    Upraveno = DateTime.Now
                };

                if (!await _observableDataModel.UpdateVyrobek(newVyrobek))
                {
                    MessageBox.Show("Error occured during Update Vyrobek operation", "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception just occurred: " + ex.Message, "Exception ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CenaVyrobek_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CenaVyrobek.Text.Contains(","))
            {
                CenaVyrobek.Text = CenaVyrobek.Text.Replace(",", ".");
            }
        }
    }
}