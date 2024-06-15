using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Book
    {
        public int Id { get; set; }
        /// <summary>
        /// Book name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Quantity book
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Credit 1 item
        /// </summary>
        public int Credit { get; set; }

        /// <summary>
        /// Day can borrow, after will expiry transaction
        /// </summary>
        public int Expiry { get; set; }
    }
}
