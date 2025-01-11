namespace HackerNews.Common.Exceptions
{
    /// <summary>
    /// The app exteranal api exception.
    /// </summary>
    public class AppExternalApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppExternalApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AppExternalApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AppExternalApiException(string message)
            : base(message)
        {
        }
    }
}
