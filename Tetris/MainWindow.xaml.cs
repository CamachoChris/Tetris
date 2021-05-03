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

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string AppName = "Tetris";
        const string Version = "0.0.0";
        const string Developer = "Grimakar";
        const string TimeOfDevelopment = "May 2021";

        const int SquareSize = 30;
        const int FieldSizeX = 10; //horizontal
        const int FieldSizeY = 18; //vertical

        public MainWindow()
        {
            InitializeComponent();
            for (int j = 0; j < 9; j++)
            {
                if (j %2==0)
                for (int i = 0; i < 10; i++)
                {
                    PaintSquare(i, i + j, Brushes.DarkOrange);
                }
                if (j % 2 == 1)
                    for (int i = 0; i < 10; i++)
                {
                    PaintSquare(i, i + j, Brushes.Green);
                }
            }
        }

        public void PaintSquare(int x, int y, Brush brush)
        {
            Rectangle rectangle = new Rectangle() { Height = SquareSize, Width = SquareSize, RadiusX = 3, RadiusY = 3 };
            rectangle.Fill = brush;
            rectangle.Margin = new Thickness(x * SquareSize, y * SquareSize, 0, 0);
            PlayingField.Children.Add(rectangle);
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, $"{AppName}\n{Version}\n{TimeOfDevelopment} {Developer}.\nNo rights reserved...", $"About {AppName}");
        }

        private void MenuQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
