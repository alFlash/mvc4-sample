namespace MVP.Base.Common
{
    /// <summary>
    /// 
    /// </summary>
    public enum PageMode
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        View,
        /// <summary>
        /// 
        /// </summary>
        Insert,
        /// <summary>
        /// 
        /// </summary>
        Update,
        /// <summary>
        /// 
        /// </summary>
        Delete,
        /// <summary>
        /// 
        /// </summary>
        Search,
    }

    public enum CustomMode
    {
        ExportExcel,
        ValidateMessage,
        PageReset,
        PageEdit,
        PageView
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 
        /// </summary>
        Success = 0,
        /// <summary>
        /// 
        /// </summary>
        Error = 1,
        /// <summary>
        /// 
        /// </summary>
        Unexpected = 2,
        /// <summary>
        /// 
        /// </summary>
        Warning = 3,
    }

    public enum PageStatus
    {
        None,
        Failed,
        Success
    }
}