namespace HackerNews.Core.Domain.Entities
{
	public class StoryEntity
	{
		public string Title { get; set; } = string.Empty;
		public string Uri { get; set; } = string.Empty;
		public string PostedBy { get; set; } = string.Empty;
		public DateTime Time { get; set; }
		public int Score { get; set; }
		public long CommentCount { get; set; }
	}
}
