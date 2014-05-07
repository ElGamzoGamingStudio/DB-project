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
using PathToSuccess.Models;

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
            BL.Application.SetUp();
            _realCanvasWidth = 0;
            _realCanvasHeight = 0;

            var sc = new ScheduleVisualiser();
            sc.ShowDialog();

            //var log = new LoginWindow();
            //log.ShowDialog();
            //if (log.RightPass == null)
            //    Application.Current.Shutdown();

            UserInfo.Content = BL.Application.CurrentUser != null
                                   ? "Вы вошли как " + BL.Application.CurrentUser.Name
                                   : "";
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

        private void AddTask(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddStep(object sender, RoutedEventArgs e)
        {
            
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
            TreeField.MaxWidth = TreeCanvas.Width = TreeField.MinWidth + widthDifference;
            TreeField.MaxHeight = TreeCanvas.Height = TreeField.MinHeight + heightDifference;
            Filter.Margin = new Thickness(710 + widthDifference, 11, 0, 0);
            SearchButton.Margin = new Thickness(630 + widthDifference, 13, 0, 0);
        }

        private void CreateTree(object sender, RoutedEventArgs e)
        {
            var treeDialog = new CreateLoadTreeDialog();
            treeDialog.CreateClick(treeDialog.Create, new EventArgs());
            treeDialog.ShowDialog();
            
        }

        private void LoadTree(object sender, RoutedEventArgs e)
        {
            var treeDialog = new CreateLoadTreeDialog();
            treeDialog.LoadClick(treeDialog.Load, new EventArgs());
            treeDialog.ShowDialog();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if(BL.Application.CurrentTree==null) return;
        }

        private void All_OnSelected(object sender, RoutedEventArgs e)
        {
            UpdateTree(null);
        }

        private void UpdateTree(bool? filter)
        {
            if (filter != true)
            {
                
            }
            if (filter != false)
            {
                
            }
            
        }

        private void Tasks_OnSelected(object sender, RoutedEventArgs e)
        {
            UpdateTree(true);
        }

        private void TaskSt_OnSelected(object sender, RoutedEventArgs e)
        {
            UpdateTree(false);
        }

        private void OpenQuery(object sender, RoutedEventArgs e)
        {
            var q = new QueryDialog();
            q.ShowDialog();
        }
    }
}
