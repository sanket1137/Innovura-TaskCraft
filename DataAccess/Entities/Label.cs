using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Label
    {
        public int Id { get; set; }
        public string LabelName { get; set; }
        public string Color { get; set; }
        public TimeSpan TimeSpan { get; set; }

        // Navigation property for tasks associated with this label
        public ICollection<TaskItem> Tasks { get; set; }
    }

}
