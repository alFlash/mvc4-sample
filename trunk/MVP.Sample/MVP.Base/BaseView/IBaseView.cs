using MVP.Base.Common;

namespace MVP.Base.BaseView
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseView
    {
        /// <summary>
        /// Gets or sets the page mode.
        /// </summary>
        /// <value>
        /// The page mode.
        /// </value>
        PageMode PageMode { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        PageStatus PageStatus { get; set; }
    }
}