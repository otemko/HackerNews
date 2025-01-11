using AutoMapper;
using HackerNews.BusinessLogic.Models;
using HackerNews.Common.Converters;
using HackerNews.SecondaryAdapters.Models;

namespace HackerNews.BusinessLogic.MappingProfiles
{
    /// <summary>
    /// The stories profile.
    /// </summary>
    public class StoriesProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoriesProfile"/> class.
        /// </summary>
        public StoriesProfile()
        {
            CreateMap<StoryApiModel, StoryDto>()
                .ForMember(x => x.PostedBy, y => y.MapFrom(z => z.By))
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Title))
                .ForMember(x => x.Uri, y => y.MapFrom(z => z.Url))
                .ForMember(x => x.Time, y => y.MapFrom(z => ConvertUnixTimeToDateTime(z.Time)))
                .ForMember(x => x.Score, y => y.MapFrom(z => z.Score))
                .ForMember(x => x.CommentCount, y => y.MapFrom(z => z.Kids.Count()));
        }

        /// <summary>
        /// Converts the unix time to date time.
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>A DateTime? .</returns>
        private DateTime? ConvertUnixTimeToDateTime(long? unixTime)
        {
            if (unixTime == null)
            {
                return null;
            }

            return UnixTimeConverter.ConvertUnixTimeToDateTime(unixTime.Value);
        }
    }
}
