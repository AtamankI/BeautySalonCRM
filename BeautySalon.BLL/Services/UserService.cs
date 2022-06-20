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
    public class UserService
    {
        private InitializationGeneralRepository repositories;
        private IMapper mapper;
        public UserService(InitializationGeneralRepository repositories)
        {
            this.repositories = repositories;
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserDTO>().
                                ForMember(x => x.Id, x => x.MapFrom(user => user.UserId)).
                                ForMember(x => x.Login, x => x.MapFrom(user => user.UserLogin)).
                                ForMember(x => x.Password, x => x.MapFrom(user => user.UserPassword)).
                                ForMember(x => x.Email, x => x.MapFrom(user => user.UserEmail)).
                                ReverseMap());
            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<UserDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public async Task DeleteAsync(UserDTO userDTO) => await Task.Run(() => Delete(userDTO));
        public async Task UpdateAsync(UserDTO userDTO) => await Task.Run(() => Update(userDTO));
        public async Task<UserDTO> CreateAsync(UserDTO userDTO) => await Task.Run(() => Create(userDTO));
        public async Task<UserDTO> GetByIDAsync(int id) => await Task.Run(() => GetByID(id));



        public IEnumerable<UserDTO> GetAll()
        {
            return mapper.Map<IEnumerable<UserDTO>>(repositories.UserRepository.GetAll());
        }
        public UserDTO GetByID(int id)
        {
            var user = repositories.UserRepository.GetByID(id);
            return mapper.Map<UserDTO>(user);
        }
        public void Delete(UserDTO userDTO)
        {
            var userToDelete = repositories.UserRepository.GetByID(userDTO.Id);
            repositories.UserRepository.Delete(userToDelete);
            repositories.UserRepository.Save();
        }

        public void Update(UserDTO userDTO)
        {
            var userToEdit = repositories.UserRepository.GetByID(userDTO.Id);
            userDTO.Id = userToEdit.UserId;
            userToEdit = mapper.Map<User>(userDTO);
            repositories.UserRepository.CreateOrUpdate(userToEdit);
            repositories.UserRepository.Save();
        }

        public UserDTO Create(UserDTO userDTO)
        {
            var user = mapper.Map<User>(userDTO);
            repositories.UserRepository.CreateOrUpdate(user);
            repositories.UserRepository.Save();
            userDTO.Id = user.UserId;
            return userDTO;
        }

    }
}
