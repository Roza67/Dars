using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пример_практика.Models
{
    public class OperationsLog
    {
        public int ID { get; set; }
        public DateTime OperationDate { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }

        // Навигационное свойство
        public virtual User User { get; set; }
    }
}
