using MVP.Base.BasePresenter;
using MVP.Sample.Web.IRepositories;
using MVP.Sample.Web.IViews;
using MVP.Sample.Web.Repositories;

namespace MVP.Sample.Web.Presenters
{
    public class LoginUCPresenter : BasePresenter<ILoginUCView, ILoginUCRepository>
    {
        #region Constructors

        public LoginUCPresenter(ILoginUCView view)
            : base(view)
        {
            Repository = new LoginUCRepository();
        }

        public LoginUCPresenter(ILoginUCView view, ILoginUCRepository repository)
            : base(view, repository)
        {
        }

        #endregion
        
        #region Overrides of BasePresenter<ILoginUCView,ILoginUCRepository>

        protected override void ViewAction()
        {
            if (Repository.ValidateUser(new UserInfo
                {
                    Username = View.Username,
                    Password = View.Password
                }))
            {
                //!TODO: Login using FormAuthentication
            }
        }

        protected override void InitializeAction()
        {
            //!TODO: Reset the input field
            View.Username = View.Password = string.Empty;
        }

        public override void Validate()
        {
        }

        #endregion

        #region Overrides

        protected override void UpdateAction()
        {
        }

        protected override void DeleteAction()
        {
        }

        protected override void InsertAction()
        {
        }

        protected override void SearchAction()
        {
        }
        #endregion
    }
}