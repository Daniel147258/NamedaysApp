using System;
using System.Collections.Generic;
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

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private MainWindow window;
        public AddWindow(MainWindow window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Opa.Text.Length >= 3 && Opa2.SelectedDate != null)
            {
                DateTime date = Opa2.SelectedDate.Value;
                window.AddNameday(Opa.Text, date);
                Close();
            }
            else if (Opa.Text.Length < 3)
            {
                MessageBox.Show("Name must have at least length 3!!","Varovanie", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (Opa2.SelectedDate == null)
            {
                MessageBox.Show("Date wasnt selected !!!", "Varovanie", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
