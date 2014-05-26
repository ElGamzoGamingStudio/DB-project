using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using System.Windows.Interop.D3DImage;
using PathToSuccess.BL;
using PathToSuccess.Models;
using Task = PathToSuccess.Models.Task;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        public Statistics()
        {
            InitializeComponent();

            var trees = Models.Tree.FindTreesForUser(BL.Application.CurrentUser);
            foreach (var tree in trees)
            {
                var undoneTasks = Task.Select(x => !x.Criteria.IsCompleted());
                var allTasks = Task.SelectAllTreeTask(tree.TreeId);
                var treeNameText = new TextBlock()
                {
                    Text = tree.TreeUser == BL.Application.CurrentUser ? tree.Name : "[" + tree.Name + "]",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 16,
                    Foreground = tree.TreeUser == BL.Application.CurrentUser ? new SolidColorBrush(Colors.Goldenrod) : new SolidColorBrush(Colors.SlateGray)
                };
                var treeDescriptionText = new TextBlock()
                {
                    Text = tree.Description,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                };
                var treeTaskStatistics = new TextBlock()
                {
                    Text = undoneTasks.Count + " / " + allTasks.Count,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                var treeInfo = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                treeInfo.Children.Add(treeNameText);
                treeInfo.Children.Add(treeTaskStatistics);
                //treeInfo.Children.Add(treeDescriptionText);
                var treeElement = new Button()
                {
                    Background = new SolidColorBrush(Colors.Tomato),
                    Width = 150,
                    Height = 150,
                    Content = treeInfo,
                    Tag = tree.TreeId
                };
                
                StatisticBlockPanel.Children.Add(treeElement);
            }
        }
    }
}
