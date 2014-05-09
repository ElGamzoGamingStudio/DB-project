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
using PathToSuccess.DAL;
using PathToSuccess.BL;
using PathToSuccess.Models;
using PathToSuccess.Schedule;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for ScheduleTimeline.xaml
    /// </summary>
    public partial class ScheduleTimeline : Window
    {
        public ScheduleTimeline()
        {
            InitializeComponent();

            var list = CreateScheduleList();
            foreach (var panel in list)
            {
                TimelinePanel.Children.Add(panel);
            }
        }

        private IEnumerable<StackPanel> CreateScheduleList()
        {
            ScheduleManager.Initialize();
            ScheduleManager.CreateSchedule();
            var panelList = new List<StackPanel>();
            var dateCounter = DateTime.Today;
            var stepsToSchedule = ScheduleManager.withTB;
            while (dateCounter < DateTime.Today.AddMonths(1))
            {
                var panel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Background = new SolidColorBrush(Colors.DarkCyan),
                    CanHorizontallyScroll = false,
                    CanVerticallyScroll = true,
                    MinWidth = 200,
                    MinHeight = 280,
                    MaxHeight = 280,
                    MaxWidth = 200,
                    Margin = new Thickness(0, 0, 5, 0),
                };
                var dayTextBlock = new TextBlock()
                {
                    Text = dateCounter.DayOfWeek.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                panel.Children.Add(dayTextBlock);

                //TimeBinding.GetTBbyStepID(step.Id)[0].Time >= dateCounter
                //                                              && 
                //please give me the proper collection
                var thisDaySteps = stepsToSchedule.Where(step => TimeBinding.GetTBbyStepID(step.Id)[0].Time < dateCounter.AddDays(1))
                                                              .ToList(); //pls huilo
                foreach (var step in thisDaySteps)
                {
                    var stepPanel = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                        Background = new SolidColorBrush(Colors.Green),

                    };
                    var descriptionText = new TextBlock() { Text = step.Description, HorizontalAlignment = HorizontalAlignment.Center };
                    var stepImportance = new TextBlock() { Text = step.ImportanceName, HorizontalAlignment = HorizontalAlignment.Center };
                    var criteriaText = new TextBlock()
                    {
                        Text = step.Criteria.CurrentValue + " / " + step.Criteria.TargetValue,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    var timePanel = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Background = new SolidColorBrush(Colors.GreenYellow),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    var start = new TextBlock() { Text = step.BeginDate.ToShortTimeString(), HorizontalAlignment = HorizontalAlignment.Center };
                    var finish = new TextBlock() { Text = " - " + step.EndDate.ToShortTimeString(), HorizontalAlignment = HorizontalAlignment.Center };
                    timePanel.Children.Add(start);
                    timePanel.Children.Add(finish);

                    stepPanel.Children.Add(descriptionText);
                    stepPanel.Children.Add(stepImportance);
                    stepPanel.Children.Add(criteriaText);
                    stepPanel.Children.Add(timePanel);

                    panel.Children.Add(stepPanel);
                }


                panelList.Add(panel);
                dateCounter = dateCounter.AddDays(1);
            }

            return panelList;
        }
    }
}
