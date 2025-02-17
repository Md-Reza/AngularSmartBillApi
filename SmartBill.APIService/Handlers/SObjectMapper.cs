using AutoMapper;
using SmartBill.APIService.Entities;
using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Handlers
{
    internal sealed class SObjectMapper : Profile
    {
        public SObjectMapper()
        {
            CreateMap<User, LoginSessionViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<User, UserViewModel>();
            CreateMap<Supplier, SupplierViewModel>();
            CreateMap<SubCategory, SubCategoryViewModel>()
                .ForMember(dest => dest.CategoryViewModel, opt => opt.MapFrom(src => src.Category));
            CreateMap<UnitType, UnitTypeViewModel>();
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.SupplierViewModel, opt => opt.MapFrom(src => src.Supplier))
               .ForMember(dest => dest.CategoryViewModel, opt => opt.MapFrom(src => src.Category))
               .ForMember(dest => dest.UnitTypeViewModel, opt => opt.MapFrom(src => src.UnitType));
        }

    }
}
