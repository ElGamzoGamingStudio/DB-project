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
using System.Windows.Media.Animation;
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
            foreach (var urgency in DAL.SqlRepository.Urgencies.Cast<Urgency>())
            {
                Urg.Items.Add(new ComboBoxItem {Content = urgency.UrgencyName});
            }
            foreach (var importance in DAL.SqlRepository.Importancies.Cast<Importance>())
            {
                Imp.Items.Add(new ComboBoxItem {Content = importance.ImportanceName});
            }
            foreach (var entity in DAL.SqlRepository.Schedules.Cast<Models.Schedule>())
            {
                Graphs.Items.Add(new ComboBoxItem {Content = entity.Id});
            }
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
            Adding.IsEnabled = true;
            InAnimation();
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
            LevelUp.Margin = new Thickness(270 + widthDifference, 13, 0, 0);
            Adding.MinWidth = TreeField.MaxWidth;
            Adding.MinHeight = TreeField.MaxHeight;
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
            
        }

        private void UpdateTree(Models.Task displayTask)
        {
            var children = displayTask.SelectChildrenTasks();
            var visual = new TaskVisual { Height = 150, Width = 150 };
            visual.Desc.Text = displayTask.Description;
            visual.Date.Text = "С " + displayTask.BeginDate.ToShortDateString() + " по " +
                               displayTask.EndDate.ToShortDateString();
            visual.CritDesc.Text = displayTask.Criteria.Unit;
            visual.Progress.Text = displayTask.Criteria.CurrentValue.ToString() + "/" +
                                   displayTask.Criteria.TargetValue.ToString();
            visual.Field.Background = displayTask.Criteria.IsCompleted() ? Brushes.Chartreuse : Brushes.Coral;
            TreeCanvas.Children.Add(visual);
            int i = children.Count + 1;
            _realCanvasWidth = visual.Width * i;
            OverflowCanvas();
            visual.SetValue(Canvas.LeftProperty, (TreeField.MaxWidth - visual.Width) / 2);
            visual.SetValue(Canvas.TopProperty, 20.0);

            foreach (var task in children)
            {
                var child = new TaskVisual { Height = 150, Width = 150 };
                child.Desc.Text = task.Description;
                child.Date.Text = "С " + task.BeginDate.ToShortDateString() + " по " +
                                  task.EndDate.ToShortDateString();
                child.CritDesc.Text = task.Criteria.Unit;
                child.Progress.Text = task.Criteria.CurrentValue.ToString() + "/" +
                                      task.Criteria.TargetValue.ToString();
                child.Field.Background = task.Criteria.IsCompleted() ? Brushes.Chartreuse : Brushes.Coral;
                TreeCanvas.Children.Add(child);
                child.SetValue(Canvas.LeftProperty, (TreeField.MaxWidth - visual.Width * i--) / 2);
                child.SetValue(Canvas.TopProperty, visual.Height + 220);
                var lane = new Line { StrokeThickness = 1, Fill = Brushes.DarkCyan, Name = "L" + child.Desc.Text };
                TreeCanvas.Children.Add(lane);
                lane.X1 = TreeField.Width / 2;
                lane.Y1 = 20 + visual.Height;
                lane.X2 = (double)child.GetValue(Canvas.LeftProperty) + child.Width / 2;
                lane.Y2 = (double)child.GetValue(Canvas.TopProperty);
            }

            if (displayTask.ChildrenAreSteps())
            {
                var steps = displayTask.SelectChildrenSteps();
                visual.Width = 400;
                visual.Height = 400;
                var list = new ListBox()
                {
                    Margin = new Thickness(10.0, visual.Progress.Margin.Top + 30, 20.0, 0.0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                foreach (var step in steps)
                {
                    list.Items.Add(new ListBoxItem()
                    {
                        Content =
                            step.Description + "\n" + step.Criteria.Unit + "\t\t" + step.Criteria.CurrentValue +
                            "/" + step.Criteria.TargetValue
                    });
                }
                visual.Add.Click += AddTask;
                visual.Field.Children.Add(list);
            }
            
        }

        private void TreeLoaded()
        {
            var q = DAL.SqlRepository.Tasks.Cast<Models.Task>();
            var p = new List<Models.Task>();
            foreach (var task in q)
            {
                if(Models.Task.GetOldestParent(task).Id == BL.Application.CurrentTree.MainTaskId)
                    p.Add(task);
            }
            var m = p.Max(task1 => task1.Urgency.Value);
            foreach (var task1 in p)
            {
                if (Models.Task.GetOldestParent(task1).Id == BL.Application.CurrentTree.MainTaskId &&
                    task1.Urgency.Value >= m)
                {
                    UpdateTree(task1);
                    break;
                }
            }
        }

        private void Tasks_OnSelected(object sender, RoutedEventArgs e)
        {
            
        }

        private void TaskSt_OnSelected(object sender, RoutedEventArgs e)
        {
            
        }

        private void OpenQuery(object sender, RoutedEventArgs e)
        {
            var q = new QueryDialog();
            q.ShowDialog();
        }

        private void InAnimation()
        {
            var anim = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Duration(TimeSpan.FromSeconds(1)));
            Adding.BeginAnimation(MarginProperty, anim);

        }

       

        private void OutAnimation()
        {
            var anim = new ThicknessAnimation(new Thickness(-1000, 0, 0, 0), new Duration(TimeSpan.FromSeconds(1)));
            Adding.BeginAnimation(MarginProperty, anim);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var crit = new Criteria()
                {
                    CurrentValue = 0,
                    TargetValue = Convert.ToInt32(TargetVal.Text),
                    Unit = UnitBox.Text
                };
            var imp =
                DAL.SqlRepository.Importancies.Cast<Importance>()
                   .First(x => Imp.SelectedValue.ToString() == x.ImportanceName);
            var urg =
                DAL.SqlRepository.Urgencies.Cast<Urgency>().First(x => Urg.SelectedValue.ToString() == x.UrgencyName);
            if (StepButton.IsChecked == true)
            {
                var tr = new TimeRule()
                    {
                        IsPeriodic = Periodic.IsChecked == true,
                        Schedule =
                            DAL.SqlRepository.Schedules.Cast<Models.Schedule>()
                               .First(x => x.Id == Convert.ToInt32(Graphs.SelectedValue.ToString())),
                        ScheduleId = Convert.ToInt32(Graphs.SelectedValue.ToString())
                    };
                var st = new Step
                    {
                        BeginDate = Begin.SelectedDate != null ? (DateTime) Begin.SelectedDate : DateTime.Now,
                        EndDate = End.SelectedDate != null ? (DateTime) End.SelectedDate : DateTime.MaxValue,
                        Criteria = crit,
                        CriteriaId = crit.Id,
                        Description = DescBox.Text,
                        Importance = imp,
                        ImportanceName = imp.ImportanceName,
                        Urgency = urg,
                        UrgencyName = urg.UrgencyName,
                        TimeRule = tr,
                        TimeRuleId = tr.Id
                    };
            }
            else
            {
                var t = new PathToSuccess.Models.Task
                    {
                    BeginDate = Begin.SelectedDate != null ? (DateTime)Begin.SelectedDate : DateTime.Now,
                    EndDate = End.SelectedDate != null ? (DateTime)End.SelectedDate : DateTime.MaxValue,
                    Criteria = crit,
                    CriteriaId = crit.Id,
                    Description = DescBox.Text,
                    Importance = imp,
                    ImportanceName = imp.ImportanceName,
                    Urgency = urg,
                    UrgencyName = urg.UrgencyName
                };
            }
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            DescBox.Clear();
            Begin.SelectedDate = End.SelectedDate = null;
            UnitBox.Clear();
            TargetVal.Clear();
            OutAnimation();
            Adding.IsEnabled = false;
        }
    }
}
