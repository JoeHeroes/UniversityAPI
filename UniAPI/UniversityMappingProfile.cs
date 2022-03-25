using AutoMapper;
using UniAPI.Entites;
using UniAPI.Models;

namespace UniAPI
{
    public class UniversityMappingProfile : Profile
    {
        public UniversityMappingProfile()
        {
            CreateMap<University, UniversityDto>()
             .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
             .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
             .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Student, StudentDto>()
            .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
            .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
            .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));


            CreateMap<CreateUniversityDto, University>()
            .ForMember(m => m.Address, c => c.MapFrom(dto => new Address() { City = dto.City, PostalCode = dto.PostalCode, Street = dto.PostalCode }));



        }



    }
}
