using AutoMapper;
using BeautySalon.BLL.DTO;
using BeautySalon.DAL.Context;
using BeautySalon.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.Services
{
    public class ServiceService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public ServiceService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Service, ServiceDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(service => service.ServiceId)).
                                ForMember(x => x.Name, x => x.MapFrom(service => service.ServiceName)).
                                ForMember(x => x.HallId, x => x.MapFrom(service => service.HallTypeId)).
                                ForMember(x => x.HallType, x => x.MapFrom(service => service.HallType.HallType1)).
                                ForMember(x => x.CategoryId, x => x.MapFrom(service => service.ServiceCategoryId)).
                                ForMember(x => x.CategoryName, x => x.MapFrom(service => service.ServiceCategory.ServiceCategoryName)).
                                ForMember(x => x.Price, x => x.MapFrom(service => service.Price)).
                                ForMember(x => x.Avaliability, x => x.MapFrom(service => service.Avaliability))
                ;
                cfg.CreateMap<ServiceDTO, Service>().
                                ForMember(x => x.ServiceId, x => x.MapFrom(service => service.Id)).
                                ForMember(x => x.ServiceName, x => x.MapFrom(service => service.Name)).
                                ForMember(x => x.ServiceCategoryId, x => x.MapFrom(service => service.CategoryId)).
                                ForMember(x => x.HallTypeId, x => x.MapFrom(service => service.HallId)).
                                ForMember(x => x.Price, x => x.MapFrom(service => service.Price)).
                                ForMember(x => x.Avaliability, x => x.MapFrom(service => service.Avaliability)).ForAllOtherMembers(x => x.Ignore());
            });
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<ServiceDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(ServiceDTO serviceDTO) => await Task.Run(() => Delete(serviceDTO));
        public async Task UpdateAsync(ServiceDTO serviceDTO) => await Task.Run(() => Update(serviceDTO));
        public async Task<ServiceDTO> CreateAsync(ServiceDTO serviceDTO) => await Task.Run(() => Create(serviceDTO));
        public async Task<ServiceDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));


        public IEnumerable<ServiceDTO> GetAll()
        {
            return mapper.Map<IEnumerable<ServiceDTO>>(repositories.ServiceRepository.GetAll());
        }
        public ServiceDTO GetByID(int id)
        {
            var service = repositories.ServiceRepository.GetByID(id);
            return mapper.Map<ServiceDTO>(service);
        }
        public void Delete(ServiceDTO serviceDTO)
        {
            var serviceToDelete = repositories.ServiceRepository.GetByID(serviceDTO.Id);
            repositories.ServiceRepository.Delete(serviceToDelete);
            repositories.ServiceRepository.Save();
        }

        public void Update(ServiceDTO serviceDTO)
        {
            var serviceToEdit = repositories.ServiceRepository.GetByID(serviceDTO.Id);
            serviceDTO.Id = serviceToEdit.ServiceId;
            serviceToEdit = mapper.Map<Service>(serviceDTO);
            repositories.ServiceRepository.CreateOrUpdate(serviceToEdit);
            repositories.ServiceRepository.Save();
        }

        public ServiceDTO Create(ServiceDTO serviceDTO)
        {
            var service = mapper.Map<Service>(serviceDTO);
            repositories.ServiceRepository.CreateOrUpdate(service);
            repositories.ServiceRepository.Save();
            serviceDTO.Id = service.ServiceId;
            return serviceDTO;
        }

    }

}
