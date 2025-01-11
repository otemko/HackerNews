namespace HackerNews.Common.Converters
{
    /// <summary>
    /// The unix time converter.
    /// </summary>
    public static class UnixTimeConverter
    {
        /// <summary>
        /// Converts the unix time to date time.
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime ConvertUnixTimeToDateTime(long unixTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            return dateTimeOffset.DateTime;
        }
    }
}
