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
