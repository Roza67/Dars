using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пример_практика.Models
{
    public class Inventory
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int LocationID { get; set; }
        public int Quantity { get; set; }

        // Навигационные свойства
        public virtual Product Product { get; set; }
        public virtual Location Location { get; set; }
    }
}
