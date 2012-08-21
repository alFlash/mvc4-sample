using MVP.Base.BasePresenter;
using MVP.Sample.Web.IViews;

namespace MVP.Sample.Web.Presenters
{
    public class DependencyUC1Presenter : BasePresenter<IDependencyUC1, object>
    {
        public DependencyUC1Presenter(IDependencyUC1 view) : base(view)
        {
        }

        #region Overrides of BasePresenter<IDependencyUC1,object>

        protected override object Repository
        {
            get { return null; }
        }

        protected override void ViewAction()
        {
            
        }

        protected override void UpdateAction()
        {
            
        }

        protected override void DeleteAction()
        {
            
        }

        protected override void InsertAction()
        {
           
        }

        protected override void InitializeAction()
        {
            
        }

        protected override void SearchAction()
        {
            
        }

        public override void Validate()
        {
            
        }

        #endregion
    }
}