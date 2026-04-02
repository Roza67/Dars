using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пример_практика.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // Навигационные свойства
        public virtual ICollection<WarehouseTask> Tasks { get; set; } = new HashSet<WarehouseTask>();
        public virtual ICollection<OperationsLog> OperationsLogs { get; set; } = new HashSet<OperationsLog>();
    }
}
