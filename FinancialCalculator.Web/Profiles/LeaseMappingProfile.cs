using AutoMapper;
using FinancialCalculator.Services.DTO;
using FinancialCalculator.Web.Dto;

namespace FinancialCalculator.Web.Profiles;

/// <summary>
/// Class used for configuring service-to-view model AutoMapper profiles.
/// </summary>
public class LeaseMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LeaseMappingProfile"/> class.
    /// Constructor for the controller mapping profile class.
    /// </summary>
    public LeaseMappingProfile()
    {
        this.CreateMap<LeaseInputDto, LeaseServiceInputDto>();
    }    
}
