using System;
using AutoMapper;
using FilingPortal.Web.Tests.Common;

namespace FilingPortal.Web.Tests
{
    public class AutoMapperTestProfile : Profile
    {
        public AutoMapperTestProfile()
        {
            CreateMap<FakeRuleEntityEditModel, FakeRuleEntity>()
                .ForMember(x => x.CreatedUser, opt => opt.UseValue("testuser"))
                .ForMember(x => x.CreatedDate, opt => opt.UseValue(new DateTime(2000, 1, 1, 12, 0, 0)));
        }
    }
}
