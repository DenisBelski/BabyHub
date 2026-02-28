using AutoMapper;
using BabyHub.Application.Contracts.Patients;
using BabyHub.Domain.Patients;
using BabyHub.Domain.Shared.Enums;

namespace Sot.ProductService;

public class ApplicationAutoMapperProfile : Profile
{
    public ApplicationAutoMapperProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new PatientNameDto
            {
                Id = src.Id,
                Family = src.FamilyName,
                Given = src.GivenNames.Select(x => x.Value).ToList(),
                Use = src.NameUsage
            }));

        CreateMap<Patient, PatientNameDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.FamilyName))
            .ForMember(dest => dest.Use, opt => opt.MapFrom(src => src.NameUsage))
            .ForMember(dest => dest.Given, opt => opt.MapFrom(src =>
                src.GivenNames.Select(x => x.Value).ToList()));

        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src));
    }
}