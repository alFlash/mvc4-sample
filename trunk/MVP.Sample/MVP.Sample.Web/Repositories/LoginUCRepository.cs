using MVP.Base.BaseRepository;
using MVP.Sample.Web.IRepositories;

namespace MVP.Sample.Web.Repositories
{
    public class LoginUCRepository: BaseRepository<UserInfo>, ILoginUCRepository
    {
        #region Implementation of ILoginUCRepository

        public bool ValidateUser(UserInfo user)
        {
            return !string.IsNullOrWhiteSpace(user.Username) 
                && !string.IsNullOrWhiteSpace(user.Password) 
                && user.Username.Equals("testuser") 
                && user.Password.Equals("P@ssword123");
        }

        #endregion
    }
}