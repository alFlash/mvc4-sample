using MVP.Base.BaseRepository;

namespace MVP.Sample.Web.IRepositories
{
    public interface ILoginUCRepository: IRepository<UserInfo>
    {
        bool ValidateUser(UserInfo user);
    }
}
