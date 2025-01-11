namespace HackerNews.Common.Exceptions
{
    /// <summary>
    /// The app exteranal api exception.
    /// </summary>
    public class AppExteranalApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppExteranalApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AppExteranalApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AppExteranalApiException(string message)
            : base(message)
        {
        }
    }
}
