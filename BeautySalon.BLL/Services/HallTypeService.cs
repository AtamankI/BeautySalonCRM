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
    public class HallTypeService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public HallTypeService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<HallType, HallTypeDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(hallType => hallType.HallTypeId)).
                                ForMember(x => x.HallType, x => x.MapFrom(hallType => hallType.HallType1)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<HallTypeDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(HallTypeDTO hallTypeDTO) => await Task.Run(() => Delete(hallTypeDTO));
        public async Task UpdateAsync(HallTypeDTO hallTypeDTO) => await Task.Run(() => Update(hallTypeDTO));
        public async Task<HallTypeDTO> CreateAsync(HallTypeDTO hallTypeDTO) => await Task.Run(() => Create(hallTypeDTO));
        public async Task<HallTypeDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<HallTypeDTO> GetAll()
        {
            return mapper.Map<IEnumerable<HallTypeDTO>>(repositories.HallTypeRepository.GetAll());
        }
        public HallTypeDTO GetByID(int id)
        {
            var hallType = repositories.HallTypeRepository.GetByID(id);
            return mapper.Map<HallTypeDTO>(hallType);
        }
        public void Delete(HallTypeDTO hallTypeDTO)
        {
            var hallTypeToDelete = repositories.HallTypeRepository.GetByID(hallTypeDTO.Id);
            repositories.HallTypeRepository.Delete(hallTypeToDelete);
            repositories.HallTypeRepository.Save();
        }

        public void Update(HallTypeDTO hallTypeDTO)
        {
            var hallTypeToEdit = repositories.HallTypeRepository.GetByID(hallTypeDTO.Id);
            hallTypeDTO.Id = hallTypeToEdit.HallTypeId;
            hallTypeToEdit = mapper.Map<HallType>(hallTypeDTO);
            repositories.HallTypeRepository.CreateOrUpdate(hallTypeToEdit);
            repositories.HallTypeRepository.Save();
        }

        public HallTypeDTO Create(HallTypeDTO hallTypeDTO)
        {
            var hallType = mapper.Map<HallType>(hallTypeDTO);
            repositories.HallTypeRepository.CreateOrUpdate(hallType);
            repositories.HallTypeRepository.Save();
            hallTypeDTO.Id = hallType.HallTypeId;
            return hallTypeDTO;
        }

    }

}
