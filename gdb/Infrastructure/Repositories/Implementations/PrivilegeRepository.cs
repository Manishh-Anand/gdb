using GDB.App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Infrastructure.Repositories.Implementations
{
    public class PrivilegeRepository
    {
        private static Dictionary<AccountPrivilege, decimal> _dailyLimitPool;

        static PrivilegeRepository()
        {
            _dailyLimitPool = new Dictionary<AccountPrivilege, decimal>();
            _dailyLimitPool.Add(AccountPrivilege.Premium, 100000m);
            _dailyLimitPool.Add(AccountPrivilege.Gold, 50000m);
            _dailyLimitPool.Add(AccountPrivilege.Silver, 25000m);
        }

    }
}
