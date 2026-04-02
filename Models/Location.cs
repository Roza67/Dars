using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пример_практика.Models
{
    public class Location
    {
        public int ID { get; set; }
        public string Row { get; set; }
        public string Rack { get; set; }
        public string Shelf { get; set; }
        public string FullCode { get; set; }
        public bool IsOccupied { get; set; }

        // Навигационное свойство для связи с Inventory
        public virtual ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();
    }
}
