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
    public class ClientService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public ClientService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Client, ClientDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(client => client.ClientId)).
                                ForMember(x => x.Lastname, x => x.MapFrom(client => client.ClientLastName)).
                                ForMember(x => x.Name, x => x.MapFrom(client => client.ClientName)).
                                ForMember(x => x.Phone, x => x.MapFrom(client => client.Phone)).
                                ForMember(x => x.Email, x => x.MapFrom(client => client.Email)).
                                ForMember(x => x.NumberOfVisits, x => x.MapFrom(client => client.NumberOfVisits)).
                                ForMember(x => x.Notes, x => x.MapFrom(client => client.Notes)).
                                ForMember(x => x.LastVisit, x => x.MapFrom(client => client.LastVisit)).
                                ForMember(x => x.InArchive, x => x.MapFrom(client => client.InArchive)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<ClientDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(ClientDTO clientDTO) => await Task.Run(() => Delete(clientDTO));
        public async Task UpdateAsync(ClientDTO clientDTO) => await Task.Run(() => Update(clientDTO));
        public async Task<ClientDTO> CreateAsync(ClientDTO clientDTO) => await Task.Run(() => Create(clientDTO));
        public async Task<ClientDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<ClientDTO> GetAll()
        {
            return mapper.Map<IEnumerable<ClientDTO>>(repositories.ClientRepository.GetAll());
        }
        public ClientDTO GetByID(int id)
        {
            var client = repositories.ClientRepository.GetByID(id);
            return mapper.Map<ClientDTO>(client);
        }
        public void Delete(ClientDTO clientDTO)
        {
            var clientToDelete = repositories.ClientRepository.GetByID(clientDTO.Id);
            repositories.ClientRepository.Delete(clientToDelete);
            repositories.ClientRepository.Save();
        }

        public void Update(ClientDTO clientDTO)
        {
            var clientToEdit = repositories.ClientRepository.GetByID(clientDTO.Id);
            clientDTO.Id = clientToEdit.ClientId;
            clientToEdit = mapper.Map<Client>(clientDTO);
            repositories.ClientRepository.CreateOrUpdate(clientToEdit);
            repositories.ClientRepository.Save();
        }

        public ClientDTO Create(ClientDTO clientDTO)
        {
            var client = mapper.Map<Client>(clientDTO);
            repositories.ClientRepository.CreateOrUpdate(client);
            repositories.ClientRepository.Save();
            clientDTO.Id = client.ClientId;
            return clientDTO;
        }

    }
}
