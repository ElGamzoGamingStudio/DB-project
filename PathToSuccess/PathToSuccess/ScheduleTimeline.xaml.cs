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
                    Background = new SolidColorBrush(Colors.Wheat),
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
                    Text = dateCounter.DayOfWeek.ToString() + " " + dateCounter.ToShortDateString(),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Colors.Teal),
                    FontSize = 14,
                    TextAlignment = TextAlignment.Center,
                };
                panel.Children.Add(dayTextBlock);

                var stepsBindings = TimeBinding.GetTBofDay(dateCounter.Day, dateCounter.Month, dateCounter.Year,BL.Application.CurrentTree);
               // var thisDaySteps = new List<Step>();
                //thisDaySteps.AddRange(steps.Select(binding => binding.Step));

                foreach (var binding in stepsBindings)
                {
                    var step = binding.Step;
                   
                    var stepPanel = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                        Background = new SolidColorBrush(Colors.Coral),
                        Margin = new Thickness(0, 1, 0, 0),
                    };
                    var descriptionText = new TextBlock() { Text = step.Description, HorizontalAlignment = HorizontalAlignment.Center };
                    var stepImportance = new TextBlock() { Text = step.ImportanceName, HorizontalAlignment = HorizontalAlignment.Center };
                    var criteriaText = new TextBlock()
                    {
                        Text = step.Criteria.CurrentValue + " / " + step.Criteria.TargetValue,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    var timeText = new TextBlock()
                    {
                        Text = binding.Time.ToShortTimeString() + " - " + binding.Time.AddHours(1).ToShortTimeString(),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Background = new SolidColorBrush(Colors.CadetBlue),
                        TextAlignment = TextAlignment.Center,
                    };
                    
                    stepPanel.Children.Add(descriptionText);
                    stepPanel.Children.Add(stepImportance);
                    stepPanel.Children.Add(criteriaText);
                    stepPanel.Children.Add(timeText);

                    panel.Children.Add(stepPanel);
                }


                panelList.Add(panel);
                dateCounter = dateCounter.AddDays(1);
            }

            return panelList;
        }
    }
}
