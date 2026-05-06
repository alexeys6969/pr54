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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PermDynamics.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
        }

        private void OpenPageChart(object sender, RoutedEventArgs e)
        {
            float value = Convert.ToInt32(tb_value.Text);
            MainWindow.init.pointsInfo.Add(new Classes.PointInfo(value));
            MainWindow.init.frame.Navigate(new Pages.Chart());
        }
    }
}
