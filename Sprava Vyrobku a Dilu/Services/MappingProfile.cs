using AutoMapper;
using Sprava_Vyrobku_a_Dilu.Database.Models;
using Sprava_Vyrobku_a_Dilu.Models;

namespace Sprava_Vyrobku_a_Dilu.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VyrobekModel, VyrobekViewModel>()
                .ForMember(dest => dest.CountOfDily, opt => opt.MapFrom(src => src.Dily.Count));

            CreateMap<VyrobekViewModel, VyrobekModel>()
                .ForMember(dest => dest.Dily, opt => opt.Ignore());
        }
    }
}
