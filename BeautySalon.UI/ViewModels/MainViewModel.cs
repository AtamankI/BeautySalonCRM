using BeautySalon.BLL.DTO;
using BeautySalon.BLL.Services;
using BeautySalon.UI.Infrastructure;
using BeautySalon.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BeautySalon.UI.ViewModels
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private string mainViewVisibility = "visible";
        private string userControlVisibility = "hidden";
        private string loadingTextBlockVisibility = "hidden";
        private string login;
        private string password; 
        public string ImageUrl { get => "https://content3.jdmagicbox.com/comp/delhi/z8/011pxx11.xx11.190601172113.f9z8/catalogue/welcome-beauty-salon-the-unisex-salon-dwarka-sector-7-delhi-beauty-parlours-c0roz6enqg.jpg?clr=333333"; }
        private UserDTO user;
        private GeneralService generalService;
        public string LoadingTextBlockVisibility
        {
            get => loadingTextBlockVisibility;
            set
            {
                loadingTextBlockVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        private UserControl currentView;
        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                NotifyOfPropertyChanged();
            }
        }
        public UserDTO User
        {
            get => user;
            set
            {
                user = value;
                NotifyOfPropertyChanged();
            }
        }
        public string Login
        {
            get => login;
            set
            {
                login = value;
                NotifyOfPropertyChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                NotifyOfPropertyChanged();
            }
        }

        public string MainViewVisibility
        {
            get => mainViewVisibility;
            set
            {
                mainViewVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        public string UserControlVisibility
        {
            get => userControlVisibility;
            set
            {
                userControlVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        #endregion
        public MainViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            User = new UserDTO();
            InitCommands();
        }
        private void InitCommands()
        {
            EnterSystemCommand = new RelayCommand(async param => 
            {
                User = new UserDTO();
                if (Login == null || Password == null)
                {
                    InformationMessage("Login and Password must be filled");
                    return;
                }
                LoadingTextBlockVisibility = "Visible";
                MainViewVisibility = "Hidden";
                await CheckUser();
                if (User?.Login == null)
                {
                    MainViewVisibility = "Visible";
                    LoadingTextBlockVisibility = "Hidden";
                    InformationMessage("Wrong Login or Password");
                    return;
                }
                CurrentView = new MenuView();
                LoadingTextBlockVisibility = "Hidden";
                UserControlVisibility = "Visible";
            });
            SendRenovateEmailCommand = new RelayCommand(async param =>
            {
                if (Login == null)
                {
                    InformationMessage("Login must be filled");
                    return;
                }
                var users = await generalService.UserService.GetAllAsync();
                User = users.FirstOrDefault(x => x.Login == Login);
                if (User?.Login == null)
                {
                    InformationMessage("Wrong Login");
                    return;
                }
                SendMessage(User?.Password);
            });
        }

        private void SendMessage(string password)
        {  
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("iraatamvlad@gmail.com");
                message.To.Add(new MailAddress(User?.Email));
                message.Subject = "Renovate password";
                message.IsBodyHtml = true;
                message.Body = password;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("iraatamvlad@gmail.com", "");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                InformationMessage("Message was sent");
            }
            catch (Exception) { }
        }

        private async Task CheckUser()
        {
            var encryptedPass = EncryptPassword();
            var users = await generalService.UserService.GetAllAsync();
            User = users.FirstOrDefault(x => x.Login == Login && x.Password == encryptedPass);
        }
        private string EncryptPassword()
        {
            if (Password == null)
                return null;
            MD5 md5 = MD5.Create();

            byte[] inputPasswordBytes = Encoding.ASCII.GetBytes(Password);
            byte[] hash = md5.ComputeHash(inputPasswordBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var h in hash)
            {
                sb.Append(h.ToString("X2"));
            }
            return sb.ToString();
        }
        private void InformationMessage(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }
        public ICommand EnterSystemCommand { get; private set; }
        public ICommand SendRenovateEmailCommand { get; private set; }
    }
}
