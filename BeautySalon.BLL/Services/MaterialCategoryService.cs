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
    public class MaterialCategoryService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public MaterialCategoryService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<MaterialCategory, MaterialCategoryDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(materialCategory => materialCategory.MaterialCategoryId)).
                                ForMember(x => x.Name, x => x.MapFrom(materialCategory => materialCategory.MaterialCategoryName)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<MaterialCategoryDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(MaterialCategoryDTO materialCategoryDTO) => await Task.Run(() => Delete(materialCategoryDTO));
        public async Task UpdateAsync(MaterialCategoryDTO materialCategoryDTO) => await Task.Run(() => Update(materialCategoryDTO));
        public async Task<MaterialCategoryDTO> CreateAsync(MaterialCategoryDTO materialCategoryDTO) => await Task.Run(() => Create(materialCategoryDTO));
        public async Task<MaterialCategoryDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<MaterialCategoryDTO> GetAll()
        {
            return mapper.Map<IEnumerable<MaterialCategoryDTO>>(repositories.MaterialCategoryRepository.GetAll());
        }
        public MaterialCategoryDTO GetByID(int id)
        {
            var materialCategory = repositories.MaterialCategoryRepository.GetByID(id);
            return mapper.Map<MaterialCategoryDTO>(materialCategory);
        }
        public void Delete(MaterialCategoryDTO materialCategoryDTO)
        {
            var materialCategoryToDelete = repositories.MaterialCategoryRepository.GetByID(materialCategoryDTO.Id);
            repositories.MaterialCategoryRepository.Delete(materialCategoryToDelete);
            repositories.MaterialCategoryRepository.Save();
        }

        public void Update(MaterialCategoryDTO materialCategoryDTO)
        {
            var materialCategoryToEdit = repositories.MaterialCategoryRepository.GetByID(materialCategoryDTO.Id);
            materialCategoryDTO.Id = materialCategoryToEdit.MaterialCategoryId;
            materialCategoryToEdit = mapper.Map<MaterialCategory>(materialCategoryDTO);
            repositories.MaterialCategoryRepository.CreateOrUpdate(materialCategoryToEdit);
            repositories.MaterialCategoryRepository.Save();
        }

        public MaterialCategoryDTO Create(MaterialCategoryDTO materialCategoryDTO)
        {
            var materialCategory = mapper.Map<MaterialCategory>(materialCategoryDTO);
            repositories.MaterialCategoryRepository.CreateOrUpdate(materialCategory);
            repositories.MaterialCategoryRepository.Save();
            materialCategoryDTO.Id = materialCategory.MaterialCategoryId;
            return materialCategoryDTO;
        }

    }

}
