
namespace HackerNews.Core.Dto
{
	public class StoryDto
	{
		public string By { get; set; } = string.Empty;
		public long Descendants { get; set; }
		public long Id { get; set; }
		public List<long> Kids { get; set; } = new();
		public long Score { get; set; }
		public long Time { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;
		public string Url { get; set; } = string.Empty;
	}
}
