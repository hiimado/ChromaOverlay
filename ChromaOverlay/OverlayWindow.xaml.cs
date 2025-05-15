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
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ChromaOverlay
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        private const int Rows = 18;
        private const int Columns = 5;
        private const double CellSize = 40;
        private const double CellPadding = 2;

        public OverlayWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            int exStyle = (int)GetWindowLong(hwnd, GWL_EXSTYLE);
            exStyle |= WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW;
            SetWindowLong(hwnd, GWL_EXSTYLE, (IntPtr)exStyle);
            DrawGrid();
        }

        // Interop declarations
        const int GWL_EXSTYLE = -20;
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int WS_EX_TOOLWINDOW = 0x00000080;

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private void DrawGrid()
        {
            OverlayCanvas.Children.Clear();

            double startX = (SystemParameters.PrimaryScreenWidth - (Columns * CellSize)) / 2;
            double startY = (SystemParameters.PrimaryScreenHeight - (Rows * CellSize)) / 2;

            for (int row = 0; row < Rows; row++) {
                    for (int col = 0; col < Columns; col++) {
                            double x = startX + (col * CellSize);
                            double y = startY + (row * CellSize);

                            var rect = new System.Windows.Shapes.Rectangle
                            {
                                Width = CellSize - CellPadding,
                                Height = CellSize - CellPadding,
                                Stroke = System.Windows.Media.Brushes.Lime,
                                StrokeThickness = 1,
                                Fill = System.Windows.Media.Brushes.Transparent
                            };

                            Canvas.SetLeft(rect, x);
                            Canvas.SetTop(rect, y);
                            OverlayCanvas.Children.Add(rect);

                            var label = new TextBlock
                            {
                                Text = $"{(char)('A' + col)}{row + 1}",
                                Foreground = System.Windows.Media.Brushes.White,
                                FontSize = 10,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                            };

                            Canvas.SetLeft(label, x + 4);
                            Canvas.SetTop(label, y + 4);
                            OverlayCanvas.Children.Add(label);
                    }
            } 
        }
    }
}
