using nGantt.GanttChart;
using nGantt.PeriodSplitter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace nGanttDemoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int GantLenght { get; set; }
        private ObservableCollection<ContextMenuItem> ganttTaskContextMenuItems = new ObservableCollection<ContextMenuItem>();
        private ObservableCollection<SelectionContextMenuItem> selectionContextMenuItems = new ObservableCollection<SelectionContextMenuItem>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //nGanttChart = new nGantt.GanttControl();
            GantLenght = 20;

            DateTime minDate = DateTime.Parse("2012-02-01");
            DateTime maxDate = minDate.AddDays(GantLenght);

            // Set selection -mode
            nGanttChart.TaskSelectionMode = nGantt.GanttControl.SelectionMode.Single;
            // Enable GanttTasks to be selected
            nGanttChart.AllowUserSelection = true;

            // listen to the GanttRowAreaSelected event
            nGanttChart.GanttRowAreaSelected += new EventHandler<PeriodEventArgs>(ganttControl1_GanttRowAreaSelected);

           
            ganttTaskContextMenuItems.Add(new ContextMenuItem(ViewReport, "View Report"));
            nGanttChart.GanttTaskContextMenuItems = ganttTaskContextMenuItems;

           

            nGanttChart.ClearGantt();
            DateTime MinDate = DateTime.Parse("2012-02-01");
            DateTime MaxDate = MinDate.AddDays(GantLenght);
            this.CreateData(MinDate, MaxDate);


        }

       
        private void ViewReport(GanttTask ganttTask)
        {
           


        }

        

        private System.Windows.Media.Brush DetermineBackground(TimeLineItem timeLineItem)
        {
            if (timeLineItem.End.Date.DayOfWeek == DayOfWeek.Saturday || timeLineItem.End.Date.DayOfWeek == DayOfWeek.Sunday)
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightBlue);
            else
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }

        void ganttControl1_GanttRowAreaSelected(object sender, PeriodEventArgs e)
        {
            MessageBox.Show(e.SelectionStart.ToString() + " -> " + e.SelectionEnd.ToString());
        }



        private void CreateData(DateTime minDate, DateTime maxDate)
        {
            // Set max and min dates
            nGanttChart.Initialize(minDate, maxDate);

            // Create timelines and define how they should be presented
            nGanttChart.CreateTimeLine(new PeriodYearSplitter(minDate, maxDate), FormatYear);
            nGanttChart.CreateTimeLine(new PeriodMonthSplitter(minDate, maxDate), FormatMonth);
            var gridLineTimeLine = nGanttChart.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDay);
            nGanttChart.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDayName);

            // Set the timeline to atatch gridlines to
            nGanttChart.SetGridLinesTimeline(gridLineTimeLine, DetermineBackground);

            // Create and data
            var rowgroup1 = nGanttChart.CreateGanttRowGroup("Group");
            var row1 = nGanttChart.CreateGanttRow(rowgroup1, "GanttRow 1");
            nGanttChart.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2012-02-01"), End = DateTime.Parse("2012-02-03"), Name = "GanttRow 1:GanttTask 1", TaskProgressVisibility = System.Windows.Visibility.Hidden });
            nGanttChart.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2012-02-05"), End = DateTime.Parse("2012-03-01"), Name = "GanttRow 1:GanttTask 2" });
            nGanttChart.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2012-06-01"), End = DateTime.Parse("2012-06-15"), Name = "GanttRow 1:GanttTask 3" });

            var rowgroup2 = nGanttChart.CreateGanttRowGroup("ExpandableGanttRowGroup", true);
            var row2 = nGanttChart.CreateGanttRow(rowgroup2, "GanttRow 2");
            var row3 = nGanttChart.CreateGanttRow(rowgroup2, "GanttRow 3");
            nGanttChart.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2012-02-10"), End = DateTime.Parse("2012-03-10"), Name = "GanttRow 2:GanttTask 1" });
            nGanttChart.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2012-03-25"), End = DateTime.Parse("2012-05-10"), Name = "GanttRow 2:GanttTask 2" });
            nGanttChart.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2012-06-10"), End = DateTime.Parse("2012-09-15"), Name = "GanttRow 2:GanttTask 3", PercentageCompleted = 0.375 });
            nGanttChart.AddGanttTask(row3, new GanttTask() { Start = DateTime.Parse("2012-01-07"), End = DateTime.Parse("2012-09-15"), Name = "GanttRow 3:GanttTask 1", PercentageCompleted = 0.5 });

        }



        private string FormatYear(Period period)
        {
            return period.Start.Year.ToString();
        }

        private string FormatMonth(Period period)
        {
            return period.Start.ToString("MMMM");
        }

        private string FormatDay(Period period)
        {
            return period.Start.Day.ToString();
        }

        private string FormatDayName(Period period)
        {
            return period.Start.DayOfWeek.ToString();
        }



    }
}
