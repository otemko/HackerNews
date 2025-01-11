namespace HackerNews.Common.Exceptions
{
    /// <summary>
    /// The app exteranal api exception.
    /// </summary>
    public class AppNotFoundApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppNotFoundApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AppNotFoundApiException(string message)
            : base(message)
        {
        }
    }
}
