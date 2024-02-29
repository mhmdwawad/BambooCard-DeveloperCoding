using AutoMapper;
using HackerNews.Core.Domain;
using HackerNews.Core.Domain.Entities;
using HackerNews.Core.Dto;
using HackerNews.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HackerNews.Core.Services
{
	public class HackerNewsService : IHackerNewsService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IOptions<StoryOptions> _storyOptions;
		private readonly IMemoryCache _cache;
		private readonly IMapper _mapper;

		public HackerNewsService(IHttpClientFactory httpClientFactory
			, IOptions<StoryOptions> storyOptions
			, IMemoryCache cache
			, IMapper mapper)
		{
			_httpClientFactory = httpClientFactory;
			_storyOptions = storyOptions;
			_cache = cache;
			_mapper = mapper;
		}

		public async Task<List<int>> GetStoriesIdsAsync(string title)
		{
			var cacheKey = $"BestStories_{title}";
			if (!(_cache.TryGetValue(cacheKey, out List<int>? cachedBestStories)))
			{
				List<int> storyIds = await GetBestStoryIdsAsync();

				_cache.Set(cacheKey, storyIds, TimeSpan.FromSeconds(60));

				return storyIds;
			}
			return cachedBestStories;
		}

		public async Task<List<StoryEntity>> GetBestStoriesAsync(int count, string title, int pageNumber)
		{
			List<int> storyIds = await GetStoriesIdsAsync(title);

			if (storyIds != null)
			{
				var bestStories = new List<StoryDto>();
				int storiesToReturn = _storyOptions.Value.PageSize;
				int storiesToSkip = (pageNumber - 1) * storiesToReturn;
				int skippedStories = 0;

				if (count < storiesToReturn)
				{
					storiesToReturn = count;
				}

				foreach (var storyId in storyIds)
				{
					var story = await GetStoryDetailsAsync(storyId);

					if (story != null && (string.IsNullOrEmpty(title) || story.Title.Contains(title)))
					{
						if (skippedStories >= storiesToSkip)
						{
							bestStories.Add(story);
						}
						else
						{
							skippedStories++;
						}
					}

					if (bestStories.Count == storiesToReturn)
					{
						break;
					}
				}
				return _mapper.Map<List<StoryEntity>>(bestStories.OrderByDescending(x => x.Score));
			}

			return new List<StoryEntity>();
		}

		private async Task<List<int>> GetBestStoryIdsAsync()
		{
			using (HttpClient httpClient = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
				{
					RequestUri = new Uri(string.Format(_storyOptions.Value.HackerNewsApiBaseUrl, _storyOptions.Value.BestStoriesIdsEndpoint)),
					Method = HttpMethod.Get
				};

				HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
				Stream stream = httpResponseMessage.Content.ReadAsStream();

				StreamReader streamReader = new StreamReader(stream);

				string response = streamReader.ReadToEnd();
				List<int>? storyIds = JsonConvert.DeserializeObject<List<int>>(response);

				if (storyIds == null && !storyIds.Any())
					throw new InvalidOperationException("Empty storyIds");


				return storyIds;
			}
		}

		private async Task<StoryDto?> GetStoryDetailsAsync(int storyId)
		{
			if (!(_cache.TryGetValue(storyId, out StoryDto? bestStory) && bestStory != null))
			{
				using (HttpClient httpClient = _httpClientFactory.CreateClient())
				{
					HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
					{
						RequestUri = new Uri(
							string.Format(_storyOptions.Value.HackerNewsApiBaseUrl,
							string.Format(_storyOptions.Value.BestStoryDetailsEndpoint, storyId))),

						Method = HttpMethod.Get
					};

					HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
					Stream stream = httpResponseMessage.Content.ReadAsStream();

					StreamReader streamReader = new StreamReader(stream);

					string response = streamReader.ReadToEnd();
					StoryDto? stories = JsonConvert.DeserializeObject<StoryDto>(response);

					if (stories == null)
						throw new InvalidOperationException("Empty stories");


					_cache.Set(storyId, stories, TimeSpan.FromSeconds(60));

					return stories;
				}
			}
			return bestStory;
		}
	}
}
