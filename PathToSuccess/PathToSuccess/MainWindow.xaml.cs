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

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _realCanvasHeight;
        private double _realCanvasWidth;
        public MainWindow()
        {
            InitializeComponent();
            _realCanvasWidth = 0;
            _realCanvasHeight = 0;
            DAL.SqlRepository.Initialize();
            var wind = new CreateLoadTreeDiaolg();
            wind.Show();
        }

        private void OverflowCanvas()
        {
            TreeCanvas.Width += _realCanvasWidth >= TreeCanvas.Width
                                    ? 300
                                    : _realCanvasWidth + 300 < TreeCanvas.Width ? -300 : 0;
            TreeCanvas.Height += _realCanvasHeight >= TreeCanvas.Height
                                    ? 170
                                    : _realCanvasHeight + 170 < TreeCanvas.Height ? -170 : 0;
        }

        private void Update(object sender, EventArgs e)
        {
            double widthDifference,heightDifference;
            if (WindowState == WindowState.Maximized)
            {
                widthDifference = SystemParameters.PrimaryScreenWidth - MinWidth ;
                heightDifference = SystemParameters.PrimaryScreenHeight - MinHeight - 40;
            }
            else
            {
                widthDifference = Width - MinWidth;
                heightDifference = Height - MinHeight;
            }
            TreeField.MaxWidth = TreeField.MinWidth + widthDifference;
            TreeField.MaxHeight = TreeField.MinHeight + heightDifference;

        }
    }
}
