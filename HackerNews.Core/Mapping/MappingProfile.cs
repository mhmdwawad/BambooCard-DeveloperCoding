using AutoMapper;
using HackerNews.Core.Domain.Entities;
using HackerNews.Core.Dto;

namespace HackerNews.Core.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<StoryDto, StoryEntity>()
				.ForMember(dest => dest.Time, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.Time).DateTime))
				.ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Kids.Count))
				.ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Url))
				.ForMember(dest => dest.PostedBy, opt => opt.MapFrom(src => src.By));
		}
	}
}
