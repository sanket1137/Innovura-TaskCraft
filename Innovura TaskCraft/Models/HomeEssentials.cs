using Business_Layer.IServices;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innovura_TaskCraft.Models
{
    public interface IHomeEssentials
    {
        IList<TaskItem> Tasks { get; set; }
        IList<Label> Labels { get; set; }

    }
    public class HomeEssentials: IHomeEssentials
    {
        private readonly ITaskManager _taskManager;
        public HomeEssentials(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public IList<TaskItem> Tasks { get; set; }
        public IList<Label> Labels { get; set; }
        public object ChartData { get; set; } // Replace with the appropriate data structure for your chart

        //public async Task OnGetAsync()
        //{
        //    // Use your data access layer or service to fetch task and label data
        //    //Tasks = await _taskService.GetTasksAsync(userId); // Replace with your actual method
        //    //Labels = await _labelService.GetLabelsAsync(); // Replace with your actual method

        //    // Prepare data for the chart based on your requirements
        //    //ChartData = PrepareChartData(Tasks, Labels); // Implement this method
        //}

        private object PrepareChartData(IList<TaskItem> tasks, IList<Label> labels)
        {
            // Implement logic to prepare data for your chosen chart library
            // This might involve grouping tasks by label, counting completed tasks,
            // calculating timespans, etc.

            // Example using a dictionary for basic label-count data:
            var chartData = new Dictionary<string, int>();
            foreach (var task in tasks)
            {
                if (!chartData.ContainsKey(task.Label.LabelName))
                {
                    chartData.Add(task.Label.LabelName, 0);
                }
                //if (task.State == task.State.Completed)
                //{
                //    chartData[task.Label.Name]++;
                //}
            }
            return chartData;
        }
    }
}
