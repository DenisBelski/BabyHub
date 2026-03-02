using AutoMapper;
using BabyHub.Application.Contracts.Patients;
using BabyHub.Domain.Patients;

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
    }
}