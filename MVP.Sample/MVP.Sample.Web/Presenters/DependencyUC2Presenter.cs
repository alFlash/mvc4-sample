﻿using MVP.Base.BasePresenter;
using MVP.Base.BaseRepository;
using MVP.Sample.Web.IRepositories;
using MVP.Sample.Web.IViews;
using MVP.Sample.Web.Repositories;

namespace MVP.Sample.Web.Presenters
{
    public class DependencyUC2Presenter : BasePresenter<IDependencyUC2View, IDependencyUC2Repository>
    {
        #region Constructors
        public DependencyUC2Presenter(IDependencyUC2View view)
            : base(view)
        {
            Repository = new DependencyUC2Repository();
        }

        public DependencyUC2Presenter(IDependencyUC2View view, IDependencyUC2Repository repository): base(view, repository)
        {
        }
        #endregion

        #region Overrides of BasePresenter<IDependencyUC2View,object>

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