using AutoMapper;
using SmartBill.APIService.Entities;
using SmartBillApi.DataTransferObject.ViewModel;
using System.Reflection;

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
        }

    }
}
