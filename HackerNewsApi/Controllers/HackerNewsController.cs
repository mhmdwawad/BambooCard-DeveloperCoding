using HackerNews.Core.Domain.Entities;
using HackerNews.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net;

namespace HackerNewsApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HackerNewsController : ControllerBase
	{
		private readonly IHackerNewsService _hackerNewsService;
		private readonly ILogger<HackerNewsController> _logger;

		public HackerNewsController(IHackerNewsService hackerNewsService
			, ILogger<HackerNewsController> logger)
		{
			_hackerNewsService = hackerNewsService;
			_logger = logger;
		}

		[HttpGet("best-stories")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StoryEntity>))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<List<StoryEntity>>> GetBestStories(
			[FromQuery][Range(1, 200)][Required][DefaultValue(1)] int count,
			[FromQuery] string title = "",
			int pageNumber = 1)
		{
			try
			{
				List<StoryEntity> bestStories = await _hackerNewsService.GetBestStoriesAsync(count, title, pageNumber);

				if (bestStories == null || !bestStories.Any())
				{
					_logger.LogInformation("No data found ");
					return NoContent();
				}


				return Ok(bestStories);
			}
			catch (Exception ex)
			{
				// Log the exception
				_logger.LogError("Error bestStories API ", ex.Message);
				return StatusCode((int)HttpStatusCode.InternalServerError, "Somthing wrong on the server");
			}
		}
	}
}
