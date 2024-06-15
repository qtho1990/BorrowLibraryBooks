using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    public enum CreateTransactionStatusEnum
    {
        None = 0,
        Success = 1,
        NotEnoughCredit = 2,
        MaximumCanBorrow = 3,
        NoLongerBook = 4,
        Other = 5,
    }
}
