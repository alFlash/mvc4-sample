using MVP.Base.BaseView;

namespace MVP.Sample.Web.IViews
{
    public interface ILoginUCView : IBaseView
    {
        string Username { get; set; }
        string Password { get; set; }
        string Message { get; set; }
    }
}
