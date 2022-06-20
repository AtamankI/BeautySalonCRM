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
    public class ClientViewModel : BaseNotifyPropertyChanged 
    {
        #region Properties
        private GeneralService generalService;
        private ClientDTO selectedClient;
        private ObservableCollection<ClientDTO> clients;
        private UserControl currentOptionalView;

        private bool isClientsChecked = false;
        private string mainControlsVisible = "Visible";
        private string loadingTextBlockVisibility = "hidden";
        private string datagridVisibility = "hidden";
        private string selectedMenuControlVisibility = "visible";

        public string SelectedMenuControlVisibility
        {
            get => selectedMenuControlVisibility;
            set
            {
                selectedMenuControlVisibility = value;
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
        public bool IsClientsChecked
        {
            get => isClientsChecked;
            set
            {
                isClientsChecked = value;
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
        public UserControl CurrentOptionalView
        {
            get => currentOptionalView;
            set
            {
                currentOptionalView = value;
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
        public ObservableCollection<ClientDTO> Clients
        {
            get => clients;
            set
            {
                clients = value;
                NotifyOfPropertyChanged();
            }
        }


        #endregion

        public ClientViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();

        }
        private void InitCommands()
        {
            GetAllClientsCommand = new RelayCommand(param =>
            {
                IsClientsChecked = false;
                LoadClientsAsync();
            });
            AddClientCommand = new RelayCommand(param => {
                LoadingTextBlockVisibility = "Visible";
                SelectedClient = new ClientDTO();
                CurrentOptionalView = new UpdateClientView();
                CurrentOptionalView.DataContext = this;
                ShowUserControlView();
                LoadingTextBlockVisibility = "Hidden";
            });
            UpdateClientCommand = new RelayCommand(param => {
                if (SelectedClient == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                CurrentOptionalView = new UpdateClientView();
                CurrentOptionalView.DataContext = this;
                ShowUserControlView();
            });
            RemoveClientCommand = new RelayCommand(async param =>
            {

                if (SelectedClient == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                try
                {
                    await generalService.ClientService.DeleteAsync(SelectedClient);
                    Clients.Remove(SelectedClient);
                }
                catch
                {
                    InformationMessage("This client can't be removed. You can move him in archive");
                }
            });
            SortClientsCommand = new RelayCommand(param => SortClients(param));
            ShowArhiveClientsCommand = new RelayCommand(param => LoadClientsAsync());
            SaveClientCommand = new RelayCommand(param =>
            {
                SaveClient();
                ResetSelectedItem();
                ShowMainView();
            }); 
            CancelCommand = new RelayCommand(param => {
                ShowMainView();
                if (SelectedClient != null && SelectedClient?.Id != 0)
                    LoadClientsAsync();
                ResetSelectedItem();

            });
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                CurrentOptionalView = new OptionsView();

            });

        }

        #region ClientMethods
        private async void LoadClientsAsync()
        {
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Visible";
            var clients = await generalService.ClientService.GetAllAsync();
            Clients = new ObservableCollection<ClientDTO>(clients);
            foreach (var client in Clients)
            {
                if (client.InArchive == 1)
                    client.IsInArhive = true;
            }
            if (IsClientsChecked == false)
                Clients = Clients.Where(x => x.IsInArhive == false).ToObservableCollection();
            DataGridVisibility = "Visible";
            LoadingTextBlockVisibility = "Hidden";
        }
        private async void SaveClient()
        {
            if (SelectedClient.Name == null)
            {
                InformationMessage("Client wasn't added. Client's name must be filled!");
                return;
            }
            if (SelectedClient.IsInArhive)
            {
                SelectedClient.InArchive = 1;
            }
            else SelectedClient.InArchive = 0;
            if (SelectedClient?.Id != 0)
            {
                var tmpClient = SelectedClient;
                int index = Clients.IndexOf(SelectedClient);
                Clients.Remove(SelectedClient);
                await generalService.ClientService.UpdateAsync(tmpClient);
                Clients.Insert(index, tmpClient);
                return;
            }
            var clientDto = await generalService.ClientService.CreateAsync(SelectedClient);
            if (Clients == null)
            {
                InformationMessage("Client was added. To see all clients - press \"Get All\" button");
                ResetSelectedItem();
                return;
            }
            Clients.Add(clientDto);
            ResetSelectedItem();
        }
        private void SortClients(object param)
        {
            switch (param)
            {
                case "ClientNameAsc":
                    Clients = Clients.OrderBy(x => x.Lastname).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "ClientNameDesc":
                    Clients = Clients.OrderByDescending(x => x.Lastname).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "NumberOfVisitsAsc":
                    Clients = Clients.OrderBy(x => x.NumberOfVisits).ToObservableCollection();
                    break;
                case "NumberOfVisitsDesc":
                    Clients = Clients.OrderByDescending(x => x.NumberOfVisits).ToObservableCollection();
                    break;
                case "LastVisitAsc":
                    Clients = Clients.OrderBy(x => x.LastVisit).ToObservableCollection();
                    break;
                case "LastVisitDesc":
                    Clients = Clients.OrderByDescending(x => x.LastVisit).ToObservableCollection();
                    break;
            }
        }

        #endregion

        #region MainMethods
        private void ResetSelectedItem()
        {
            SelectedClient = null;
        }
        private void InformationMessage(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }
        private void ShowUserControlView()
        {
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Hidden";
            MainControlsVisible = "Hidden";
        }
        private void ShowMainView()
        {
            DataGridVisibility = "Visible";
            MainControlsVisible = "Visible";
        }
        #endregion


        #region Commands
        public ICommand CancelCommand { get; private set; }

        public ICommand GetAllClientsCommand { get; private set; }
        public ICommand AddClientCommand { get; private set; }
        public ICommand UpdateClientCommand { get; private set; }
        public ICommand RemoveClientCommand { get; private set; }
        public ICommand SortClientsCommand { get; private set; }
        public ICommand ShowArhiveClientsCommand { get; private set; }
        public ICommand SaveClientCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }


        #endregion
    }
}

