using MVP.Base.BasePresenter;

namespace MVP.Base.BaseUserControl
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseUserControl
    {
        /// <summary>
        /// Gets or sets the presenter.
        /// </summary>
        /// <value>
        /// The presenter.
        /// </value>
        IBasePresenter Presenter { get; set; }
    }
}
