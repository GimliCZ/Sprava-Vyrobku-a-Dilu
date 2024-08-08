using AutoMapper;
using SpravaVyrobkuaDilu.Database.Models;
using SpravaVyrobkuaDilu.Models;

namespace SpravaVyrobkuaDilu.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VyrobekModel, VyrobekViewableModel>();

            CreateMap<VyrobekViewableModel, VyrobekModel>()
                .ForMember(dest => dest.Dily, opt => opt.Ignore());
        }
    }
}
