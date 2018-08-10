using AutoMapper;
using HAC.Helpdesk.Model.Account;
using HAC.Helpdesk.Services.ViewModels;

namespace HAC.Helpdesk.SimpleMembership.Repository.EF.Test.AutoMapper
{
    public class UserProfileAdministrationProfile : Profile
    {
        protected override void Configure()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            CreateMap<UserProfileAdministrationGetAllDto, UserProfile>()
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedById))
                .ForMember(x => x.CreatedDate, n => n.MapFrom(m => m.CreatedDate))
                // .ForMember(x => x.DeletedDate, n => n.MapFrom(m => m.))
                .ForMember(x => x.EditedById, n => n.MapFrom(m => m.EditedById))
                .ForMember(x => x.Email, n => n.MapFrom(m => m.Email))
                .ForMember(x => x.MobilePhone, n => n.MapFrom(m => m.MobilePhone))
                .ForMember(x => x.HomePhone, n => n.MapFrom(m => m.HomePhone))
                .ForMember(x => x.LastLoginDate, n => n.MapFrom(m => m.LastLoginDate))
                .ForMember(x => x.ModifiedDate, n => n.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.RowVersion, n => n.MapFrom(m => m.RowVersion))
                .ForMember(x => x.ObjectState, n => n.Ignore())
                .ForMember(x => x.TwoFactorSecret, n => n.MapFrom(m => m.TwoFactorSecret))
                .ForMember(x => x.UserName, n => n.MapFrom(m => m.UserName))
                .ForMember(x => x.UserUid, n => n.MapFrom(m => m.UserUid))
                .ForMember(x => x.UserId, n => n.MapFrom(m => m.UserId))
                .ReverseMap();
        }
    }
}