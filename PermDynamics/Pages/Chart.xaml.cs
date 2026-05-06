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

        // Линия для отображения среднего значения
        private Line averageLine;

        public Chart()
        {
            InitializeComponent();
            actualHeightCanvas = MainWindow.init.Height - 50d;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += CreateNewValue;
            timer.Start();

            CreateChart();
            ColorChart();
            CreateAverageLine();
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
            CreateAverageLine();
        }

        public void CreateChart()
        {
            canvas.Children.Clear();
            maxValue = 0;

            // Находим максимальное значение
            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                if (MainWindow.init.pointsInfo[i].value > maxValue)
                    maxValue = MainWindow.init.pointsInfo[i].value;
            }

            // Добавляем небольшой запас к максимальному значению
            maxValue *= 1.1;

            // Создаем линии графика
            for (int i = 0; i < MainWindow.init.pointsInfo.Count - 1; i++)
            {
                Line line = new Line();
                line.X1 = i * 20;
                line.X2 = (i + 1) * 20;
                line.Y1 = actualHeightCanvas - ((MainWindow.init.pointsInfo[i].value / maxValue) * actualHeightCanvas);
                line.Y2 = actualHeightCanvas - ((MainWindow.init.pointsInfo[i + 1].value / maxValue) * actualHeightCanvas);
                line.StrokeThickness = 2;
                line.Stroke = Brushes.Gray;
                canvas.Children.Add(line);
                MainWindow.init.pointsInfo[i].line = line;
            }

            // Для последней точки тоже сохраняем линию
            if (MainWindow.init.pointsInfo.Count >= 2)
            {
                MainWindow.init.pointsInfo[MainWindow.init.pointsInfo.Count - 1].line =
                    (Line)canvas.Children[canvas.Children.Count - 1];
            }
        }

        public void CreatePoint()
        {
            if (MainWindow.init.pointsInfo.Count < 2) return;

            int lastIndex = MainWindow.init.pointsInfo.Count - 1;
            int prevIndex = lastIndex - 1;

            Line line = new Line();
            line.X1 = prevIndex * 20;
            line.X2 = lastIndex * 20;
            line.Y1 = actualHeightCanvas - ((MainWindow.init.pointsInfo[prevIndex].value / maxValue) * actualHeightCanvas);
            line.Y2 = actualHeightCanvas - ((MainWindow.init.pointsInfo[lastIndex].value / maxValue) * actualHeightCanvas);
            line.StrokeThickness = 2;
            line.Stroke = Brushes.Gray;
            MainWindow.init.pointsInfo[lastIndex].line = line;
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
            UpdateAverageLine();
        }

        public void ColorChart()
        {
            // Правильно вычисляем среднее значение (с обнулением)
            averageValue = 0;
            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                averageValue += MainWindow.init.pointsInfo[i].value;
            }
            averageValue = averageValue / MainWindow.init.pointsInfo.Count;

            // Раскрашиваем каждую линию в зависимости от её значения
            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                if (MainWindow.init.pointsInfo[i].line != null)
                {
                    if (MainWindow.init.pointsInfo[i].value < averageValue)
                    {
                        MainWindow.init.pointsInfo[i].line.Stroke = Brushes.Red;
                    }
                    else
                    {
                        MainWindow.init.pointsInfo[i].line.Stroke = Brushes.Green;
                    }
                }
            }

            // Обновляем отображение
            double lastValue = MainWindow.init.pointsInfo[MainWindow.init.pointsInfo.Count - 1].value;
            canvas.Width = MainWindow.init.pointsInfo.Count * 20 + 300;
            scroll.ScrollToHorizontalOffset(canvas.Width);
            current_value.Content = "Тек. знач: " + Math.Round(lastValue, 2).ToString();
            average_value.Content = "Сред. знач: " + Math.Round(averageValue, 2).ToString();
        }

        public void CreateAverageLine()
        {
            if (MainWindow.init.pointsInfo.Count == 0) return;

            // Вычисляем среднее значение
            averageValue = 0;
            for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
            {
                averageValue += MainWindow.init.pointsInfo[i].value;
            }
            averageValue = averageValue / MainWindow.init.pointsInfo.Count;

            // Удаляем старую линию, если она есть
            if (averageLine != null && canvas.Children.Contains(averageLine))
            {
                canvas.Children.Remove(averageLine);
            }

            // Создаем новую желтую пунктирную линию
            averageLine = new Line();
            averageLine.X1 = 0;
            averageLine.X2 = Math.Max(canvas.Width, MainWindow.init.pointsInfo.Count * 20);
            averageLine.Y1 = actualHeightCanvas - ((averageValue / maxValue) * actualHeightCanvas);
            averageLine.Y2 = averageLine.Y1;
            averageLine.StrokeThickness = 2;
            averageLine.Stroke = Brushes.Yellow;
            averageLine.Opacity = 0.8;

            // Добавляем линию на canvas
            canvas.Children.Add(averageLine);
        }

        public void UpdateAverageLine()
        {
            if (averageLine != null && canvas.Children.Contains(averageLine))
            {
                // Пересчитываем среднее значение
                averageValue = 0;
                for (int i = 0; i < MainWindow.init.pointsInfo.Count; i++)
                {
                    averageValue += MainWindow.init.pointsInfo[i].value;
                }
                averageValue = averageValue / MainWindow.init.pointsInfo.Count;

                // Обновляем позицию линии
                double y = actualHeightCanvas - ((averageValue / maxValue) * actualHeightCanvas);
                averageLine.Y1 = y;
                averageLine.Y2 = y;
                averageLine.X2 = Math.Max(canvas.Width, MainWindow.init.pointsInfo.Count * 20);
            }
            else
            {
                CreateAverageLine();
            }
        }
    }
}