using MVP.Base.BaseView;
using MVP.Base.Common;

namespace MVP.Base.BasePresenter
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BasePresenter<TView, TRepository> : IBasePresenter
        where TView : IBaseView
        where TRepository : class
    {
        #region IView
        protected TView View;
        #endregion

        #region Repository

        protected abstract TRepository Repository { get; }
        #endregion

        #region IBasePresenter

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePresenter&lt;TView, TRepository&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected BasePresenter(TView view)
        {
            View = view;
        }

        #endregion

        #region Actions

        protected abstract void ViewAction();
        protected abstract void UpdateAction();
        protected abstract void DeleteAction();
        protected abstract void InsertAction();
        protected abstract void InitializeAction();
        protected abstract void SearchAction();

        /// <summary>
        /// Does the action.
        /// </summary>
        public virtual void DoAction()
        {
            switch (View.PageMode)
            {
                case PageMode.None:
                    InitializeAction();
                    break;
                case PageMode.Insert:
                    InsertAction();
                    break;
                case PageMode.Delete:
                    DeleteAction();
                    break;
                case PageMode.Update:
                    UpdateAction();
                    break;
                case PageMode.View:
                    ViewAction();
                    break;
                case PageMode.Search:
                    SearchAction();
                    break;
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public abstract void Validate();

        #endregion
    }
}