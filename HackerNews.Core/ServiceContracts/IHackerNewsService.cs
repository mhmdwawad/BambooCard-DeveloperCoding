using HackerNews.Core.Domain.Entities;

namespace HackerNews.Core.ServiceContracts
{
	public interface IHackerNewsService
	{
		Task<List<StoryEntity>> GetBestStoriesAsync(int count, string title, int pageNumber);
	}
}
