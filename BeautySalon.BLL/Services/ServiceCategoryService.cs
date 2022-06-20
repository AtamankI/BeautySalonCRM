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
    public class ServiceCategoryService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public ServiceCategoryService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ServiceCategory, ServiceCategoryDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(serviceCategory => serviceCategory.ServiceCategoryId)).
                                ForMember(x => x.Name, x => x.MapFrom(serviceCategory => serviceCategory.ServiceCategoryName)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<ServiceCategoryDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(ServiceCategoryDTO serviceCategoryDTO) => await Task.Run(() => Delete(serviceCategoryDTO));
        public async Task UpdateAsync(ServiceCategoryDTO serviceCategoryDTO) => await Task.Run(() => Update(serviceCategoryDTO));
        public async Task<ServiceCategoryDTO> CreateAsync(ServiceCategoryDTO serviceCategoryDTO) => await Task.Run(() => Create(serviceCategoryDTO));
        public async Task<ServiceCategoryDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<ServiceCategoryDTO> GetAll()
        {
            return mapper.Map<IEnumerable<ServiceCategoryDTO>>(repositories.ServiceCategoryRepository.GetAll());
        }
        public ServiceCategoryDTO GetByID(int id)
        {
            var serviceCategory = repositories.ServiceCategoryRepository.GetByID(id);
            return mapper.Map<ServiceCategoryDTO>(serviceCategory);
        }
        public void Delete(ServiceCategoryDTO serviceCategoryDTO)
        {
            var serviceCategoryToDelete = repositories.ServiceCategoryRepository.GetByID(serviceCategoryDTO.Id);
            repositories.ServiceCategoryRepository.Delete(serviceCategoryToDelete);
            repositories.ServiceCategoryRepository.Save();
        }

        public void Update(ServiceCategoryDTO serviceCategoryDTO)
        {
            var serviceCategoryToEdit = repositories.ServiceCategoryRepository.GetByID(serviceCategoryDTO.Id);
            serviceCategoryDTO.Id = serviceCategoryToEdit.ServiceCategoryId;
            serviceCategoryToEdit = mapper.Map<ServiceCategory>(serviceCategoryDTO);
            repositories.ServiceCategoryRepository.CreateOrUpdate(serviceCategoryToEdit);
            repositories.ServiceCategoryRepository.Save();
        }

        public ServiceCategoryDTO Create(ServiceCategoryDTO serviceCategoryDTO)
        {
            var serviceCategory = mapper.Map<ServiceCategory>(serviceCategoryDTO);
            repositories.ServiceCategoryRepository.CreateOrUpdate(serviceCategory);
            repositories.ServiceCategoryRepository.Save();
            serviceCategoryDTO.Id = serviceCategory.ServiceCategoryId;
            return serviceCategoryDTO;
        }

    }

}
