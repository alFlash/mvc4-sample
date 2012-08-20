namespace MVP.Base.BaseUserControl
{
    public interface IMasterPage
    {
        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void ShowErrorMessage(string message);
        /// <summary>
        /// Clears the error message.
        /// </summary>
        void ClearErrorMessage();
    }
}
