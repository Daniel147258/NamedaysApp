using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NameDayCalendar c;
        private int month;
        private List<Nameday> sortedFilter;
        private Nameday meniny;
        EditWindow eWin;
        AddWindow aWin;

        public MainWindow()
        {
            c = new NameDayCalendar();
            sortedFilter = new List<Nameday>();
            FileInfo file = new FileInfo("C:\\Users\\danie\\source\\repos\\Namedays\\Uniza.Namedays\\namedays-sk.csv");
            c.Load(file);
            InitializeComponent();
            ShowNameDays(DateTime.Now);
            eWin = null;
            aWin = null;
            month = -1;
            ShowCount(0);
        }

        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            calendar.DisplayDate = DateTime.Now;
            calendar.SelectedDate = DateTime.Now;
            ShowNameDays(DateTime.Now);
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate != null)
            {
                DateTime displayDate = (DateTime)calendar.SelectedDate;
                ShowNameDays(displayDate);
            }
        }

        private void ShowNameDays(DateTime date)
        {
            string generatedText = date.ToString("dd.MM.yyyy") + " celebrates";
            Namedays1.Text = generatedText;
            var n = c[date];
            string a = "";
            foreach (var d in n)
            {
                a += d + "\n";
            }
            Names.Text = a;
            Names.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double desiredHeight = Names.DesiredSize.Height;
            Names.Height = desiredHeight;
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContextMenu contextMenu = button.ContextMenu;
            contextMenu.PlacementTarget = button;
            contextMenu.IsOpen = true;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContextMenu contextMenu = button.ContextMenu;
            contextMenu.PlacementTarget = button;
            contextMenu.IsOpen = true;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (c.NameCount > 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want delete actual calendar of namedays?", "Confirmation",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    c.Clear();
                }
            }
            else
                c.Clear();
            NamesInFilter(0, "");
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV Files (*.csv)| *.csv|All Files (*.*)| *.*";
            if (dialog.ShowDialog() == true)
            {
                string nameOfFile = dialog.FileName;
                FileInfo file = new FileInfo(nameOfFile);
                c.Load(file);
            }
            NamesInFilter(0, "");
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV Files (*.csv)| *.csv|All Files (*.*)| *.*";
            if (dialog.ShowDialog() == true)
            {
                string nameOfFile = dialog.FileName;
                FileInfo file = new FileInfo(nameOfFile);
                c.Save(file);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Really do you want exit application?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                result = MessageBox.Show("Really?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    result = MessageBox.Show("Are you crazy?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        result = MessageBox.Show("Ok last one?", "Confirmation",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                            Application.Current.Shutdown();
                    }
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Height = 250;
            about.Width = 390;
            about.ShowDialog();
        }

        private void Click_Month(object sender, SelectionChangedEventArgs e)
        {
            
            int selectedMonthIndex = Month.SelectedIndex  + 1;
            this.month = selectedMonthIndex ;
            NamesInFilter(this.month, regex.Text);
        }

        private void NamesInFilter(int month, string filter)
        {
            sortedFilter.Clear();
            SortedNames.ItemsSource = null;
            if (month == 0) 
            {
                regex.Text = null;
                sortedFilter.Clear();
                SortedNames.ItemsSource = null;
                Month.SelectedItem = null;
                this.month = -1;
            }
            if (filter.Equals("") || filter.Length == 0 )
            {
                var list = c[month];
                var list2 = list.OrderBy(x => x.DayMonth.Day);
                foreach (var item in list2)
                {
                    sortedFilter.Add(item);

                }
                SortedNames.ItemsSource = sortedFilter;
            }
            else if (month != -1)
            {
                var list = c[month];
                var list2 = list.OrderBy(x => x.DayMonth.Day);
                foreach (var item in list2)
                {
                    if (item.Name.Contains(filter))
                    {
                        sortedFilter.Add(item);
                    }
                }
                SortedNames.ItemsSource = sortedFilter;
            }
            else if(month == -1 )
            {
                var list = c.GetNameDays(filter);
                var list2 = list.OrderBy(x => x.DayMonth.ToDateTime());
                foreach (var item in list2)
                {
                    if (item.Name.Contains(filter))
                    {
                        sortedFilter.Add(item);
                    }
                }
                SortedNames.ItemsSource = sortedFilter;
            }
            ShowCount(sortedFilter.Count);
        }

        private void RegexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NamesInFilter(month, regex.Text);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            NamesInFilter(0, "");
        }

        private void SortedNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedItem != null)
            {
                if ((Nameday)listBox.SelectedItem != null)
                {
                    meniny = (Nameday)listBox.SelectedItem;
                    Edit.IsEnabled = true;
                    Remove.IsEnabled = true;
                    ShowOnCalendar.IsEnabled = true;
                }
            }
            else
            {
                Edit.IsEnabled = false;
                Remove.IsEnabled = false; 
                ShowOnCalendar.IsEnabled = false;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (eWin == null || !eWin.IsVisible)
            {
                eWin = new EditWindow(this);
                eWin.Owner = this;
                eWin.Height = 140;
                eWin.Width = 300;
                eWin.showText(meniny.Name, meniny.DayMonth.ToDateTime());
                eWin.Show();
            } 
        }

        public void EditNameday(string name, DateTime date)
        {
            c.Remove(meniny.Name);
            meniny = new Nameday(name, new DayMonth(date.Day, date.Month));
           c.Add(meniny);
            NamesInFilter(meniny.DayMonth.Month, regex.Text);
            eWin = null;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (aWin == null || !aWin.IsVisible)
            {
                aWin = new AddWindow(this);
                aWin.Owner = this;
                aWin.Height = 140;
                aWin.Width = 300;
                aWin.Show();
            }
        }

        public void AddNameday(string name, DateTime date)
        {
            if (c.Contains(name))
            {
                MessageBox.Show("There is name like that.", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Sucessfuly added :)", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                c.Add(new Nameday(name, new DayMonth(date.Day, date.Month)));
                NamesInFilter(date.Month, regex.Text);
            }
            aWin = null;
        }

        private void Remove_Click(object sender, RoutedEventArgs e )
        {
            MessageBoxResult result = MessageBox.Show("Really do you want remove (" + meniny.Name + ") ", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int mon = meniny.DayMonth.Month;
                c.Remove(meniny.Name);
                NamesInFilter(mon, regex.Text);
            }
        }

        private void ShowCount(int currentCount)
        {
            string retazec = string.Format("{0}/{1}", currentCount, c.NameCount);
            Count.Text = retazec;
        }

        private void ShowOnCalendar_Click(object sender, RoutedEventArgs e)
        {
            DateTime displayDate = meniny.DayMonth.ToDateTime();
            calendar.DisplayDate = displayDate;
            calendar.SelectedDate = displayDate;
            ShowNameDays(displayDate);
        }
    }
}
