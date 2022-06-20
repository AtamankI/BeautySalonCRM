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
    public class MasterService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public MasterService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Master, MasterDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(master => master.MasterId)).
                                ForMember(x => x.Lastname, x => x.MapFrom(master => master.MasterLastName)).
                                ForMember(x => x.Name, x => x.MapFrom(master => master.MasterName)).
                                ForMember(x => x.Phone, x => x.MapFrom(master => master.Phone)).
                                ForMember(x => x.HireDate, x => x.MapFrom(master => master.HireDate)).
                                ForMember(x => x.RetireDate, x => x.MapFrom(master => master.RetireDate)).
                                ForMember(x => x.Category, x => x.MapFrom(master => master.MasterCategory.MasterCategoryName)).
                                ForMember(x => x.SalaryPercent, x => x.MapFrom(master => master.SalaryType.SalaryPercent));
                cfg.CreateMap<MasterDTO, Master>().
                                ForMember(x => x.MasterId, x => x.MapFrom(master => master.Id)).
                                ForMember(x => x.MasterLastName, x => x.MapFrom(master => master.Lastname)).
                                ForMember(x => x.MasterName, x => x.MapFrom(master => master.Name)).
                                ForMember(x => x.Phone, x => x.MapFrom(master => master.Phone)).
                                ForMember(x => x.HireDate, x => x.MapFrom(master => master.HireDate)).
                                ForMember(x => x.RetireDate, x => x.MapFrom(master => master.RetireDate)).
                                ForMember(x => x.MasterCategoryId, x => x.MapFrom(master => master.CategoryId)).
                                ForMember(x => x.SalaryTypeId, x => x.MapFrom(master => master.SalaryTypeId));
            });
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<MasterDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(MasterDTO masterDTO) => await Task.Run(() => Delete(masterDTO));
        public async Task UpdateAsync(MasterDTO masterDTO) => await Task.Run(() => Update(masterDTO));
        public async Task<MasterDTO> CreateAsync(MasterDTO masterDTO) => await Task.Run(() => Create(masterDTO));
        public async Task<MasterDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<MasterDTO> GetAll()
        {
            return mapper.Map<IEnumerable<MasterDTO>>(repositories.MasterRepository.GetAll());
        }
        public MasterDTO GetByID(int id)
        {
            var master = repositories.MasterRepository.GetByID(id);
            return mapper.Map<MasterDTO>(master);
        }
        public void Delete(MasterDTO masterDTO)
        {
            var masterToDelete = repositories.MasterRepository.GetByID(masterDTO.Id);
            repositories.MasterRepository.Delete(masterToDelete);
            repositories.MasterRepository.Save();
        }

        public void Update(MasterDTO masterDTO)
        {
            var masterToEdit = repositories.MasterRepository.GetByID(masterDTO.Id);
            masterDTO.Id = masterToEdit.MasterId;
            masterToEdit = mapper.Map<Master>(masterDTO);
            repositories.MasterRepository.CreateOrUpdate(masterToEdit);
            repositories.MasterRepository.Save();
        }

        public MasterDTO Create(MasterDTO masterDTO)
        {
            var master = mapper.Map<Master>(masterDTO);
            repositories.MasterRepository.CreateOrUpdate(master);
            repositories.MasterRepository.Save();
            masterDTO.Id = master.MasterId;
            return masterDTO;
        }

    }

}
