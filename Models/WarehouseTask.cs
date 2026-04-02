using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пример_практика.Models
{
    public class WarehouseTask
    {
        public int ID { get; set; }
        public string TaskType { get; set; }
        public string Status { get; set; }
        public int AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }

        // Навигационное свойство
        public virtual User AssignedUser { get; set; }
    }
}
