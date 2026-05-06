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
using System.Windows.Threading;

namespace PermDynamics.Pages
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : Page
    {
        public double actualHeightCanvas = 0;
        public double maxValue = 0;
        double averageValue = 0;
        public DispatcherTimer timer = new DispatcherTimer();
        public Chart()
        {
            InitializeComponent();
            actualHeightCanvas = MainWindow.init.Height - 50d;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += CreateNewValue;
            timer.Start();

            CreateChart();
            ColorChart();
        }

        private void CreateNewValue(object sender, EventArgs e)
        {
            Random random = new Random();
            double value = MainWindow.init.pointsInfo[MainWindow.init.pointsInfo.Count - 1].value;
            double newValue = value * (random.NextDouble() + 0.5d);
            MainWindow.init.pointsInfo.Add(new Classes.PointInfo(newValue));
            ControlCreateChart();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            actualHeightCanvas = MainWindow.init.Height - 50d;
            CreateChart();
            ColorChart();
        }

        public void CreateChart()
        {
            canvas.Children.Clear();
            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                if (MainWindow.init.pointsInfo[i].value > maxValue)
                    maxValue = MainWindow.init.pointsInfo[i].value;
            }
            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                Line line = new Line();
                line.X1 = i * 20;
                line.X2 = (i + 1) * 20;
                if (i == 0)
                    line.Y1 = actualHeightCanvas;
                else
                    line.Y1 = actualHeightCanvas - ((MainWindow.init.pointsInfo[(i - 1)].value / maxValue) * actualHeightCanvas);
                line.Y1 = actualHeightCanvas - ((MainWindow.init.pointsInfo[i].value / maxValue) * actualHeightCanvas);
                line.StrokeThickness = 2;
                MainWindow.init.pointsInfo[i].line = line;
                canvas.Children.Add(line);
            }
        }

        public void CreatePoint()
        {
            Line line = new Line();
            line.X1 = (MainWindow.init.pointsInfo.Count - 1) * 20;
            line.X2 = MainWindow.init.pointsInfo.Count * 20;
            line.Y1 = actualHeightCanvas - ((MainWindow.init.pointsInfo[(MainWindow.init.pointsInfo.Count - 2)].value / maxValue) * actualHeightCanvas);
            line.Y2 = actualHeightCanvas - ((MainWindow.init.pointsInfo[(MainWindow.init.pointsInfo.Count - 1)].value / maxValue) * actualHeightCanvas);
            line.StrokeThickness = 2;
            MainWindow.init.pointsInfo[(MainWindow.init.pointsInfo.Count - 1)].line = line;
            canvas.Children.Add(line);
        }

        public void ControlCreateChart()
        {
            double value = MainWindow.init.pointsInfo[MainWindow.init.pointsInfo.Count - 1].value;
            if (value < maxValue)
                CreatePoint();
            else
                CreateChart();
            ColorChart();
        }

        public void ColorChart()
        {
            double value = MainWindow.init.pointsInfo[MainWindow.init.pointsInfo.Count - 1].value;
            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                averageValue += MainWindow.init.pointsInfo[i].value;
            }
            averageValue = averageValue / MainWindow.init.pointsInfo.Count;

            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                if (value < averageValue)
                {
                    MainWindow.init.pointsInfo[i].line.Stroke = Brushes.Red;
                }
                else
                {
                    MainWindow.init.pointsInfo[i].line.Stroke = Brushes.Green;
                }
            }

            canvas.Width = MainWindow.init.pointsInfo.Count * 20 + 300;
            scroll.ScrollToHorizontalOffset(canvas.Width);
            current_value.Content += Math.Round(value, 2).ToString();
            average_value.Content += Math.Round(averageValue, 2).ToString();
        }
    }
}
