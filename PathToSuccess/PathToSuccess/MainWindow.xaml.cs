using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PathToSuccess.Models;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class MainWindow : Window
// ReSharper restore RedundantExtendsListEntry
    {
        
        private double _realCanvasWidth;
        private TaskVisual _parent;
        public MainWindow()
        {
            InitializeComponent();
            BL.Application.SetUp();
            _realCanvasWidth = 0;

            var tree = new CreateLoadTreeDialog();
            tree.ShowDialog();
            //var sc = new ScheduleVisualiser();
            //sc.ShowDialog();
            //var timeline = new ScheduleTimeline();
            //timeline.ShowDialog();

            Adding.Visibility=Visibility.Collapsed;
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
            TreeCanvas.MinWidth = _realCanvasWidth >= TreeCanvas.Width
                                    ? _realCanvasWidth
                                    : TreeField.MaxWidth;
           
        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            Adding.IsEnabled = true;
            Adding.Visibility = Visibility.Visible;
            InAnimation();
        }

        private void AddStep(object sender, RoutedEventArgs e)
        {
            Adding.IsEnabled = true;
            StepButton.IsChecked = true;
            TaskButton.IsEnabled = false;
            Adding.Visibility = Visibility.Visible;
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
            LevelUp.Margin = new Thickness(270 + widthDifference, 13, 0, 0);
            Adding.MinWidth = TreeField.MaxWidth;
            Adding.MinHeight = TreeField.MaxHeight;
        }

        private void CreateTree(object sender, RoutedEventArgs e)
        {
            var treeDialog = new CreateLoadTreeDialog();
            treeDialog.CreateClick(treeDialog.Create, new EventArgs());
            treeDialog.ShowDialog();
            if (BL.Application.CurrentTree != null)
            {
                TreeLoaded();

            }
        }

        private void LoadTree(object sender, RoutedEventArgs e)
        {
            var treeDialog = new CreateLoadTreeDialog();
            treeDialog.LoadClick(treeDialog.Load, new EventArgs());
            treeDialog.ShowDialog();
            if (BL.Application.CurrentTree != null)
            {
                TreeLoaded();
                ByName.IsSelected = false;
                ByName.IsSelected = true;
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if(BL.Application.CurrentTree==null) return;
            DAL.SqlRepository.Save();
        }

        private void All_OnSelected(object sender, RoutedEventArgs e)
        {
            if(RawView==null)return;
            foreach (var result in RawView.Items.Cast<ListBoxItem>())
            {
                result.Background = Brushes.White;
            }
        }

        public enum MoveDirections
        {
            Up, Down, None
        }

        private void UpdateTree(Task displayTask,MoveDirections dir)
        {
            TreeCanvas.Children.Clear();
            var children = displayTask.SelectChildrenTasks();
            var visual = new TaskVisual { Height = 150, Width = 150 };
            LevelUp.IsEnabled = displayTask.ParentId != -1;
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
            visual.SetValue(Canvas.LeftProperty, (TreeCanvas.MinWidth - visual.Width) / 2);
            visual.SetValue(Canvas.TopProperty, 20.0);
            _parent = visual;
            if (displayTask.ChildrenAreSteps())
                visual.Add.Click += AddStep;
            else
                visual.Add.Click += AddTask;
            bool evenCount = children.Count%2 == 0;
            int center = children.Count/2;
            var centerLeft = (double) visual.GetValue(Canvas.LeftProperty);
            int module=(i-1)/2;
            var anim = new ThicknessAnimation(dir==MoveDirections.Down?new Thickness(0, -1000, 0, 200):new Thickness(0,200,0,-1000), new Thickness(0, 0, 0, 0),
                                                  new Duration(TimeSpan.FromSeconds(1)));
            if(dir!=MoveDirections.None)
                visual.BeginAnimation(MarginProperty,anim);
            foreach (var task in children)
            {
                var child = new TaskVisual
                    {
                        Height = 150,
                        Width = 150,
                        Desc = {Text = task.Description},
                        Date =
                            {
                                Text = "С " + task.BeginDate.ToShortDateString() + " по " +
                                       task.EndDate.ToShortDateString()
                            },
                        CritDesc = {Text = task.Criteria.Unit},
                        Progress =
                            {
                                Text = task.Criteria.CurrentValue.ToString() + "/" +
                                       task.Criteria.TargetValue.ToString()
                            },
                        Field = {Background = task.Criteria.IsCompleted() ? Brushes.Chartreuse : Brushes.Coral}
                    };
                TreeCanvas.Children.Add(child);
                i--;
                double offset = !evenCount
                                    ? (i == center + 1
                                           ? centerLeft
                                           : (i > center + 1
                                                  ? centerLeft + (visual.Width + visual.Width/children.Count)*module
                                                  : centerLeft - (visual.Width + visual.Width/2)*i))
                                    : (i > center
                                           ? centerLeft + visual.Width/2 + visual.Width / children.Count / 2 +
                                             (visual.Width/children.Count + visual.Width)*(module-1)
                                           : centerLeft - visual.Width / 2 - visual.Width / children.Count / 2 -
                                             (visual.Width/children.Count + visual.Width)*(i - 1));
                module--;
                
                
                child.SetValue(Canvas.LeftProperty, offset);
                child.SetValue(Canvas.TopProperty, visual.Height + 180);
                child.Add.Visibility=Visibility.Collapsed;
                var lane = new Line
                    {
                        StrokeThickness = 5,
                        Fill = Brushes.DarkCyan,
                        X1 = (double) visual.GetValue(Canvas.LeftProperty) + visual.Width/2,
                        Y1 = 20 + visual.Height,
                        Stroke = Brushes.DarkCyan,
                        X2 = (double) child.GetValue(Canvas.LeftProperty) + child.Width/2,
                        Y2 = (double) child.GetValue(Canvas.TopProperty),
                        SnapsToDevicePixels = true
                    };
                lane.SetValue(RenderOptions.EdgeModeProperty,EdgeMode.Aliased);
                TreeCanvas.Children.Add(lane);
                
                if (dir != MoveDirections.None)
                {
                    child.BeginAnimation(MarginProperty, anim);
                    lane.BeginAnimation(MarginProperty,anim);
                }
                child.PreviewMouseLeftButtonUp += SelectChild;
                
            }
            if (TreeCanvas.Children.Count == 1)
            {
                visual.Width = visual.Height = 300;
            }
            if (displayTask.ChildrenAreSteps())
            {
                var steps = displayTask.SelectChildrenSteps();
                var list = new ListBox
                    {
                    Margin = new Thickness(10.0, visual.Progress.Margin.Top + 30, 20.0, 0.0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = visual.Width*0.8
                };
                foreach (var step in steps)
                {
                    list.Items.Add(new ListBoxItem
                        {
                        Content =
                            step.Description + "\n\n" + step.Criteria.Unit + "\t\t" + step.Criteria.CurrentValue +
                            "/" + step.Criteria.TargetValue,
                            BorderThickness = new Thickness(1),
                            BorderBrush = Brushes.LightBlue
                    });
                }
                visual.Add.Click += AddStep;
                visual.Field.Children.Add(list);
            }
            
        }

        private void SelectChild(object sender, RoutedEventArgs e)
        {
            var child = sender as TaskVisual;
            TreeCanvas.Children.Clear();
            UpdateTree(DAL.SqlRepository.Tasks.Cast<Task>().First(x=>x.Description==child.Desc.Text),MoveDirections.Up);
        }

        private void TreeLoaded()
        {
            var q = DAL.SqlRepository.Tasks.Cast<Task>();
            var p = new List<Task>();
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var task in q)
// ReSharper restore LoopCanBeConvertedToQuery
            {
                if(Task.GetOldestParent(task).Id == BL.Application.CurrentTree.MainTaskId)
                    p.Add(task);
            }
            foreach (var task in p)
            {
                RawView.Items.Add(new ListBoxItem {Content = task.Description, Height = 20});
            }
            var m = p.Max(task1 => task1.Urgency.Value);
            foreach (var task1 in p)
            {
                if (Task.GetOldestParent(task1).Id == BL.Application.CurrentTree.MainTaskId &&
                    task1.Urgency.Value >= m)
                {
                    UpdateTree(task1, MoveDirections.None);
                    RawView.Items.Cast<ListBoxItem>().First(x => x.Content.ToString() == task1.Description).IsSelected =
                        true;
                    break;
                }
            }
        }

        private void Tasks_OnSelected(object sender, RoutedEventArgs e)
        {
            var tasks = Task.Select(x => !x.ChildrenAreSteps());
            foreach (var result in RawView.Items.Cast<ListBoxItem>())
            {
                result.Background = tasks.FirstOrDefault(x => x.Description == result.Content.ToString()) != null
                                        ? Brushes.CornflowerBlue
                                        : Brushes.White;
            }
        }

        private void TaskSt_OnSelected(object sender, RoutedEventArgs e)
        {
            var tasks = Task.Select(x => x.ChildrenAreSteps());
            foreach (var result in RawView.Items.Cast<ListBoxItem>())
            {
                result.Background = tasks.FirstOrDefault(x => x.Description == result.Content.ToString()) != null
                                        ? Brushes.CornflowerBlue
                                        : Brushes.White;
            }
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
            anim.Completed += HideThisShit;
            Adding.BeginAnimation(MarginProperty, anim);
        }

        private void HideThisShit(object sender, EventArgs e)
        {
            Adding.Visibility=Visibility.Collapsed;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<Task>();
            foreach (var entity in DAL.SqlRepository.Tasks.Cast<Task>())
            {
                if (Task.GetOldestParent(entity).Id == BL.Application.CurrentTree.MainTaskId &&
                       entity.Description == DescBox.Text)
                    list.Add(entity);
            }
            var crit = new Criteria
                {
                CurrentValue = 0,
                TargetValue = Convert.ToInt32(TargetVal.Text),
                Unit = UnitBox.Text
            };
            var imp =
                DAL.SqlRepository.Importancies.Cast<Importance>()
                   .First(x => ((ComboBoxItem)Imp.SelectedItem).Content.ToString() == x.ImportanceName);
            var urg =
                DAL.SqlRepository.Urgencies.Cast<Urgency>().First(x => ((ComboBoxItem)Urg.SelectedItem).Content.ToString() == x.UrgencyName);
            if (StepButton.IsChecked == true)
            {
                var tr = new TimeRule
                    {
                        IsPeriodic = Periodic.IsChecked == true,
                        Schedule =
                            DAL.SqlRepository.Schedules.Cast<Models.Schedule>().ToList()
                               .First(x => x.Id == Convert.ToInt32(((ComboBoxItem)Graphs.SelectedItem).Content.ToString())),
                        ScheduleId = Convert.ToInt32(((ComboBoxItem)Graphs.SelectedItem).Content.ToString())
                    };
                var t = DAL.SqlRepository.Tasks.Cast<Task>().First(x => x.Description == _parent.Desc.Text);
                if (
                    DAL.SqlRepository.Steps.Cast<Step>()
                       .FirstOrDefault(x => x.TaskId == t.Id && x.Description == DescBox.Text) != null)
                {
                    MessageBox.Show("Такой шаг у цели уже есть!", "Warning", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                }
                var st = new Step
                    {
                        BeginDate = Begin.SelectedDate != null ? (DateTime) Begin.SelectedDate : DateTime.Now,
                        EndDate = End.SelectedDate != null ? (DateTime) End.SelectedDate : Interval.PIOS,
                        Criteria = crit,
                        CriteriaId = crit.Id,
                        Description = DescBox.Text,
                        Importance = imp,
                        ImportanceName = imp.ImportanceName,
                        Urgency = urg,
                        UrgencyName = urg.UrgencyName,
                        TimeRule = tr,
                        TimeRuleId = tr.Id,
                        ParentTask = t,
                        TaskId = t.Id
                    };
                DAL.SqlRepository.Steps.Add(st);
                UpdateTree(t,MoveDirections.None);
            }
            else
            {
                var p = DAL.SqlRepository.Tasks.Cast<Task>().First(x => x.Description == _parent.Desc.Text);
                var t = new Task
                    {
                    BeginDate = Begin.SelectedDate != null ? (DateTime)Begin.SelectedDate : DateTime.Now,
                    EndDate = End.SelectedDate != null ? (DateTime)End.SelectedDate : Interval.PIOS,
                    Criteria = crit,
                    CriteriaId = crit.Id,
                    Description = DescBox.Text,
                    Importance = imp,
                    ImportanceName = imp.ImportanceName,
                    Urgency = urg,
                    UrgencyName = urg.UrgencyName,
                    Parent = p,
                    ParentId = p.Id
                };
                DAL.SqlRepository.Tasks.Add(t);

                UpdateTree(p,MoveDirections.None);
                
            }
            OutAnimation();
            Adding.IsEnabled = false;
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

        private void LevelUp_Click(object sender, RoutedEventArgs e)
        {
            var parent =
                DAL.SqlRepository.Tasks.Cast<Task>().First(x => x.Description == _parent.Desc.Text);
            TreeCanvas.Children.Clear();
            
            UpdateTree(parent.Parent,MoveDirections.Down);
        }


        private void ShowScheduleTimeline(object sender, RoutedEventArgs e)
        {
            var sched = new ScheduleTimeline();
            sched.ShowDialog();
        }

        private void ShowSchedule(object sender, RoutedEventArgs e)
        {
            var schedVis = new ScheduleVisualiser();
            schedVis.ShowDialog();
        }

        private void OrderByUrgency(object sender, RoutedEventArgs e)
        {
            if (RawView.Items.Count == 0) return;
            var displayedItems = RawView.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
            var sorted =
                DAL.SqlRepository.Tasks.Cast<Task>()
                   .Where(x => displayedItems.Contains(x.Description))
                   .OrderByDescending(task => task.Urgency.Value);
            RawView.Items.Clear();
            foreach (var task in sorted)
            {
                RawView.Items.Add(new ListBoxItem {Content = task.Description});
            }
        }

        private void OrderByImportance(object sender, RoutedEventArgs e)
        {
            if (RawView.Items.Count == 0) return;
            var displayedItems = RawView.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
            var sorted =
                DAL.SqlRepository.Tasks.Cast<Task>()
                   .Where(x => displayedItems.Contains(x.Description))
                   .OrderByDescending(task => task.Importance.Value);
            RawView.Items.Clear();
            foreach (var task in sorted)
            {
                RawView.Items.Add(new ListBoxItem { Content = task.Description });
            }
        }

        private void OrderByDescription(object sender, RoutedEventArgs e)
        {
            if(RawView.Items.Count==0)return;
            var displayedItems = RawView.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
            var sorted =
                DAL.SqlRepository.Tasks.Cast<Task>()
                   .Where(x => displayedItems.Contains(x.Description))
                   .OrderBy(task => task.Description.ToLower());
            RawView.Items.Clear();
            foreach (var task in sorted)
            {
                RawView.Items.Add(new ListBoxItem { Content = task.Description });
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(SearchBox.Text.Length==0)return;
            var result = DAL.SqlRepository.Tasks.Cast<Task>().Where(task => task.Description.Contains(SearchBox.Text));
            RawView.Items.Clear();
            foreach (var task in result)
            {
                RawView.Items.Add(new ListBoxItem {Content = task.Description});
            }
            Disc.Visibility=Visibility.Visible;
            ByName.IsSelected = false;
            ByName.IsSelected = true;
        }

        private void Disc_OnClick(object sender, RoutedEventArgs e)
        {
            RawView.Items.Clear();
            var treeList = new List<Task>();
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var entity in DAL.SqlRepository.Tasks.Cast<Task>())
// ReSharper restore LoopCanBeConvertedToQuery
            {
                if (Task.GetOldestParent(entity).Id == BL.Application.CurrentTree.MainTaskId)
                {
                    treeList.Add(entity);
                }
            }
            foreach (var source in treeList)
            {
                RawView.Items.Add(new ListBoxItem {Content = source.Description});
            }
            ByName.IsSelected = false;
            ByName.IsSelected = true;
            Disc.Visibility=Visibility.Hidden;
        }

        private void TaskSelected(object sender, RoutedEventArgs e)
        {
            var source = sender as ListBoxItem;
            var selectedTask =
                DAL.SqlRepository.Tasks.Cast<Task>().First(task => task.Description == source.Content.ToString());
            UpdateTree(selectedTask,MoveDirections.None);

        }
    }
}
