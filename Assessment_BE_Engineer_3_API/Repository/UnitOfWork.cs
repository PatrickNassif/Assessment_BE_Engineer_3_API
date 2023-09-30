using Assessment_BE_Engineer_3_API.Data;
using Assessment_BE_Engineer_3_API.Repository.IRepository;

namespace Assessment_BE_Engineer_3_API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            FileRepository = new FileRepository(_db);
        }
        public IFileRepository FileRepository { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
