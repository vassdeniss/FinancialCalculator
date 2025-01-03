using AutoMapper;
using FinancialCalculator.Services.DTO;
using FinancialCalculator.Web.Dto;

namespace FinancialCalculator.Web.Profiles;

/// <summary>
/// Class used for configuring service-to-view model AutoMapper profiles.
/// </summary>
public class RefinanceMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefinanceMappingProfile"/> class.
    /// Constructor for the controller mapping profile class.
    /// </summary>
    public RefinanceMappingProfile()
    {
        this.CreateMap<RefinanceInputDto, RefinanceServiceInputDto>();
    }    
}
