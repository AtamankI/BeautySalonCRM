using BeautySalon.BLL.DTO;
using BeautySalon.BLL.Services;
using BeautySalon.UI.Extensions;
using BeautySalon.UI.Infrastructure;
using BeautySalon.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BeautySalon.UI.ViewModels
{
    public class MenuViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private GeneralService generalService;
        private string selectedMenuControlVisibility = "hidden";
        private string menubuttonsVisibility = "visible";
        private UserControl currentMenuView;
        private ObservableCollection<BookingDTO> Bookings;
        public UserControl CurrentMenuView
        {
            get => currentMenuView;
            set
            {
                currentMenuView = value;
                NotifyOfPropertyChanged();
            }
        }
        public string MenubuttonsVisibility
        {
            get => menubuttonsVisibility;
            set
            {
                menubuttonsVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        public string SelectedMenuControlVisibility
        {
            get => selectedMenuControlVisibility;
            set
            {
                selectedMenuControlVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        #endregion

        public MenuViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();

        }
        private void InitCommands()
        {
            BookingCommand = new RelayCommand(param =>
            {
                MenubuttonsVisibility = "Hidden";
                CurrentMenuView = new BookingView();
                SelectedMenuControlVisibility = "Visible";
            });
            VisitsCommand = new RelayCommand(param =>
            {
                MenubuttonsVisibility = "Hidden";
                CurrentMenuView = new VisitsView();
                SelectedMenuControlVisibility = "Visible";
            });
            ServicesCommand = new RelayCommand(param =>
            {
                MenubuttonsVisibility = "Hidden";
                CurrentMenuView = new ServicesView();
                SelectedMenuControlVisibility = "Visible";
            });
            ArchiveCommand = new RelayCommand(param =>
            {
                MenubuttonsVisibility = "Hidden";
                CurrentMenuView = new ArchiveView();
                SelectedMenuControlVisibility = "Visible";
            });

            OptionsCommand = new RelayCommand(param =>
            {
                MenubuttonsVisibility = "Hidden";
                CurrentMenuView = new OptionsView();
                SelectedMenuControlVisibility = "Visible";
            });
            ExitCommand = new RelayCommand(param => Environment.Exit(0));
            EndDayCommand = new RelayCommand(async param =>
            {
                var bookings = await generalService.BookingService.GetAllAsync();
                Bookings = new ObservableCollection<BookingDTO>(bookings);
                try
                {
                    DateTime nextDate = DateTime.Now.Date;
                    nextDate = nextDate.AddDays(1);
                    Bookings = Bookings.Where(x => x.Date == nextDate)?.ToObservableCollection();
                    var clients = await generalService.ClientService.GetAllAsync();
                    foreach (var item in Bookings)
                    {
                        var clientEmail = clients.FirstOrDefault(x => x.Id == item.ClientId).Email;
                        if (clientEmail != null)
                        {
                            SendMessage(clientEmail, item.Date.ToString(), item.Time);
                        }
                    }
                    InformationMessage("Message was sent");
                }
                catch
                { }
            });
        }
        private void InformationMessage(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }
        private void SendMessage(string email, string booking, string time)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("iraatamvlad@gmail.com");
                message.To.Add(new MailAddress(email));
                message.Subject = "BeautifulSalon";
                message.IsBodyHtml = true;
                message.Body = $"You have booking on {booking} at {time}";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("iraatamvlad@gmail.com", "");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }

        public ICommand BookingCommand { get; private set; }
        public ICommand VisitsCommand { get; private set; }
        public ICommand ServicesCommand { get; private set; }
        public ICommand ArchiveCommand { get; private set; }
        public ICommand OptionsCommand { get; private set; }
        public ICommand EndDayCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
    }
}
