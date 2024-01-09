namespace Savi_Thrift.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IKycRepository KycRepository { get; }
        int SaveChanges();
    }
}
