using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Transfer
{
    public class TransactionTransfer
    {
        public string Id { get; set; }
        public string BookName { get; set; }
        public string CreatedTime { get; set; }
        public string ExpiryTime { get; set; }
        public TransactionStatusEnum Status { get; set; }
        public int CostCredit { get; set; }
        public int ExpiryCredit {  get; set; }
    }
}
