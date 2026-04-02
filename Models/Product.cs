using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пример_практика.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }

        // Навигационное свойство для связи с Inventory
        public virtual ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();
    }
}
