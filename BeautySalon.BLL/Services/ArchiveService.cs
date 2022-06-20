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
    public class ArchiveService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public ArchiveService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Archive, ArchiveDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(archive => archive.ArchiveId)).
                                ForMember(x => x.Date, x => x.MapFrom(archive => archive.ArchiveDate)).
                                ForMember(x => x.MasterLastname, x => x.MapFrom(archive => archive.Master.MasterLastName)).
                                ForMember(x => x.MasterName, x => x.MapFrom(archive => archive.Master.MasterName)).
                                ForMember(x => x.MasterId, x => x.MapFrom(archive => archive.MasterId)).
                                ForMember(x => x.TotalCustomers, x => x.MapFrom(archive => archive.TotalCustomers)).
                                ForMember(x => x.Salary, x => x.MapFrom(archive => archive.Salary)).
                                ForMember(x => x.Profit, x => x.MapFrom(archive => archive.Profit));
                cfg.CreateMap<ArchiveDTO, Archive>().
                                ForMember(x => x.ArchiveId, x => x.MapFrom(archive => archive.Id)).
                                ForMember(x => x.ArchiveDate, x => x.MapFrom(archive => archive.Date)).
                                ForMember(x => x.TotalCustomers, x => x.MapFrom(archive => archive.TotalCustomers)).
                                ForMember(x => x.Salary, x => x.MapFrom(archive => archive.Salary)).
                                ForMember(x => x.Profit, x => x.MapFrom(archive => archive.Profit)).
                                ForMember(x => x.MasterId, x => x.MapFrom(archive => archive.MasterId)).ForAllOtherMembers(x => x.Ignore());
            });
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<ArchiveDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(ArchiveDTO archiveDTO) => await Task.Run(() => Delete(archiveDTO));
        public async Task UpdateAsync(ArchiveDTO archiveDTO) => await Task.Run(() => Update(archiveDTO));
        public async Task<ArchiveDTO> CreateAsync(ArchiveDTO archiveDTO) => await Task.Run(() => Create(archiveDTO));
        public async Task<ArchiveDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<ArchiveDTO> GetAll()
        {
            return mapper.Map<IEnumerable<ArchiveDTO>>(repositories.ArchiveRepository.GetAll());
        }
        public ArchiveDTO GetByID(int id)
        {
            var archive = repositories.ArchiveRepository.GetByID(id);
            return mapper.Map<ArchiveDTO>(archive);
        }
        public void Delete(ArchiveDTO archiveDTO)
        {
            var archiveToDelete = repositories.ArchiveRepository.GetByID(archiveDTO.Id);
            repositories.ArchiveRepository.Delete(archiveToDelete);
            repositories.ArchiveRepository.Save();
        }

        public void Update(ArchiveDTO archiveDTO)
        {
            var archiveToEdit = repositories.ArchiveRepository.GetByID(archiveDTO.Id);
            archiveDTO.Id = archiveToEdit.ArchiveId;
            archiveToEdit = mapper.Map<Archive>(archiveDTO);
            repositories.ArchiveRepository.CreateOrUpdate(archiveToEdit);
            repositories.ArchiveRepository.Save();
        }

        public ArchiveDTO Create(ArchiveDTO archiveDTO)
        {
            var archive = mapper.Map<Archive>(archiveDTO);
            repositories.ArchiveRepository.CreateOrUpdate(archive);
            repositories.ArchiveRepository.Save();
            archiveDTO.Id = archive.ArchiveId;
            return archiveDTO;
        }
    }
}
