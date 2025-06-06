//using CineVault.DataAccessLayer.Context;
//using CineVault.DataAccessLayer.Interfaces;

//namespace CineVault.DataAccessLayer.Repositories
//{
//    public class LoggerRepository : ILoggerRepository
//    {
//        private readonly AppDBContext _context;

//        public LoggerRepository(AppDBContext context)
//        {
//            _context = context;
//        }

//        public void Insert(Logger log)
//        {
//            _context.Logs.Add(log);
//            _context.SaveChanges();
//        }
//    }
//}
