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
    public class PaymentService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public PaymentService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<PaymentType, PaymentTypeDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(paymentType => paymentType.PaymentTypeId)).
                                ForMember(x => x.Type, x => x.MapFrom(paymentType => paymentType.PaymentType1)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<PaymentTypeDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(PaymentTypeDTO paymentTypeDTO) => await Task.Run(() => Delete(paymentTypeDTO));
        public async Task UpdateAsync(PaymentTypeDTO paymentTypeDTO) => await Task.Run(() => Update(paymentTypeDTO));
        public async Task<PaymentTypeDTO> CreateAsync(PaymentTypeDTO paymentTypeDTO) => await Task.Run(() => Create(paymentTypeDTO));
        public async Task<PaymentTypeDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<PaymentTypeDTO> GetAll()
        {
            return mapper.Map<IEnumerable<PaymentTypeDTO>>(repositories.PaymentTypeRepository.GetAll());
        }
        public PaymentTypeDTO GetByID(int id)
        {
            var paymentType = repositories.PaymentTypeRepository.GetByID(id);
            return mapper.Map<PaymentTypeDTO>(paymentType);
        }
        public void Delete(PaymentTypeDTO paymentTypeDTO)
        {
            var paymentTypeToDelete = repositories.PaymentTypeRepository.GetByID(paymentTypeDTO.Id);
            repositories.PaymentTypeRepository.Delete(paymentTypeToDelete);
            repositories.PaymentTypeRepository.Save();
        }

        public void Update(PaymentTypeDTO paymentTypeDTO)
        {
            var paymentTypeToEdit = repositories.PaymentTypeRepository.GetByID(paymentTypeDTO.Id);
            paymentTypeDTO.Id = paymentTypeToEdit.PaymentTypeId;
            paymentTypeToEdit = mapper.Map<PaymentType>(paymentTypeDTO);
            repositories.PaymentTypeRepository.CreateOrUpdate(paymentTypeToEdit);
            repositories.PaymentTypeRepository.Save();
        }

        public PaymentTypeDTO Create(PaymentTypeDTO paymentTypeDTO)
        {
            var paymentType = mapper.Map<PaymentType>(paymentTypeDTO);
            repositories.PaymentTypeRepository.CreateOrUpdate(paymentType);
            repositories.PaymentTypeRepository.Save();
            paymentTypeDTO.Id = paymentType.PaymentTypeId;
            return paymentTypeDTO;
        }

    }

}
