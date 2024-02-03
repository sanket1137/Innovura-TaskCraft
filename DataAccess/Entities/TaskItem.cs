using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string State { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Foreign key for the associated label
        public int LabelId { get; set; }
        public Label Label { get; set; }

        // Foreign key for the associated user
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
