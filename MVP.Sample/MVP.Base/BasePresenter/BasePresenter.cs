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

        /// <summary>
        /// The View action.
        /// </summary>
        protected virtual void ViewAction()
        {
        }

        /// <summary>
        /// The Edit action.
        /// </summary>
        protected virtual void UpdateAction()
        {
        }

        /// <summary>
        /// The Delete action.
        /// </summary>
        protected virtual void DeleteAction()
        {
        }

        /// <summary>
        /// The Create action.
        /// </summary>
        protected virtual void InsertAction()
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected virtual void Initialize()
        {
        }


        /// <summary>
        /// Searches the action.
        /// </summary>
        protected virtual void SearchAction()
        {

        }
        #endregion

        #region Implementation of IBasePresenter

        /// <summary>
        /// Does the action.
        /// </summary>
        public virtual void DoAction()
        {
            switch (View.PageMode)
            {
                case PageMode.None:
                    Initialize();
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

        #region Private Members
        
        #endregion

        #region Members

        #endregion
    }
}