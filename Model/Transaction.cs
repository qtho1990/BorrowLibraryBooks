using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Transaction
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int CostCredit { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
