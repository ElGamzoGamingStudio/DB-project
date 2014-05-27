using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private Grid _stepChanger;
        private bool _addFlag;
        public MainWindow()
        {
            InitializeComponent();
            BL.Application.SetUp();
            _realCanvasWidth = 0;
            _addFlag = true;
            //var tree = new CreateLoadTreeDialog();
            //tree.ShowDialog();
            //var sc = new ScheduleVisualiser();
            //sc.ShowDialog();
            //var timeline = new ScheduleTimeline();
            //timeline.ShowDialog();

            Adding.Visibility = Visibility.Collapsed;
            var log = new LoginWindow();
            log.ShowDialog();
            if (log.RightPass == null)
                Application.Current.Shutdown();

            UserInfo.Content = BL.Application.CurrentUser != null
                                   ? "Вы вошли как " + BL.Application.CurrentUser.Name
                                   : "";
            foreach (var urgency in DAL.SqlRepository.Urgencies.Cast<Urgency>())
            {
                Urg.Items.Add(new ComboBoxItem { Content = urgency.UrgencyName });
            }
            foreach (var importance in DAL.SqlRepository.Importancies.Cast<Importance>())
            {
                Imp.Items.Add(new ComboBoxItem { Content = importance.ImportanceName });
            }
            foreach (var entity in DAL.SqlRepository.Schedules.Cast<Models.Schedule>())
            {
                Graphs.Items.Add(new ComboBoxItem { Content = entity.Id });
            }

            /*
             * test section
             */
            var stat = new Statistics();
            stat.ShowDialog();
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
            var t = BL.ChangesBuffer.CurrentState.TaskBuffer.Find(x => _parent.Desc.Text == x.Description);
            Begin.DisplayDateStart = End.DisplayDateStart = t.BeginDate;
            End.DisplayDateEnd = Begin.DisplayDateEnd = t.EndDate;
            TaskButton.IsChecked = false;
            TaskButton.IsChecked = true;
            StepButton.IsEnabled = t.ChildrenAreSteps();
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
            double widthDifference, heightDifference;
            if (WindowState == WindowState.Maximized)
            {
                widthDifference = SystemParameters.PrimaryScreenWidth - MinWidth;
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
            if (BL.Application.CurrentTree == null) return;
            BL.ChangesBuffer.SaveChanges();
        }

        private void All_OnSelected(object sender, RoutedEventArgs e)
        {
            if(BL.Application.CurrentTree==null)return;
            RawView.Items.Clear();
            foreach (var result in Task.SelectAllTreeTask(BL.Application.CurrentTree.MainTaskId))
            {
                RawView.Items.Add(new ListBoxItem {Content = result.Description});
            }
        }

        public enum MoveDirections
        {
            Up, Down, None
        }

        private void RemoveTask(object sender, RoutedEventArgs e)
        {

            var targetTask = BL.ChangesBuffer.CurrentState.TaskBuffer.First(task => task.Description == _parent.Desc.Text);
            Task.CascadeRemoving(targetTask);
            if (targetTask.ParentId == -1)
            {
                TreeCanvas.Children.Clear();
                return;
            }
            UpdateTree(BL.ChangesBuffer.CurrentState.TaskBuffer.First(task => task.Id == targetTask.ParentId), MoveDirections.Down);
            UpdateRawView();
            ByName.IsSelected = false;
            ByName.IsSelected = true;
        }

        private void EditTask(object sender, RoutedEventArgs e)
        {
            var targetTask = BL.ChangesBuffer.CurrentState.TaskBuffer.First(task => task.Description == _parent.Desc.Text);
            Adding.Visibility = Visibility.Visible;
            Adding.IsEnabled = true;
            InAnimation();
            Labl.Content = "Редактирование";
            Chos.Visibility = Visibility.Collapsed;
            DescBox.Text = targetTask.Description;
            Begin.SelectedDate = targetTask.BeginDate;
            End.SelectedDate = targetTask.EndDate;
            Imp.SelectedItem =
                Imp.Items.Cast<ComboBoxItem>().First(item => item.Content.ToString() == targetTask.ImportanceName);
            Urg.SelectedItem =
                Urg.Items.Cast<ComboBoxItem>().First(item => item.Content.ToString() == targetTask.UrgencyName);
            CritLabel.Visibility =
                UnitLabel.Visibility =
                UnitBox.Visibility =
                TargetLabel.Visibility =
                TargetVal.Visibility = TrLabel.Visibility = Periodic.Visibility = Graphs.Visibility = Visibility.Hidden;
            OkButton.Click -= Add_Click;
            _addFlag = true;
            OkButton.Click += ApplyEditing;
        }

        private void ApplyEditing(object sender, RoutedEventArgs e)
        {
            var buff = new BL.Buffer(BL.ChangesBuffer.CurrentState);
            var tas = buff.TaskBuffer.First(task => task.Description == _parent.Desc.Text);
            var targetTask = new Task(tas);
            targetTask.Description = DescBox.Text;
            targetTask.ImportanceName = (Imp.SelectedItem as ComboBoxItem).Content.ToString();
            targetTask.Importance =
                DAL.SqlRepository.Importancies.Cast<Importance>()
                   .First(imp => imp.ImportanceName == targetTask.ImportanceName);
            targetTask.UrgencyName = (Urg.SelectedItem as ComboBoxItem).Content.ToString();
            targetTask.UpdateUrgency();
            OkButton.Click -= ApplyEditing;
            targetTask.BeginDate = Begin.SelectedDate != null ? (DateTime)Begin.SelectedDate : DateTime.MinValue;
            targetTask.EndDate = End.SelectedDate != null ? (DateTime)End.SelectedDate : DateTime.MaxValue;
            buff.TaskBuffer.Remove(tas);
            buff.TaskBuffer.Add(targetTask);
            foreach (var step in buff.StepBuffer)
            {
                if (step.TaskId == tas.Id)
                    step.ParentTask = targetTask;
            }
            BL.ChangesBuffer.CaptureChanges(buff);
            UpdateTree(targetTask,MoveDirections.None);
            UpdateRawView();
            Discard_Click(sender, e);
        }

        private void UpdateTree(Task displayTask, MoveDirections dir)
        {
            TreeCanvas.Children.Clear();
            var children = displayTask.SelectChildrenTasks();
            var visual = new TaskVisual { Height = 150, Width = 150 };
            LevelUp.IsEnabled = displayTask.ParentId != -1;
            visual.Removing.IsEnabled = LevelUp.IsEnabled;
            visual.Desc.Text = displayTask.Description;
            visual.Date.Text = "С " + displayTask.BeginDate.ToShortDateString() + " по " +
                               displayTask.EndDate.ToShortDateString();
            TreeCanvas.Children.Add(visual);
            int i = children.Count + 1;
            _realCanvasWidth = visual.Width * i;
            OverflowCanvas();
            visual.SetValue(Canvas.LeftProperty, (TreeCanvas.MinWidth - visual.Width) / 2);
            visual.SetValue(Canvas.TopProperty, 20.0);
            visual.PreviewMouseLeftButtonDown += ClearStepChanger;
            _parent = visual;
            if (displayTask.ChildrenAreSteps())
            {
                visual.Add.Click += AddStep;
            }
            else
                visual.Add.Click += AddTask;
            bool evenCount = children.Count % 2 == 0;
            int center = children.Count / 2;
            var centerLeft = (double)visual.GetValue(Canvas.LeftProperty);
            int module = (i - 1) / 2;
            var anim = new ThicknessAnimation(dir == MoveDirections.Down ? new Thickness(0, -1000, 0, 200) : new Thickness(0, 200, 0, -1000), new Thickness(0, 0, 0, 0),
                                                  new Duration(TimeSpan.FromSeconds(1)));
            if (dir != MoveDirections.None)
                visual.BeginAnimation(MarginProperty, anim);
            visual.Background = !displayTask.HasUncomplitedTasks() ? Brushes.LimeGreen : Brushes.Orange;
            visual.Removing.Click += RemoveTask;
            visual.Edit.Click += EditTask;
            
            foreach (var task in children)
            {
                var child = new TaskVisual
                    {
                        Height = 150,
                        Width = 150,
                        Desc = { Text = task.Description },
                        Date =
                            {
                                Text = "С " + task.BeginDate.ToShortDateString() + " по " +
                                       task.EndDate.ToShortDateString()
                            },
                        Field = { Background = !task.HasUncomplitedTasks() ? Brushes.LimeGreen : Brushes.Orange }
                    };
                child.Removing.Visibility = Visibility.Collapsed;
                child.Edit.Visibility = Visibility.Collapsed;
                TreeCanvas.Children.Add(child);
                i--;
                double offset = !evenCount
                                    ? (i == center + 1
                                           ? centerLeft
                                           : (i > center + 1
                                                  ? centerLeft + (visual.Width + visual.Width / children.Count) * module
                                                  : centerLeft - (visual.Width + visual.Width / 2) * i))
                                    : (i > center
                                           ? centerLeft + visual.Width / 2 + visual.Width / children.Count / 2 +
                                             (visual.Width / children.Count + visual.Width) * (module - 1)
                                           : centerLeft - visual.Width / 2 - visual.Width / children.Count / 2 -
                                             (visual.Width / children.Count + visual.Width) * (i - 1));
                module--;


                child.SetValue(Canvas.LeftProperty, offset);
                child.SetValue(Canvas.TopProperty, visual.Height + 180);
                child.Add.Visibility = Visibility.Collapsed;
                var lane = new Line
                    {
                        StrokeThickness = 5,
                        Fill = Brushes.DarkCyan,
                        X1 = (double)visual.GetValue(Canvas.LeftProperty) + visual.Width / 2,
                        Y1 = 20 + visual.Height,
                        Stroke = Brushes.DarkCyan,
                        X2 = (double)child.GetValue(Canvas.LeftProperty) + child.Width / 2,
                        Y2 = (double)child.GetValue(Canvas.TopProperty),
                        SnapsToDevicePixels = true
                    };
                lane.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                TreeCanvas.Children.Add(lane);

                if (dir != MoveDirections.None)
                {
                    child.BeginAnimation(MarginProperty, anim);
                    lane.BeginAnimation(MarginProperty, anim);
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
                        Margin = new Thickness(10.0, 30, 20.0, 0.0),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = visual.Width - 100
                    };
                foreach (var step in steps)
                {
                    var item = new ListBoxItem
                        {
                            Content =
                                step.Description + "\n\n" + step.Criteria.Unit + "\t\t" + step.Criteria.CurrentValue +
                                "/" + step.Criteria.TargetValue,
                            BorderThickness = new Thickness(1),
                            BorderBrush = Brushes.LightBlue,
                            Name = "S" + step.Id
                        };
                    item.Selected += StepSelected;
                    list.Items.Add(item);
                }
                visual.Add.Click += AddStep;
                visual.Field.Children.Add(list);
            }

        }

        private void ClearStepChanger(object sender, RoutedEventArgs e)
        {
            if (_stepChanger != null)
            {
                TreeCanvas.Children.Remove(_stepChanger);
            }
            _stepChanger = null;
        }

        private void StepSelected(object sender, RoutedEventArgs e)
        {
            if (_stepChanger != null)
            {
                TreeCanvas.Children.Remove(_stepChanger);
            }
            int id = Convert.ToInt32((sender as ListBoxItem).Name.Substring(1));
            var step = BL.ChangesBuffer.CurrentState.StepBuffer.First(st => st.Id == id);
            var valueSetter = new Grid() { Width = 120, Height = 80, Background = Brushes.CornflowerBlue };
            valueSetter.Children.Add(new Label
                {
                    Content = "Фиксация прогресса",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                });
            valueSetter.Children.Add(new TextBox
                {
                    Text = step.Criteria.CurrentValue.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Width = 40,
                    MaxLength = step.Criteria.TargetValue.ToString().Length,
                    Name = "ValueChanger"
                });
            var b = new Button()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Width = 25,
                    Height = 25,
                    Content = FindResource("Edit1"),
                    Name = "Q" + id.ToString()
                };
            var b1 = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 25,
                Height = 25,
                Margin = new Thickness(26,0,0,0),
                Content = FindResource("Remove1"),
                Name = "R" + id.ToString()
            };
            var ok = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 40,
                Height = 25,
                Content = "OK",
                Name = "O" + step.Id.ToString()
            };
            valueSetter.Children.Add(b);
            valueSetter.Children.Add(b1);
            valueSetter.Children.Add(ok);
            b1.Click += RemoveStep;
            b.Click += EditStep;
            ok.Click += ChangeCurrentValueOfStep;
            TreeCanvas.Children.Add(valueSetter);
            var p = Mouse.GetPosition(TreeCanvas);
            valueSetter.SetValue(Canvas.LeftProperty, p.X);
            valueSetter.SetValue(Canvas.TopProperty, p.Y);
            _stepChanger = valueSetter;
        }

        private void RemoveStep(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Name.Substring(1));
            var step = BL.ChangesBuffer.CurrentState.StepBuffer.First(st => st.Id == id);
            var buff = new BL.Buffer(BL.ChangesBuffer.CurrentState);
            buff.StepBuffer.Remove(step);
            BL.ChangesBuffer.CaptureChanges(buff);
            var task = BL.ChangesBuffer.CurrentState.TaskBuffer.First(t => t.Description == _parent.Desc.Text);
            UpdateTree(task,MoveDirections.None);
        }

        private void EditStep(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Name.Substring(1));
            var targetStep = new Step(BL.ChangesBuffer.CurrentState.StepBuffer.First(step => step.Id == id));
            Adding.Visibility = Visibility.Visible;
            Adding.IsEnabled = true;
            InAnimation();
            Labl.Content = "Редактирование";
            Chos.Visibility = Visibility.Collapsed;
            DescBox.Text = targetStep.Description;
            Begin.SelectedDate = targetStep.BeginDate;
            End.SelectedDate = targetStep.EndDate;
            Periodic.IsChecked = targetStep.TimeRule.IsPeriodic;
            Graphs.SelectedItem = targetStep.TimeRule.IsPeriodic
                                      ? Graphs.Items.Cast<ComboBoxItem>()
                                              .First(
                                                  i => i.Content.ToString() == targetStep.TimeRule.ScheduleId.ToString())
                                      : null;
            Imp.SelectedItem =
                Imp.Items.Cast<ComboBoxItem>().First(item => item.Content.ToString() == targetStep.ImportanceName);
            Urg.SelectedItem =
                Urg.Items.Cast<ComboBoxItem>().First(item => item.Content.ToString() == targetStep.UrgencyName);
            TargetVal.Text = targetStep.Criteria.TargetValue.ToString();
            UnitBox.Text = targetStep.Criteria.Unit;
            DescBox.Name = "S" + targetStep.Id.ToString();
            UnitBox.Name = "C" + targetStep.CriteriaId.ToString();
            OkButton.Click -= Add_Click;
            _addFlag = true;
            OkButton.Click += ApplyStep;
        }

        private void ApplyStep(object sender, RoutedEventArgs e)
        {
            var buff = new BL.Buffer(BL.ChangesBuffer.CurrentState);
            int id = Convert.ToInt32(DescBox.Name.Substring(1));
            var step = BL.ChangesBuffer.CurrentState.StepBuffer.First(s => s.Id == id);
            var newStep = new Step(step);
            var crit =
                BL.ChangesBuffer.CurrentState.CriteriaBuffer.First(
                    c => c.Id == Convert.ToInt32(UnitBox.Name.Substring(1)));
            var newCrit = new Criteria(crit);
            buff.CriteriaBuffer.Remove(crit);
            buff.CriteriaBuffer.Add(newCrit);
            buff.StepBuffer.Remove(step);
            newStep.Criteria = newCrit;
            newStep.Description = DescBox.Text;
            var tr = BL.ChangesBuffer.CurrentState.TimeRuleBuffer.First(t => t.Id == step.TimeRuleId);
            var newtr = new TimeRule(tr);
            if (Periodic.IsChecked.HasValue && Periodic.IsChecked.Value)
            {
                newtr.IsPeriodic = true;
                newtr.Schedule =
                    DAL.SqlRepository.Schedules.Cast<Models.Schedule>().ToList()
                       .First(sc => sc.Id == Convert.ToInt32((Graphs.SelectedItem as ComboBoxItem).Content.ToString()));
                newtr.ScheduleId = newtr.Schedule.Id;
            }
            else
            {
                newtr.IsPeriodic = false;

            }
            buff.TimeRuleBuffer.Remove(tr);
            buff.TimeRuleBuffer.Add(newtr);
            newStep.TimeRule = newtr;
            buff.StepBuffer.Add(newStep);
            BL.ChangesBuffer.CaptureChanges(buff);
            UpdateTree(BL.ChangesBuffer.CurrentState.TaskBuffer.First(task=>task.Description==_parent.Desc.Text),MoveDirections.None);
            OkButton.Click -= ApplyStep;
            
            Discard_Click(sender, e);
        }

        private void ChangeCurrentValueOfStep(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Name.Substring(1));
            var step = BL.ChangesBuffer.CurrentState.StepBuffer.First(s => s.Id == id);
            var crit = BL.ChangesBuffer.CurrentState.CriteriaBuffer.First(c => step.CriteriaId == c.Id);
            var newStep = new Step(step);
            var newCrit = new Criteria(crit);
            newCrit.CurrentValue = Convert.ToInt32((_stepChanger.Children[1] as TextBox).Text);
            var buff = new BL.Buffer(BL.ChangesBuffer.CurrentState);
            buff.CriteriaBuffer.Remove(crit);
            buff.CriteriaBuffer.Add(newCrit);
            newStep.Criteria = newCrit;
            buff.StepBuffer.Remove(step);
            buff.StepBuffer.Add(newStep);
            BL.ChangesBuffer.CaptureChanges(buff);
            UpdateTree(BL.ChangesBuffer.CurrentState.TaskBuffer.First(t=>t.Description==_parent.Desc.Text),MoveDirections.None);
        }

        private void SelectChild(object sender, RoutedEventArgs e)
        {
            var child = sender as TaskVisual;
            TreeCanvas.Children.Clear();
            UpdateTree(BL.ChangesBuffer.CurrentState.TaskBuffer.First(x => x.Description == child.Desc.Text), MoveDirections.Up);
        }

        private void TreeLoaded()
        {
            BL.ChangesBuffer.Initialize();
            var p = Task.SelectAllTreeTask(BL.Application.CurrentTree.MainTaskId);
            foreach (var task in p)
            {
                RawView.Items.Add(new ListBoxItem { Content = task.Description, Height = 20 });
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

        private void UpdateRawView()
        {
            var p = Task.SelectAllTreeTask(BL.Application.CurrentTree.MainTaskId);
            RawView.Items.Clear();
            foreach (var task in p)
            {
                RawView.Items.Add(new ListBoxItem { Content = task.Description, Height = 20 });
            }
        }

        private void Tasks_OnSelected(object sender, RoutedEventArgs e)
        {
            if (BL.Application.CurrentTree == null) return;
            RawView.Items.Clear();
            foreach (var result in Task.SelectAllTreeTask(BL.Application.CurrentTree.MainTaskId))
            {
                if(!result.ChildrenAreSteps())
                    RawView.Items.Add(new ListBoxItem { Content = result.Description });
            }
        }

        private void TaskSt_OnSelected(object sender, RoutedEventArgs e)
        {
            if (BL.Application.CurrentTree == null) return;
            RawView.Items.Clear();
            foreach (var result in Task.SelectAllTreeTask(BL.Application.CurrentTree.MainTaskId))
            {
                if(result.ChildrenAreSteps())
                    RawView.Items.Add(new ListBoxItem { Content = result.Description });
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
            Adding.Visibility = Visibility.Collapsed;
            if (!_addFlag)
            {
                OkButton.Click += Add_Click;
                _addFlag = true;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (
                BL.ChangesBuffer.CurrentState.TaskBuffer.Any(
                    entity =>
                    Task.GetOldestParent(entity).Id == BL.Application.CurrentTree.MainTaskId &&
                    entity.Description == DescBox.Text))
            {
                MessageBox.Show("Такая цель уже есть в этом дереве!", "Warning", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
            
            var imp =
                DAL.SqlRepository.Importancies.Cast<Importance>()
                   .First(x => ((ComboBoxItem)Imp.SelectedItem).Content.ToString() == x.ImportanceName);
            var buff = new BL.Buffer(BL.ChangesBuffer.CurrentState);
            if (StepButton.IsChecked == true)
            {
                var tr = new TimeRule
                    {
                        IsPeriodic = Periodic.IsChecked == true,
                        Schedule = 
                            DAL.SqlRepository.Schedules.Cast<Models.Schedule>().ToList()
                               .First(x => Periodic.IsChecked == true ? x.Id == Convert.ToInt32(((ComboBoxItem)Graphs.SelectedItem).Content.ToString()):x.Id==11),
                        ScheduleId = Periodic.IsChecked == true ? Convert.ToInt32(((ComboBoxItem)Graphs.SelectedItem).Content.ToString()):11
                    };
                var t = BL.ChangesBuffer.CurrentState.TaskBuffer.First(x => x.Description == _parent.Desc.Text);
                if (
                    BL.ChangesBuffer.CurrentState.StepBuffer
                       .Any(x => x.TaskId == t.Id && x.Description == DescBox.Text))
                {
                    MessageBox.Show("Такой шаг у цели уже есть!", "Warning", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                }
                var crit = new Criteria
                {
                    CurrentValue = 0,
                    TargetValue = Convert.ToInt32(TargetVal.Text),
                    Unit = UnitBox.Text
                };
                var st = new Step
                    {
                        BeginDate = Begin.SelectedDate != null ? (DateTime) Begin.SelectedDate : DateTime.Now,
                        EndDate = End.SelectedDate != null ? (DateTime) End.SelectedDate : Interval.PIOS,
                        Criteria = crit,
                        CriteriaId = crit.Id,
                        Description = DescBox.Text,
                        Importance = imp,
                        ImportanceName = imp.ImportanceName,
                        TimeRule = tr,
                        TimeRuleId = tr.Id,
                        ParentTask = t,
                        TaskId = t.Id
                    };
                st.UpdateUrgency();
                buff.StepBuffer.Add(st);
                BL.ChangesBuffer.CaptureChanges(buff);
                UpdateTree(t,MoveDirections.None);
            }
            else
            {
                var crit = new Criteria
                {
                    CurrentValue = 0,
                    TargetValue = 1,
                    Unit = "default"
                };
                var p = BL.ChangesBuffer.CurrentState.TaskBuffer.First(x => x.Description == _parent.Desc.Text);
                var t = new Task
                    {
                    BeginDate = Begin.SelectedDate != null ? (DateTime)Begin.SelectedDate : DateTime.Now,
                    EndDate = End.SelectedDate != null ? (DateTime)End.SelectedDate : Interval.PIOS,
                    Criteria = crit,
                    CriteriaId = crit.Id,
                    Description = DescBox.Text,
                    Importance = imp,
                    ImportanceName = imp.ImportanceName,
                    Parent = p,
                    ParentId = p.Id,
                    Id = BL.ChangesBuffer.CurrentState.TaskBuffer.Max(taskk=>taskk.Id) + 1
                };
                t.UpdateUrgency();
                buff.TaskBuffer.Add(t);
                BL.ChangesBuffer.CaptureChanges(buff);
                UpdateTree(p,MoveDirections.None);
                
            }
            UpdateRawView();
            Discard_Click(sender,e);
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            DescBox.Clear();
            Begin.SelectedDate = End.SelectedDate = null;
            UnitBox.Clear();
            TargetVal.Clear();
            Labl.Content = "Добавление";
            CritLabel.Visibility =
                UnitLabel.Visibility =
                UnitBox.Visibility =
                TargetLabel.Visibility =
                TargetVal.Visibility = TrLabel.Visibility = Periodic.Visibility = Graphs.Visibility = Chos.Visibility = Visibility.Visible;

            
            OutAnimation();
            Adding.IsEnabled = false;
        }

        private void LevelUp_Click(object sender, RoutedEventArgs e)
        {
            var parent =
                BL.ChangesBuffer.CurrentState.TaskBuffer.First(x => x.Description == _parent.Desc.Text);
            TreeCanvas.Children.Clear();

            UpdateTree(parent.Parent, MoveDirections.Down);
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
                BL.ChangesBuffer.CurrentState.TaskBuffer
                   .Where(x => displayedItems.Contains(x.Description))
                   .OrderByDescending(task => task.Urgency.Value);
            RawView.Items.Clear();
            foreach (var task in sorted)
            {
                RawView.Items.Add(new ListBoxItem { Content = task.Description });
            }
        }

        private void OrderByImportance(object sender, RoutedEventArgs e)
        {
            if (RawView.Items.Count == 0) return;
            var displayedItems = RawView.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
            var sorted =
                BL.ChangesBuffer.CurrentState.TaskBuffer
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
            if (RawView.Items.Count == 0) return;
            var displayedItems = RawView.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
            var sorted =
                BL.ChangesBuffer.CurrentState.TaskBuffer
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
            if (SearchBox.Text.Length == 0 || RawView.Items.Count == 0) return;
            var result = Task.SelectAllTreeTask(BL.Application.CurrentTree.MainTaskId).Where(task => task.Description.Contains(SearchBox.Text));
            
            foreach (var task in RawView.Items.Cast<ListBoxItem>())
            {
                var enumerable = result as IList<Task> ?? result.ToList();
                task.Background = enumerable.FirstOrDefault(x => x.Description == task.Content.ToString()) != null
                                      ? Brushes.CornflowerBlue
                                      : Brushes.White;
            }
            Disc.Visibility = Visibility.Visible;
        }

        private void Disc_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in RawView.Items.Cast<ListBoxItem>())
            {
                item.Background = Brushes.White;
            }
            Disc.Visibility=Visibility.Collapsed;
        }

        private void TaskSelected(object sender, RoutedEventArgs e)
        {
            var source = sender as ListBoxItem;
            var selectedTask =
                BL.ChangesBuffer.CurrentState.TaskBuffer.First(task => task.Description == source.Content.ToString());
            UpdateTree(selectedTask, MoveDirections.None);

        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            BL.ChangesBuffer.Undo();
            UpdateTree(BL.Application.CurrentTree.MainTask,MoveDirections.None);
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            BL.ChangesBuffer.Redo();
            UpdateTree(BL.Application.CurrentTree.MainTask,MoveDirections.None);
        }

        private void Periodic_OnChecked(object sender, RoutedEventArgs e)
        {
            Graphs.IsEnabled = Periodic.IsChecked.HasValue&&Periodic.IsChecked.Value;
        }

        private void TaskButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CritLabel.Visibility =
                UnitLabel.Visibility =
                UnitBox.Visibility =
                TargetLabel.Visibility =
                TargetVal.Visibility = TrLabel.Visibility = Periodic.Visibility = Graphs.Visibility = Visibility.Hidden;
        }

        private void StepButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CritLabel.Visibility =
                UnitLabel.Visibility =
                UnitBox.Visibility =
                TargetLabel.Visibility =
                TargetVal.Visibility = TrLabel.Visibility = Periodic.Visibility = Graphs.Visibility = Visibility.Visible;
        }

        private void Begin_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            End.DisplayDateStart = Begin.SelectedDate;
        }

        private void End_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Begin.DisplayDateEnd = End.SelectedDate;
        }

        private void SendInfo(object sender, RoutedEventArgs e)
        {
            if (BL.Application.CurrentTree == null)
            {
                MessageBox.Show("Open a tree first.", "Synchronization");
                return;
            }
            if (PhoneSync.InfoSender.Send())
                MessageBox.Show("Information succesfully sent to the server.", "Synchronization");
            else
                MessageBox.Show("Couldn't connect to the server.", "Synchronization");

        }
    }
}
