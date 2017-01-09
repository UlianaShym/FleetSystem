using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.DataAccess.DbContext
{
    public class ContexSingleton
    {
        private static ApplicationDbContext _context;
        public static ApplicationDbContext GetDbContext()
        {
            if (_context != null)
                return _context;
            return _context = new ApplicationDbContext();
        }
    }
}
