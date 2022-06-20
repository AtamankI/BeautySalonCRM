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
    public class MasterCategoryService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public MasterCategoryService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<MasterCategory, MasterCategoryDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(masterCategory => masterCategory.MasterCategoryId)).
                                ForMember(x => x.Name, x => x.MapFrom(client => client.MasterCategoryName)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<MasterCategoryDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(MasterCategoryDTO masterCategoryDTO) => await Task.Run(() => Delete(masterCategoryDTO));
        public async Task UpdateAsync(MasterCategoryDTO masterCategoryDTO) => await Task.Run(() => Update(masterCategoryDTO));
        public async Task<MasterCategoryDTO> CreateAsync(MasterCategoryDTO masterCategoryDTO) => await Task.Run(() => Create(masterCategoryDTO));
        public async Task<MasterCategoryDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<MasterCategoryDTO> GetAll()
        {
            return mapper.Map<IEnumerable<MasterCategoryDTO>>(repositories.MasterCategoryRepository.GetAll());
        }
        public MasterCategoryDTO GetByID(int id)
        {
            var masterCategory = repositories.MasterCategoryRepository.GetByID(id);
            return mapper.Map<MasterCategoryDTO>(masterCategory);
        }
        public void Delete(MasterCategoryDTO masterCategoryDTO)
        {
            var masterCategoryToDelete = repositories.MasterCategoryRepository.GetByID(masterCategoryDTO.Id);
            repositories.MasterCategoryRepository.Delete(masterCategoryToDelete);
            repositories.MasterCategoryRepository.Save();
        }

        public void Update(MasterCategoryDTO masterCategoryDTO)
        {
            var masterCategoryToEdit = repositories.MasterCategoryRepository.GetByID(masterCategoryDTO.Id);
            masterCategoryDTO.Id = masterCategoryToEdit.MasterCategoryId;
            masterCategoryToEdit = mapper.Map<MasterCategory>(masterCategoryDTO);
            repositories.MasterCategoryRepository.CreateOrUpdate(masterCategoryToEdit);
            repositories.MasterCategoryRepository.Save();
        }

        public MasterCategoryDTO Create(MasterCategoryDTO masterCategoryDTO)
        {
            var masterCategory = mapper.Map<MasterCategory>(masterCategoryDTO);
            repositories.MasterCategoryRepository.CreateOrUpdate(masterCategory);
            repositories.MasterCategoryRepository.Save();
            masterCategoryDTO.Id = masterCategory.MasterCategoryId;
            return masterCategoryDTO;
        }

    }

}
