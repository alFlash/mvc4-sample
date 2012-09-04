using MVP.Base.BasePresenter;
using MVP.Sample.Web.IViews;

namespace MVP.Sample.Web.Presenters
{
    public class DependencyUC2Presenter: BasePresenter<IDependencyUC2View, object>
    {
        public DependencyUC2Presenter(IDependencyUC2View view) : base(view)
        {
        }

        #region Overrides of BasePresenter<IDependencyUC2View,object>

        protected override object Repository
        {
            get { return null; }
        }

        protected override void ViewAction()
        {
            //throw new NotImplementedException();
        }

        protected override void UpdateAction()
        {
            //throw new NotImplementedException();
        }

        protected override void DeleteAction()
        {
            //throw new NotImplementedException();
        }

        protected override void InsertAction()
        {
            //throw new NotImplementedException();
        }

        protected override void InitializeAction()
        {
            //throw new NotImplementedException();
        }

        protected override void SearchAction()
        {
            //throw new NotImplementedException();
        }

        public override void Validate()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}