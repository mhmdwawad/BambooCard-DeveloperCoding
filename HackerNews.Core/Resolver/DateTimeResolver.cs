using AutoMapper;
using HackerNews.Core.Domain.Entities;
using HackerNews.Core.Dto;

namespace HackerNews.Core.Resolver
{
	public class DateTimeResolver : IValueResolver<StoryEntity, StoryDto, DateTime>
	{
		public DateTime Resolve(StoryEntity source, StoryDto destination, DateTime destMember, ResolutionContext context)
		{
			//return DateTimeOffset.FromUnixTimeSeconds(source.Time).UtcDateTime;

			return DateTime.UtcNow;
		}
	}
}
