using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    public enum TransactionStatusEnum
    {
        None = 0,
        Booking = 1,
        NearExpiry = 2,
        Expiry = 3
    }
}
