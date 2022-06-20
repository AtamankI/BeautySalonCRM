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
    public class SalaryService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public SalaryService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<SalaryType, SalaryTypeDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(salary => salary.SalaryTypeId)).
                                ForMember(x => x.Percent, x => x.MapFrom(salary => salary.SalaryPercent)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<SalaryTypeDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(SalaryTypeDTO salaryTypeDTO) => await Task.Run(() => Delete(salaryTypeDTO));
        public async Task UpdateAsync(SalaryTypeDTO salaryTypeDTO) => await Task.Run(() => Update(salaryTypeDTO));
        public async Task<SalaryTypeDTO> CreateAsync(SalaryTypeDTO salaryTypeDTO) => await Task.Run(() => Create(salaryTypeDTO));
        public async Task<SalaryTypeDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<SalaryTypeDTO> GetAll()
        {
            return mapper.Map<IEnumerable<SalaryTypeDTO>>(repositories.SalaryTypeRepository.GetAll());
        }
        public SalaryTypeDTO GetByID(int id)
        {
            var salaryType = repositories.SalaryTypeRepository.GetByID(id);
            return mapper.Map<SalaryTypeDTO>(salaryType);
        }
        public void Delete(SalaryTypeDTO salaryTypeDTO)
        {
            var salaryTypeToDelete = repositories.SalaryTypeRepository.GetByID(salaryTypeDTO.Id);
            repositories.SalaryTypeRepository.Delete(salaryTypeToDelete);
            repositories.SalaryTypeRepository.Save();
        }

        public void Update(SalaryTypeDTO salaryTypeDTO)
        {
            var salaryTypeToEdit = repositories.SalaryTypeRepository.GetByID(salaryTypeDTO.Id);
            salaryTypeDTO.Id = salaryTypeToEdit.SalaryTypeId;
            salaryTypeToEdit = mapper.Map<SalaryType>(salaryTypeDTO);
            repositories.SalaryTypeRepository.CreateOrUpdate(salaryTypeToEdit);
            repositories.SalaryTypeRepository.Save();
        }

        public SalaryTypeDTO Create(SalaryTypeDTO salaryTypeDTO)
        {
            var salaryType = mapper.Map<SalaryType>(salaryTypeDTO);
            repositories.SalaryTypeRepository.CreateOrUpdate(salaryType);
            repositories.SalaryTypeRepository.Save();
            salaryTypeDTO.Id = salaryType.SalaryTypeId;
            return salaryTypeDTO;
        }

    }

}
