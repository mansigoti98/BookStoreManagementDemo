using AutoMapper;
using BookStoreManagement_1.Models;
using BookStoreManagement_1.ViewModels.Customer;

namespace BookStoreManagement_1.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerCreateViewModel>()
                .ReverseMap()
                .ForMember(des => des.FullName, opt => opt.MapFrom(src => src.Name));

            //CreateMap<Book, BookViewModel>().ReverseMap();
        }
    }
}
