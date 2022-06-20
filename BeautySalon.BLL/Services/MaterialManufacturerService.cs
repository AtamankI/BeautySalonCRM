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
    public class MaterialManufacturerService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public MaterialManufacturerService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<MaterialManufacturer, MaterialManufacturerDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(materialManufacturer => materialManufacturer.MaterialManufacturerId)).
                                ForMember(x => x.Name, x => x.MapFrom(materialManufacturer => materialManufacturer.MaterialManufacturerName)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<MaterialManufacturerDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(MaterialManufacturerDTO materialManufacturerDTO) => await Task.Run(() => Delete(materialManufacturerDTO));
        public async Task UpdateAsync(MaterialManufacturerDTO materialManufacturerDTO) => await Task.Run(() => Update(materialManufacturerDTO));
        public async Task<MaterialManufacturerDTO> CreateAsync(MaterialManufacturerDTO materialManufacturerDTO) => await Task.Run(() => Create(materialManufacturerDTO));
        public async Task<MaterialManufacturerDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<MaterialManufacturerDTO> GetAll()
        {
            return mapper.Map<IEnumerable<MaterialManufacturerDTO>>(repositories.MaterialManufacturerRepository.GetAll());
        }
        public MaterialManufacturerDTO GetByID(int id)
        {
            var materialManufacturer = repositories.MaterialManufacturerRepository.GetByID(id);
            return mapper.Map<MaterialManufacturerDTO>(materialManufacturer);
        }
        public void Delete(MaterialManufacturerDTO materialManufacturerDTO)
        {
            var materialManufacturerToDelete = repositories.MaterialManufacturerRepository.GetByID(materialManufacturerDTO.Id);
            repositories.MaterialManufacturerRepository.Delete(materialManufacturerToDelete);
            repositories.MaterialManufacturerRepository.Save();
        }

        public void Update(MaterialManufacturerDTO materialManufacturerDTO)
        {
            var materialManufacturerToEdit = repositories.MaterialManufacturerRepository.GetByID(materialManufacturerDTO.Id);
            materialManufacturerDTO.Id = materialManufacturerToEdit.MaterialManufacturerId;
            materialManufacturerToEdit = mapper.Map<MaterialManufacturer>(materialManufacturerDTO);
            repositories.MaterialManufacturerRepository.CreateOrUpdate(materialManufacturerToEdit);
            repositories.MaterialManufacturerRepository.Save();
        }

        public MaterialManufacturerDTO Create(MaterialManufacturerDTO materialManufacturerDTO)
        {
            var materialManufacturer = mapper.Map<MaterialManufacturer>(materialManufacturerDTO);
            repositories.MaterialManufacturerRepository.CreateOrUpdate(materialManufacturer);
            repositories.MaterialManufacturerRepository.Save();
            materialManufacturerDTO.Id = materialManufacturer.MaterialManufacturerId;
            return materialManufacturerDTO;
        }

    }
}
