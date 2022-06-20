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
    public class VisitService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public VisitService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Visit, VisitDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(visit => visit.VisitId)).
                                ForMember(x => x.Date, x => x.MapFrom(visit => visit.VisitDate)).
                                ForMember(x => x.MastedId, x => x.MapFrom(visit => visit.MasterId)).
                                ForMember(x => x.MasterLastname, x => x.MapFrom(visit => visit.Master.MasterLastName)).
                                ForMember(x => x.MasterName, x => x.MapFrom(visit => visit.Master.MasterName)).
                                ForMember(x => x.ClientId, x => x.MapFrom(visit => visit.ClientId)).
                                ForMember(x => x.ClientLastname, x => x.MapFrom(visit => visit.Client.ClientLastName)).
                                ForMember(x => x.ClientName, x => x.MapFrom(visit => visit.Client.ClientName)).
                                ForMember(x => x.ServiceId, x => x.MapFrom(visit => visit.ServiceId)).
                                ForMember(x => x.Service, x => x.MapFrom(visit => visit.Service.ServiceName)).
                                ForMember(x => x.PaymentTypeId, x => x.MapFrom(visit => visit.PaymentTypeId)).
                                ForMember(x => x.PaymentType, x => x.MapFrom(visit => visit.PaymentType.PaymentType1)).
                                ForMember(x => x.ServicePrice, x => x.MapFrom(visit => visit.ServicePrice)).
                                ForMember(x => x.Amount, x => x.MapFrom(visit => visit.Amount)).
                                ForMember(x => x.AdditionalCost, x => x.MapFrom(visit => visit.AdditionalCost)).
                                ForMember(x => x.MaterialPrice, x => x.MapFrom(visit => visit.MaterialPrice)).
                                ForMember(x => x.Profit, x => x.MapFrom(visit => visit.Profit)).
                                ForMember(x => x.Salary, x => x.MapFrom(visit => visit.Salary)).
                                ForMember(x => x.Tips, x => x.MapFrom(visit => visit.Tips));
                cfg.CreateMap<VisitDTO, Visit>().
                                ForMember(x => x.VisitId, x => x.MapFrom(visit => visit.Id)).
                                ForMember(x => x.VisitDate, x => x.MapFrom(visit => visit.Date)).
                                ForMember(x => x.MasterId, x => x.MapFrom(visit => visit.MastedId)).
                                ForMember(x => x.ClientId, x => x.MapFrom(visit => visit.ClientId)).
                                ForMember(x => x.ServiceId, x => x.MapFrom(visit => visit.ServiceId)).
                                ForMember(x => x.PaymentTypeId, x => x.MapFrom(visit => visit.PaymentTypeId)).
                                ForMember(x => x.ServicePrice, x => x.MapFrom(visit => visit.ServicePrice)).
                                ForMember(x => x.MaterialPrice, x => x.MapFrom(visit => visit.MaterialPrice)).
                                ForMember(x => x.AdditionalCost, x => x.MapFrom(visit => visit.AdditionalCost)).
                                ForMember(x => x.Amount, x => x.MapFrom(visit => visit.Amount)).
                                ForMember(x => x.Profit, x => x.MapFrom(visit => visit.Profit)).
                                ForMember(x => x.Salary, x => x.MapFrom(visit => visit.Salary)).
                                ForMember(x => x.Tips, x => x.MapFrom(visit => visit.Tips)).ForAllOtherMembers(x => x.Ignore());
            });
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<VisitDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(VisitDTO visitDTO) => await Task.Run(() => Delete(visitDTO));
        public async Task UpdateAsync(VisitDTO visitDTO) => await Task.Run(() => Update(visitDTO));
        public async Task<VisitDTO> CreateAsync(VisitDTO visitDTO) => await Task.Run(() => Create(visitDTO));
        public async Task<VisitDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));

        public IEnumerable<VisitDTO> GetAll()
        {
            return mapper.Map<IEnumerable<VisitDTO>>(repositories.VisitRepository.GetAll());
        }
        public VisitDTO GetByID(int id)
        {
            var visit = repositories.VisitRepository.GetByID(id);
            return mapper.Map<VisitDTO>(visit);
        }
        public void Delete(VisitDTO visitDTO)
        {
            var visitToDelete = repositories.VisitRepository.GetByID(visitDTO.Id);
            repositories.VisitRepository.Delete(visitToDelete);
            repositories.VisitRepository.Save();
        }
        public void Update(VisitDTO visitDTO)
        {
            var visitToEdit = repositories.VisitRepository.GetByID(visitDTO.Id);
            visitDTO.Id = visitToEdit.VisitId;
            visitToEdit = mapper.Map<Visit>(visitDTO);
            repositories.VisitRepository.CreateOrUpdate(visitToEdit);
            repositories.VisitRepository.Save();
        }

        public VisitDTO Create(VisitDTO visitDTO)
        {
            var visit = mapper.Map<Visit>(visitDTO);
            repositories.VisitRepository.CreateOrUpdate(visit);
            repositories.VisitRepository.Save();
            visitDTO.Id = visit.VisitId;
            return visitDTO;
        }

    }

}
