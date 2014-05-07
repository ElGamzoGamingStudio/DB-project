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
            if(BL.Application.CurrentTree != null) TreeLoaded();
        }

        private void LoadTree(object sender, RoutedEventArgs e)
        {
            var treeDialog = new CreateLoadTreeDialog();
            treeDialog.LoadClick(treeDialog.Load, new EventArgs());
            treeDialog.ShowDialog();
            if (BL.Application.CurrentTree != null) TreeLoaded();
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

        private void TreeLoaded()
        {
            var firetask =
                DAL.SqlRepository.Tasks.Cast<Models.Task>()
                   .First(
                       task =>
                       Models.Task.GetOldestParent(task).Id == BL.Application.CurrentTree.MainTaskId &&
                       task.Urgency.Value >=
                       DAL.SqlRepository.Tasks.Cast<Models.Task>()
                          .Where(task2 => Models.Task.GetOldestParent(task2).Id == BL.Application.CurrentTree.MainTaskId)
                          .Max(task1 => task1.Urgency.Value));
            var cildren = firetask.SelectChildrenTasks();
            var visual = new TaskVisual { Height = 150, Width = 150 };
            visual.Desc.Text = firetask.Description;
            visual.Date.Text = "С " + firetask.BeginDate.ToShortDateString() + " по " + firetask.EndDate.ToShortDateString();
            visual.CritDesc.Text = firetask.Criteria.Unit;
            visual.Progress.Text = firetask.Criteria.CurrentValue.ToString() + "/" + firetask.Criteria.TargetValue.ToString();
            visual.Field.Background = firetask.Criteria.IsCompleted() ? Brushes.Chartreuse : Brushes.Coral;
            TreeCanvas.Children.Add(visual);
            int i = cildren.Count + 1;
            _realCanvasWidth = visual.Width * i;
            OverflowCanvas();
            visual.SetValue(Canvas.LeftProperty, (TreeField.Width - visual.Width) / 2);
            visual.SetValue(Canvas.TopProperty, 20);
            
            foreach (var task in cildren)
            {
                var child = new TaskVisual {Height = 150, Width = 150};
                child.Desc.Text = task.Description;
                child.Date.Text = "С " + task.BeginDate.ToShortDateString() + " по " + task.EndDate.ToShortDateString();
                child.CritDesc.Text = task.Criteria.Unit;
                child.Progress.Text = task.Criteria.CurrentValue.ToString() + "/" + task.Criteria.TargetValue.ToString();
                child.Field.Background = task.Criteria.IsCompleted() ? Brushes.Chartreuse : Brushes.Coral;
                TreeCanvas.Children.Add(child);
                child.SetValue(Canvas.LeftProperty, ( TreeField.Width - visual.Width * i-- ) / 2);
                child.SetValue(Canvas.TopProperty, visual.Height + 220);
                var lane = new Line {StrokeThickness = 1, Fill = Brushes.DarkCyan,Name ="L" + child.Desc.Text};
                TreeCanvas.Children.Add(lane);
                lane.X1 = TreeField.Width/2;
                lane.Y1 = 20 + visual.Height;
                lane.X2 = (double) child.GetValue(Canvas.LeftProperty) + child.Width/2;
                lane.Y2 = (double) child.GetValue(Canvas.TopProperty);
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
