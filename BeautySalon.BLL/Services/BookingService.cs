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
    public class BookingService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public BookingService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Booking, BookingDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(booking => booking.BookingId)).
                                ForMember(x => x.Date, x => x.MapFrom(booking => booking.BookingDate)).
                                ForMember(x => x.Time, x => x.MapFrom(booking => booking.BookingTime)).
                                ForMember(x => x.MasterLastname, x => x.MapFrom(booking => booking.Master.MasterLastName)).
                                ForMember(x => x.MasterName, x => x.MapFrom(booking => booking.Master.MasterName)).
                                ForMember(x => x.MasterId, x => x.MapFrom(booking => booking.MasterId)).
                                ForMember(x => x.ClientLastname, x => x.MapFrom(booking => booking.Client.ClientLastName)).
                                ForMember(x => x.ClientName, x => x.MapFrom(booking => booking.Client.ClientName)).
                                ForMember(x => x.ClientPhone, x => x.MapFrom(booking => booking.Client.Phone)).
                                ForMember(x => x.ClientId, x => x.MapFrom(booking => booking.ClientId)).
                                ForMember(x => x.Service, x => x.MapFrom(booking => booking.Service.ServiceName)).
                                ForMember(x => x.ServiceID, x => x.MapFrom(booking => booking.ServiceId)).
                                ForMember(x => x.Notes, x => x.MapFrom(booking => booking.Notes));
                cfg.CreateMap<BookingDTO, Booking>().
                                ForMember(x => x.BookingId, x => x.MapFrom(booking => booking.Id)).
                                ForMember(x => x.BookingDate, x => x.MapFrom(booking => booking.Date)).
                                ForMember(x => x.MasterId, x => x.MapFrom(booking => booking.MasterId)).
                                ForMember(x => x.ClientId, x => x.MapFrom(booking => booking.ClientId)).
                                ForMember(x => x.ServiceId, x => x.MapFrom(booking => booking.ServiceID)).
                                ForMember(x => x.BookingTime, x => x.MapFrom(booking => booking.Time)).
                                ForMember(x => x.Notes, x => x.MapFrom(booking => booking.Notes)).ForAllOtherMembers(x => x.Ignore());
            });
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<BookingDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(BookingDTO bookingDTO) => await Task.Run(() => Delete(bookingDTO));
        public async Task UpdateAsync(BookingDTO bookingDTO) => await Task.Run(() => Update(bookingDTO));
        public async Task<BookingDTO> CreateAsync(BookingDTO bookingDTO) => await Task.Run(() => Create(bookingDTO));
        public async Task<BookingDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<BookingDTO> GetAll()
        {
            return mapper.Map<IEnumerable<BookingDTO>>(repositories.BookingRepository.GetAll());
        }
        public BookingDTO GetByID(int id)
        {
            var booking = repositories.BookingRepository.GetByID(id);
            return mapper.Map<BookingDTO>(booking);
        }
        public void Delete(BookingDTO bookingDTO)
        {
            var bookingToDelete = repositories.BookingRepository.GetByID(bookingDTO.Id);
            repositories.BookingRepository.Delete(bookingToDelete);
            repositories.BookingRepository.Save();
        }

        public void Update(BookingDTO bookingDTO)
        {
            var bookingToEdit = repositories.BookingRepository.GetByID(bookingDTO.Id);
            bookingDTO.Id = bookingToEdit.BookingId;
            bookingToEdit = mapper.Map<Booking>(bookingDTO);
            repositories.BookingRepository.CreateOrUpdate(bookingToEdit);
            repositories.BookingRepository.Save();
        }

        public BookingDTO Create(BookingDTO bookingDTO)
        {
            var booking = mapper.Map<Booking>(bookingDTO);
            repositories.BookingRepository.CreateOrUpdate(booking);
            repositories.BookingRepository.Save();
            bookingDTO.Id = booking.BookingId;
            return bookingDTO;
        }

    }
}
