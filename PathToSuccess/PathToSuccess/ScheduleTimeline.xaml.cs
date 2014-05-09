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
            var panelList = new List<StackPanel> ();
            var dateCounter = DateTime.Today;
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
                panelList.Add(panel);
                dateCounter = dateCounter.AddDays(1);
            }

            return panelList;
        }
    }
}
