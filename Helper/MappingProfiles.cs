using AutoMapper;
using PokemonReviewApi.Dtos;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Reviewer, src => src.MapFrom(x => (x.Reviewer.FirstName + " " + x.Reviewer.LastName)));

            CreateMap<PokemonDto, Pokemon>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CountryDto, Country>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<ReviewerDto, Reviewer>();
            CreateMap<ReviewDto, Review>()
                .ForMember(dest => dest.Reviewer, opt => opt.Ignore());

        }
    }
}
