using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PropertyChanged;
using SpravaVyrobkuaDilu.Core;
using SpravaVyrobkuaDilu.Database.Models;
using SpravaVyrobkuaDilu.Models;

namespace SpravaVyrobkuaDilu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        public ObservableDataProvider ObservableDataProvider { get; set; }
        private readonly PridatDilWindow _pridatDilWindow;
        private readonly PridatVyrobekWindow _pridatVyrobekWindow;
        private readonly UpravitVyrobekWindow _upravitVyrobekWindow;
        private readonly UpravitDilWindow _upravitDilWindow;
        public MainWindow(ObservableDataProvider observableDataProvider,
            PridatVyrobekWindow pridatVyrobekWindow,
            PridatDilWindow pridatDilWindow,
            UpravitVyrobekWindow upravitVyrobek,
            UpravitDilWindow upravitDilWindow)
        {
            ObservableDataProvider = observableDataProvider;
            _pridatDilWindow = pridatDilWindow;
            _pridatVyrobekWindow = pridatVyrobekWindow;
            _upravitVyrobekWindow = upravitVyrobek;
            _upravitDilWindow = upravitDilWindow;
            InitializeComponent();
            DataContext = this;
            ObservableDataProvider.ViewableVyrobky.CollectionChanged += ViewableVyrobky_CollectionChanged;
            UpdateDily();
        }

        private void ViewableVyrobky_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateDily();
        }

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

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ObservableDataProvider.Refresh();
            }
            catch (Exception ex)
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
            _pridatDilWindow.Close();
            _pridatVyrobekWindow.Close();
            _upravitVyrobekWindow.Close();
            _upravitDilWindow.Close();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void NovyDil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGridVyrobky.SelectedItem is (VyrobekViewableModel data and not null))
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
                if (data is (not null and DilModel))
                {
                    await ObservableDataProvider.RemoveDil(data);
                    return;
                }

                foreach (var datagrid in FindVisualChildren<DataGrid>(this))
                {
                    if (datagrid.Name == "InnerGrid")
                    {
                        var innterData = datagrid.SelectedItem as DilModel;
                        if (innterData is (not null and DilModel))
                        {
                            await ObservableDataProvider.RemoveDil(innterData);
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
                if (dataGridVyrobky.SelectedItem is (VyrobekViewableModel data and not null))
                {
                    if (!await ObservableDataProvider.RemoveVyrobek(data))
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
            var list = ObservableDataProvider.ViewableVyrobky.SelectMany(x => x.Dily).ToList();
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
                if (dataGridVyrobky.SelectedItem is (VyrobekViewableModel data and not null))
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
                if (data is (not null and DilModel))
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
                        if (innterData is (not null and DilModel))
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

                    if (child is T t) yield return t;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private void DataGridDily_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var data = dataGridDily.SelectedItem as DilModel;
            if (data is (not null and DilModel))
            {
                var vyrobekToSelect = ObservableDataProvider.ViewableVyrobky.Where(x => x.VyrobekId == data.VyrobekId).SingleOrDefault();
                if (vyrobekToSelect != null)
                {
                    // Get the ItemsSource of dataGridVyrobky

                    if (dataGridVyrobky.ItemsSource is (IList<VyrobekViewableModel> itemsSource and not null))
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