using AutoMapper;
using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Domain.Entity;

namespace Paccagroup.Ecommerce.Transversal.Mapper
{
    public class MapperProfiles: Profile
    {
        public MapperProfiles()
        {
            CreateMap<Customers, CustomersDTO>().ReverseMap();
        }
    }
}
