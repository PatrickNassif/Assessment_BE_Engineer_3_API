namespace Assessment_BE_Engineer_3_API.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IFileRepository FileRepository { get; }

        void Save();
    }
}
