using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmailId { get; set; }
        public string Password { get; set; }

        // Navigation property for tasks associated with this user
        public ICollection<TaskItem> Tasks { get; set; }
    }

}
