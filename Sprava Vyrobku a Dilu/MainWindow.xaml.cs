using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PropertyChanged;
using Sprava_Vyrobku_a_Dilu.Core;
using Sprava_Vyrobku_a_Dilu.Database.Models;
using Sprava_Vyrobku_a_Dilu.Models;
using Sprava_Vyrobku_a_Dilu.Services;

namespace Sprava_Vyrobku_a_Dilu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        private readonly IDbService _dbService;
        public ObservableDataProvider ObservableDataModel { get; set; }
        private readonly PridatDilWindow _pridatDilWindow;
        private readonly PridatVyrobekWindow _pridatVyrobekWindow;
        private readonly UpravitVyrobekWindow _upravitVyrobekWindow;
        private readonly UpravitDilWindow _upravitDilWindow;
        public MainWindow(IDbService dbService,
            ObservableDataProvider visibleDataModel,
            PridatVyrobekWindow pridatVyrobekWindow,
            PridatDilWindow pridatDilWindow,
            UpravitVyrobekWindow upravitVyrobek,
            UpravitDilWindow upravitDilWindow)
        {
            _dbService = dbService;
            ObservableDataModel = visibleDataModel;
            _pridatDilWindow = pridatDilWindow;
            _pridatVyrobekWindow = pridatVyrobekWindow;
            _upravitVyrobekWindow = upravitVyrobek;
            _upravitDilWindow = upravitDilWindow;
            InitializeComponent();
            DataContext = this;
            ObservableDataModel.ViewableVyrobky.CollectionChanged += ViewableVyrobky_CollectionChanged;
            UpdateDily();
        }

        private void ViewableVyrobky_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateDily();
        }

        public int Controlsize { get; set; } = 12;

        public int Controlsize2 { get; set; } = 9;

        public int Controlsize3 { get; set; } = 18;

        public int Heightfix { get; set; } = 130;

        public int Heightfix2 { get; set; } = 400;

        public int ImageHeightFix { get; set; } = 400;
        public int ImageWeightFix { get; set; } = 710;




        private void Exit_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Exit_button.Source = new BitmapImage(new Uri(("pack://application:,,,/img/Exit_pressed.png")));
        }

        private void Exit_button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _pridatDilWindow.Close();
            _pridatVyrobekWindow.Close();
            _upravitVyrobekWindow.Close();
            _upravitDilWindow.Close();
            Close();
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

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
               await ObservableDataModel.Refresh();
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Exception occured on Refresh" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_app(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void NovyDil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = dataGridVyrobky.SelectedItem as VyrobekViewableModel;
                if (data != null)
                {
                    _pridatDilWindow.PrepareAdd(data);
                    _pridatDilWindow.Show();
                }
                else
                {
                    MessageBox.Show("Vyberte výrobek pro přidání dílu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured on NovyDil" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void OdstranitDil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = dataGridDily.SelectedItem as DilModel;
                if (data != null && data is DilModel)
                {
                    await ObservableDataModel.RemoveDil(data);
                    return;
                }

                foreach (var datagrid in FindVisualChildren<DataGrid>(this))
                {
                    if (datagrid.Name == "InnerGrid")
                    {
                        var innterData = datagrid.SelectedItem as DilModel;
                        if (innterData != null && innterData is DilModel)
                        {
                            await ObservableDataModel.RemoveDil(innterData);
                            return;
                        }
                    }
                }
                MessageBox.Show("Vyberte prvek k odstranění.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured on OdstranitDil" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void NovyVyrobek_Click(object sender, RoutedEventArgs e)
        {
            _pridatVyrobekWindow.Show();

        }

        private async void OdstranitVyrobek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = dataGridVyrobky.SelectedItem as VyrobekViewableModel;
                if (data != null)
                {
                    if (!await ObservableDataModel.RemoveVyrobek(data))
                    {
                        MessageBox.Show("Došlo k chybě při pokusu o odstranění výrobku.");
                    }
                }
                else
                {
                    MessageBox.Show("Vyberte prvek k odstranění.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured on OdstranitVyrobek" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        public void UpdateDily()
        {
            var list = ObservableDataModel.ViewableVyrobky.SelectMany(x => x.Dily).ToList();
            dataGridDily.ItemsSource = list;
        }

        private void UpravitVyrobek_Click(object sender, RoutedEventArgs e)
        {
            UpdateVyrobku();
        }

        private void UpdateVyrobku()
        {
            try
            {
                var data = dataGridVyrobky.SelectedItem as VyrobekViewableModel;
                if (data != null)
                {
                    _upravitVyrobekWindow.PrepareEdit(data);
                    _upravitVyrobekWindow.Show();
                }
                else
                {
                    MessageBox.Show("Vyberte prvek k úpravě.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured on UpravitVyrobek" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpravitDil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = dataGridDily.SelectedItem as DilModel;
                if (data != null && data is DilModel)
                {
                    _upravitDilWindow.PrepareEdit(data);
                    _upravitDilWindow.Show();
                    return;
                }

                foreach (var datagrid in FindVisualChildren<DataGrid>(this))
                {
                    if (datagrid.Name == "InnerGrid")
                    {
                        var innterData = datagrid.SelectedItem as DilModel;
                        if (innterData != null && innterData is DilModel)
                        {
                            _upravitDilWindow.PrepareEdit(innterData);
                            _upravitDilWindow.Show();
                            return;
                        }
                    }
                }
                MessageBox.Show("Vyberte prvek k úpravě.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured on UpravitVyrobek" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child == null)
                    {
                        continue;
                    }

                    if (child is T) yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private void dataGridDily_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var data = dataGridDily.SelectedItem as DilModel;
            if (data != null && data is DilModel)
            {
                var vyrobekToSelect = ObservableDataModel.ViewableVyrobky.Where(x => x.VyrobekId == data.VyrobekId).SingleOrDefault();
                if (vyrobekToSelect != null)
                {
                    // Get the ItemsSource of dataGridVyrobky
                    var itemsSource = dataGridVyrobky.ItemsSource as IList<VyrobekViewableModel>;

                    if (itemsSource != null)
                    {
                        // Find the index of the item to select
                        var index = itemsSource.IndexOf(vyrobekToSelect);

                        if (index >= 0)
                        {
                            // Set the selected index and scroll into view
                            dataGridVyrobky.SelectedIndex = index;
                            dataGridVyrobky.ScrollIntoView(vyrobekToSelect);
                        }
                    }
                }
                return;
            }
        }
    }
}