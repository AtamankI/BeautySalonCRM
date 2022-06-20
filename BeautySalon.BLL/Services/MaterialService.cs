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
    public class MaterialService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public MaterialService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Material, MaterialDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(material => material.MaterialId)).
                                ForMember(x => x.Name, x => x.MapFrom(material => material.MaterialName)).
                                ForMember(x => x.Number, x => x.MapFrom(material => material.Number)).
                                ForMember(x => x.Volume, x => x.MapFrom(material => material.Volume)).
                                ForMember(x => x.TotalAmount, x => x.MapFrom(material => material.Total)).
                                ForMember(x => x.GramAmount, x => x.MapFrom(material => material.GramAmount)).
                                ForMember(x => x.Price, x => x.MapFrom(material => material.Price)).
                                ForMember(x => x.PriceGram, x => x.MapFrom(material => material.PriceGram)).
                                ForMember(x => x.ManufacturerId, x => x.MapFrom(material => material.ManufacturerId)).
                                ForMember(x => x.ManufacturerName, x => x.MapFrom(material => material.MaterialManufacturer.MaterialManufacturerName)).
                                ForMember(x => x.CategoryId, x => x.MapFrom(material => material.CategoryId)).
                                ForMember(x => x.CategoryName, x => x.MapFrom(material => material.MaterialCategory.MaterialCategoryName));
                cfg.CreateMap<MaterialDTO, Material>().
                                ForMember(x => x.MaterialId, x => x.MapFrom(material => material.Id)).
                                ForMember(x => x.MaterialName, x => x.MapFrom(material => material.Name)).
                                ForMember(x => x.Number, x => x.MapFrom(material => material.Number)).
                                ForMember(x => x.Volume, x => x.MapFrom(material => material.Volume)).
                                ForMember(x => x.Total, x => x.MapFrom(material => material.TotalAmount)).
                                ForMember(x => x.GramAmount, x => x.MapFrom(material => material.GramAmount)).
                                ForMember(x => x.Price, x => x.MapFrom(material => material.Price)).
                                ForMember(x => x.PriceGram, x => x.MapFrom(material => material.PriceGram)).
                                ForMember(x => x.ManufacturerId, x => x.MapFrom(material => material.ManufacturerId)).
                                ForMember(x => x.CategoryId, x => x.MapFrom(material => material.CategoryId)).ForAllOtherMembers(x => x.Ignore());
            });
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<MaterialDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(MaterialDTO materialDTO) => await Task.Run(() => Delete(materialDTO));
        public async Task UpdateAsync(MaterialDTO materialDTO) => await Task.Run(() => Update(materialDTO));
        public async Task<MaterialDTO> CreateAsync(MaterialDTO materialDTO) => await Task.Run(() => Create(materialDTO));
        public async Task<MaterialDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<MaterialDTO> GetAll()
        {
            return mapper.Map<IEnumerable<MaterialDTO>>(repositories.MaterialRepository.GetAll());
        }
        public MaterialDTO GetByID(int id)
        {
            var material = repositories.MaterialRepository.GetByID(id);
            return mapper.Map<MaterialDTO>(material);
        }
        public void Delete(MaterialDTO materialDTO)
        {
            var materialToDelete = repositories.MaterialRepository.GetByID(materialDTO.Id);
            repositories.MaterialRepository.Delete(materialToDelete);
            repositories.MaterialRepository.Save();
        }

        public void Update(MaterialDTO materialDTO)
        {
            var materialToEdit = repositories.MaterialRepository.GetByID(materialDTO.Id);
            materialDTO.Id = materialToEdit.MaterialId;
            materialToEdit = mapper.Map<Material>(materialDTO);
            repositories.MaterialRepository.CreateOrUpdate(materialToEdit);
            repositories.MaterialRepository.Save();
        }

        public MaterialDTO Create(MaterialDTO materialDTO)
        {
            var material = mapper.Map<Material>(materialDTO);
            repositories.MaterialRepository.CreateOrUpdate(material);
            repositories.MaterialRepository.Save();
            materialDTO.Id = material.MaterialId;
            return materialDTO;
        }

    }

}
