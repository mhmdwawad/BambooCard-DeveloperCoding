using HackerNews.Core.Domain.Entities;

namespace HackerNews.Core.Dto
{
	public class BaseResponse
	{
		public List<StoryEntity> BestStories { get; set; } = new List<StoryEntity>();
	}
}
