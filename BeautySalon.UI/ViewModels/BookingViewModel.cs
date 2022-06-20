using BeautySalon.BLL.DTO;
using BeautySalon.BLL.Services;
using BeautySalon.UI.Extensions;
using BeautySalon.UI.Infrastructure;
using BeautySalon.UI.Views;
using BeautySalon.UI.Views.UpdateViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BeautySalon.UI.ViewModels
{
    public class BookingViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private GeneralService generalService;
        private string mainControlsVisible = "Visible";
        private string datagridVisibility = "hidden";
        private string loadingTextBlockVisibility = "hidden";
        private DateTime selectedDate;
        private string selectedTime;
        private BookingDTO selectedBooking;
        private ClientDTO selectedClient;
        private ServiceDTO selectedService;
        private MasterDTO selectedMaster;
        private ObservableCollection<BookingDTO> bookings;
        private ObservableCollection<ClientDTO> clients;
        private ObservableCollection<MasterDTO> masters;
        private ObservableCollection<ServiceDTO> services;
        private UserControl currentView;
        private UserControl addNewClientView;
        private string addNewClientVilibility = "Hidden";
        public DateTime SelectedDate{
            get => selectedDate;
            set
            {
                selectedDate = value;
                NotifyOfPropertyChanged();
            }
        }
        public string SelectedTime
        {
            get => selectedTime;
            set
            {
                selectedTime = value;
                NotifyOfPropertyChanged();
            }
        }
        public ServiceDTO SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                NotifyOfPropertyChanged();
            }
        }
        public MasterDTO SelectedMaster
        {
            get => selectedMaster;
            set
            {
                selectedMaster = value;
                NotifyOfPropertyChanged();
            }
        }
        public ClientDTO SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                NotifyOfPropertyChanged();
            }
        }
        public BookingDTO SelectedBooking
        {
            get => selectedBooking;
            set
            {
                selectedBooking = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<ClientDTO> Clients
        {
            get => clients;
            set
            {
                clients = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<MasterDTO> Masters
        {
            get => masters;
            set
            {
                masters = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<ServiceDTO> Services
        {
            get => services;
            set
            {
                services = value;
                NotifyOfPropertyChanged();
            }
        }

        public ObservableCollection<BookingDTO> Bookings
        {
            get => bookings;
            set
            {
                bookings = value;
                NotifyOfPropertyChanged();
            }
        }
        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                NotifyOfPropertyChanged();
            }
        }
        public UserControl AddNewClientView
        {
            get => addNewClientView;
            set
            {
                addNewClientView = value;
                NotifyOfPropertyChanged();
            }
        }
        public string LoadingTextBlockVisibility
        {
            get => loadingTextBlockVisibility;
            set
            {
                loadingTextBlockVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        public string DataGridVisibility
        {
            get => datagridVisibility;
            set
            {
                datagridVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        public string MainControlsVisible
        {
            get => mainControlsVisible;
            set
            {
                mainControlsVisible = value;
                NotifyOfPropertyChanged();
            }
        }

        public string AddNewClientVilibility 
        {
            get => addNewClientVilibility;
            set
            {
                addNewClientVilibility = value;
                NotifyOfPropertyChanged();
            }
        }
        #endregion
        public BookingViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
            LoadAdditionalDataAsync();
        }

        private void InitCommands()
        {
            GetAllBookingsCommand = new RelayCommand(async param =>
            {
               await LoadBokingsAsync();
            });
            GetByDateBookingsCommand = new RelayCommand(async param =>
            {
                await LoadBokingsAsync();
                try
                {
                    Bookings = Bookings.Where(x => x.Date == SelectedDate)?.ToObservableCollection();
                }
                catch
                { }
            });
            AddBookingCommand = new RelayCommand(async param =>
            {
                LoadingTextBlockVisibility = "Visible";
                SelectedBooking = new BookingDTO();
                SelectedBooking.Date = DateTime.Now;
                CurrentView = new UpdateBookingView();
                CurrentView.DataContext = this;
                await Task.Run(() => GetAdditionDataForBooking());
                ShowUserControlView();
                LoadingTextBlockVisibility = "Hidden";

            });
            UpdateBookingCommand = new RelayCommand(async param =>
            {
                if (SelectedBooking == null)
                {
                    InformationMessage("You have to select the item to edit!");
                    return;
                }
                CurrentView = new UpdateBookingView();
                CurrentView.DataContext = this;
                await Task.Run(() => GetAdditionDataForBooking());
                ShowUserControlView();

            });
            RemoveBookingCommand = new RelayCommand(async param =>
            {
                if (SelectedBooking == null)
                {
                    InformationMessage("You have to select the item to remove!");
                    return;
                }
                try
                {
                    await generalService.BookingService.DeleteAsync(SelectedBooking);
                    Bookings.Remove(SelectedBooking);
                }
                catch
                {
                    InformationMessage("This booking can't be removed. Server is unavaliable");
                }
            });
            FinishVisitCommand = new RelayCommand(param =>
            {
                if (SelectedBooking == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                FinishBookingModel.GetSelectedBooking(SelectedBooking);
                CurrentView = new FinishBookingView();
            });
            AddNewClientCommand = new RelayCommand(param =>
            {
                SelectedClient = null;
                SelectedClient = new ClientDTO();
                AddNewClientView = new UpdateClientView();
                AddNewClientView.DataContext = this;
                AddNewClientVilibility = "Visible";
            });
            SortBookingsCommand = new RelayCommand(param => SortBookings(param));
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                CurrentView = new MenuView();

            });
            SaveBookingCommand = new RelayCommand(param =>
            {
                SaveBooking();
                ResetSelectedItem();
                ShowMainView();
            });
            CancelBookingCommand = new RelayCommand(async param =>
            {
                ShowMainView();
                if (SelectedBooking != null && SelectedBooking?.Id != 0)
                    await LoadBokingsAsync();
                ResetSelectedItem();

            });
            SaveClientCommand = new RelayCommand(async param =>
            {
                if (SelectedClient.IsInArhive)
                {
                    SelectedClient.InArchive = 1;
                }
                else SelectedClient.InArchive = 0;
                var clientDto = await generalService.ClientService.CreateAsync(SelectedClient);
                Clients.Add(clientDto);
            });
            CancelCommand = new RelayCommand(param => SelectedClient = null);
        }

        #region BookingMethods
        private async void SaveBooking()
        {
            if (SelectedBooking.ClientLastname == null || SelectedMaster.Lastname == null || SelectedService.Name==null)
            {
                InformationMessage("Booking wasn't added. Field's must be filled!");
                return;
            }
            SelectedBooking.MasterId = SelectedMaster?.Id;
            SelectedBooking.MasterLastname = SelectedMaster?.Lastname;
            SelectedBooking.MasterName = SelectedMaster?.Name;
            SelectedBooking.ClientLastname = SelectedClient?.Lastname;
            SelectedBooking.ClientName = SelectedClient?.Name;
            SelectedBooking.ClientId = SelectedClient?.Id;
            SelectedBooking.ClientPhone = SelectedClient?.Phone;
            SelectedBooking.Service = SelectedService.Name;
            SelectedBooking.ServiceID = SelectedService.Id;
            SelectedBooking.Time = $"{SelectedDate.Hour}:{SelectedDate.Minute}";
            if (SelectedBooking?.Id != 0)
            {
                var tmpBooking = SelectedBooking;
                int index = Bookings.IndexOf(SelectedBooking);
                Bookings.Remove(SelectedBooking);
                await generalService.BookingService.UpdateAsync(tmpBooking);
                Bookings.Insert(index, tmpBooking);
                return;
            }
            var bookingDto = await generalService.BookingService.CreateAsync(SelectedBooking);
            if (Bookings == null)
            {
                InformationMessage("Booking was added. To see all bookings - press \"Get All\" button");
                ResetSelectedItem();
                return;
            }
            Bookings.Add(bookingDto);
            ResetSelectedItem();
        }
        private void SortBookings(object param)
        {
            switch (param)
            {
                case "DateAsc":
                    Bookings = Bookings.OrderBy(x => x.Date).ThenBy(x => x.Time).ToObservableCollection();
                    break;
                case "DateDesc":
                    Bookings = Bookings.OrderByDescending(x => x.Date).ThenBy(x => x.Time).ToObservableCollection();
                    break;
                case "MasterNameAsc":
                    Bookings = Bookings.OrderBy(x => x.MasterLastname).ThenBy(x => x.MasterName).ToObservableCollection();
                    break;
                case "MasterNameDesc":
                    Bookings = Bookings.OrderByDescending(x => x.MasterLastname).ThenBy(x => x.MasterName).ToObservableCollection();
                    break;
            }
        }
        private async Task LoadBokingsAsync()
        {
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Visible";
            var bookings = await generalService.BookingService.GetAllAsync();
            Bookings = new ObservableCollection<BookingDTO>(bookings);
            DataGridVisibility = "Visible";
            LoadingTextBlockVisibility = "Hidden";
        }
        private async void LoadAdditionalDataAsync()
        {
            SelectedDate = DateTime.Now;
            var clients = await generalService.ClientService.GetAllAsync();
            Clients = new ObservableCollection<ClientDTO>(clients);
            foreach (var client in Clients)
            {
                if (client.InArchive == 1)
                    client.IsInArhive = true;
            }
            Clients = Clients.Where(x => x.IsInArhive == false).ToObservableCollection();
            var services = await generalService.ServiceService.GetAllAsync();
            Services = new ObservableCollection<ServiceDTO>(services);
            foreach (var service in Services)
            {
                if (service.Avaliability == 0)
                    service.IsAvaliable = true;
            }
            Services = Services.Where(x => x.IsAvaliable).ToObservableCollection();
            var masters = await generalService.MasterService.GetAllAsync();
            Masters = new ObservableCollection<MasterDTO>(masters);
            Masters = Masters.Where(x => x.RetireDate == null).ToObservableCollection();
        }
        private void GetAdditionDataForBooking()
        {
            if (SelectedBooking?.ClientName != null)
            {
                SelectedClient = Clients.FirstOrDefault(x => x.Id == SelectedBooking.ClientId);
            }
            if (SelectedBooking?.MasterName != null)
            {
                SelectedMaster = Masters.FirstOrDefault(x => x.Id == SelectedBooking.MasterId);
            }
            if (SelectedBooking?.Service != null)
            {
                SelectedService = Services.FirstOrDefault(x => x.Id == SelectedBooking.ServiceID);
            }
            if (SelectedBooking?.Date != null)
            {
                SelectedDate = SelectedBooking.Date;
            }
            if (SelectedBooking?.Time != null)
            {
                SelectedTime = SelectedBooking.Time;
            }

        }
        #endregion

        #region OtherMethods
        private void ResetSelectedItem()
        {
            SelectedBooking = null;
            SelectedClient = null;
            SelectedMaster = null;
            SelectedService = null;
            SelectedTime = null;
        }

        private void ShowMainView()
        {
            DataGridVisibility = "Visible";
            MainControlsVisible = "Visible";
            AddNewClientVilibility = "Hidden";
        }
        private void InformationMessage(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }
        private void ShowUserControlView()
        {
            DataGridVisibility = "Hidden";
            MainControlsVisible = "Hidden";
            LoadingTextBlockVisibility = "Hidden";
        }

        #endregion

        #region Commands
        public ICommand GetAllBookingsCommand { get; private set; }
        public ICommand GetByDateBookingsCommand { get; private set; }
        public ICommand AddBookingCommand { get; private set; }
        public ICommand UpdateBookingCommand { get; private set; }
        public ICommand RemoveBookingCommand { get; private set; }
        public ICommand SortBookingsCommand { get; private set; }
        public ICommand FinishVisitCommand { get; private set; }
        public ICommand AddNewClientCommand { get; private set; }
        public ICommand SaveBookingCommand { get; private set; }
        public ICommand SaveClientCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand CancelBookingCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }
        #endregion
    }
}
