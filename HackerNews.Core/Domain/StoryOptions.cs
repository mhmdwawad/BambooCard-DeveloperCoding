
namespace HackerNews.Core.Domain
{
	public class StoryOptions
	{
        public string HackerNewsApiBaseUrl { get; set; }
        public string BestStoriesIdsEndpoint { get; set; }
		public string BestStoryDetailsEndpoint { get; set; }
		public int PageSize { get; set; }
	}
}
