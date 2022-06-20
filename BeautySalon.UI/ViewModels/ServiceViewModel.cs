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
    public class ServiceViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private GeneralService generalService;
        private ServiceDTO selectedService;
        private ServiceCategoryDTO selectedServiceCategory;
        private HallTypeDTO selectedHallType;

        private string serviceControlVisibility = "hidden";
        private string mainControlsVisible = "Visible";
        private string loadingTextBlockVisibility = "hidden";
        private string datagridVisibility = "hidden";
        private bool isServicesChecked = false;
        private ObservableCollection<ServiceDTO> services;
        private ObservableCollection<ServiceCategoryDTO> serviceCategories;
        private ObservableCollection<HallTypeDTO> hallTypes;

        private string selectedMenuControlVisibility = "visible";
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
        public ObservableCollection<ServiceDTO> Services
        {
            get => services;
            set
            {
                services = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<HallTypeDTO> HallTypes
        {
            get => hallTypes;
            set
            {
                hallTypes = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<ServiceCategoryDTO> ServiceCategories
        {
            get => serviceCategories;
            set
            {
                serviceCategories = value;
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
        public bool IsServicesChecked
        {
            get => isServicesChecked;
            set
            {
                isServicesChecked = value;
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
        public string ServiceControlVisibility
        {
            get => serviceControlVisibility;
            set
            {
                serviceControlVisibility = value;
                NotifyOfPropertyChanged();
            }
        }
        public ServiceCategoryDTO SelectedServiceCategory
        {
            get => selectedServiceCategory;
            set
            {
                selectedServiceCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public HallTypeDTO SelectedHallType
        {
            get => selectedHallType;
            set
            {
                selectedHallType = value;
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

        #endregion

        public ServiceViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
            LoadAddDataAsync();
        }

        private void InitCommands()
        {
            GetAllServicesCommand = new RelayCommand(param =>
            {
                IsServicesChecked = false;
                LoadServicesAsync();
            });
            AddServiceCommand = new RelayCommand(async param => {
                LoadingTextBlockVisibility = "Visible";
                SelectedService = new ServiceDTO();
                CurrentView = new UpdateServiceView();
                CurrentView.DataContext = this;
                await Task.Run(() => GetAdditionDataForService());
                ShowUserControlView();
                LoadingTextBlockVisibility = "Hidden";
            });
            UpdateServiceCommand = new RelayCommand(async param =>
            {
                if (SelectedService == null)
                {
                    InformationMessage("You have to select the item to edit!");
                    return;
                }
                CurrentView = new UpdateServiceView();
                CurrentView.DataContext = this;
                await Task.Run(() => GetAdditionDataForService());
                ShowUserControlView();
            });
            RemoveServiceCommand = new RelayCommand(async param =>
            {
                if (SelectedService == null)
                {
                    InformationMessage("You have to select the item to remove!");
                    return;
                }
                try
                {
                    await generalService.ServiceService.DeleteAsync(SelectedService);
                    Services.Remove(SelectedService);
                }
                catch
                {
                    InformationMessage("This service can't be removed. You can make it unavaliable");
                }
            });
            MakeUnavaliableServiceCommand = new RelayCommand(param => MakeServiceUnavaliableAsync());
            SortServicesCommand = new RelayCommand(param => SortServices(param));
            ShowArhiveServiceCommand = new RelayCommand(param => LoadServicesAsync());
            SaveServiceCommand = new RelayCommand(param =>
            {
                SaveService();
                ResetSelectedItem();
                ShowMainView();
            });

            CancelCommand = new RelayCommand(param => {
                ShowMainView();
                if (SelectedService != null && SelectedService?.Id != 0)
                    LoadServicesAsync();
                ResetSelectedItem();

            });
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                CurrentView = new MenuView();
                
            });

        }

        #region ServiceMethods
        private async void LoadServicesAsync()
        {
            ServiceControlVisibility = "Hidden";
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Visible";
            var services = await generalService.ServiceService.GetAllAsync();
            Services = new ObservableCollection<ServiceDTO>(services);
            foreach (var service in Services)
            {
                if (service.Avaliability == 0)
                    service.IsAvaliable = true;
            }
            if (IsServicesChecked == false)
                Services = Services.Where(x => x.IsAvaliable).ToObservableCollection();
            DataGridVisibility = "Visible";
            LoadingTextBlockVisibility = "Hidden";
        }
        private void GetAdditionDataForService()
        {
            if (SelectedService?.CategoryName != null)
            {
                SelectedServiceCategory = ServiceCategories.FirstOrDefault(x => x.Id == SelectedService.CategoryId);
            }
            if (SelectedService?.HallType != null)
            {
                SelectedHallType = HallTypes.FirstOrDefault(x => x.Id == SelectedService.HallId);
            }
        }

        private async void LoadAddDataAsync()
        {
            var serviceCategories = await generalService.ServiceCategoryService.GetAllAsync();
            ServiceCategories = new ObservableCollection<ServiceCategoryDTO>(serviceCategories);
            var hallTypes = await generalService.HallTypeService.GetAllAsync();
            HallTypes = new ObservableCollection<HallTypeDTO>(hallTypes);
        }
        private async void SaveService()
        {
            if (SelectedService.Name == null)
            {
                InformationMessage("Service wasn't added. Service's name must be filled!");
                return;
            }
            if (SelectedService.IsAvaliable)
            {
                SelectedService.Avaliability = 0;
            }
            else SelectedService.Avaliability = 1;
            SelectedService.HallType = SelectedHallType?.HallType;
            SelectedService.HallId = SelectedHallType?.Id;
            SelectedService.CategoryName = SelectedServiceCategory?.Name;
            SelectedService.CategoryId = SelectedServiceCategory?.Id;
            if (SelectedService?.Id != 0)
            {
                var tmpService = SelectedService;
                int index = Services.IndexOf(SelectedService);
                Services.Remove(SelectedService);
                await generalService.ServiceService.UpdateAsync(tmpService);
                Services.Insert(index, tmpService);
                return;
            }
            var serviceDto = await generalService.ServiceService.CreateAsync(SelectedService);
            if (Services == null)
            {
                InformationMessage("Service was added. To see all services - press \"Get All\" button");
                ResetSelectedItem();
                return;
            }
            Services.Add(serviceDto);
            ResetSelectedItem();
        }
        private void SortServices(object param)
        {
            switch (param)
            {
                case "ServiceNameAsc":
                    Services = Services.OrderBy(x => x.Name).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "ServiceNameDesc":
                    Services = Services.OrderByDescending(x => x.Name).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "ServiceTypeAsc":
                    Services = Services.OrderBy(x => x.CategoryName).ToObservableCollection();
                    break;
                case "ServiceTypeDesc":
                    Services = Services.OrderByDescending(x => x.CategoryName).ToObservableCollection();
                    break;
            }
        }
        private async void MakeServiceUnavaliableAsync()
        {
            if (SelectedService == null)
            {
                InformationMessage("You have to select the item!");
                return;
            }
            if (SelectedService?.Id != 0)
            {
                var tmpService = SelectedService;
                int index = Services.IndexOf(SelectedService);
                Services.Remove(SelectedService);
                tmpService.IsAvaliable = false;
                tmpService.Avaliability = 1;
                await generalService.ServiceService.UpdateAsync(tmpService);
                Services.Insert(index, tmpService);
                return;
            }
            ResetSelectedItem();
        }

        #endregion

        #region MainMethods
        private void ResetSelectedItem()
        {
            SelectedService = null;
            SelectedServiceCategory = null;
            SelectedHallType = null;
        }
        private void InformationMessage(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }
        private void ShowUserControlView()
        {
            DataGridVisibility = "Hidden";
            ServiceControlVisibility = "Visible";
            MainControlsVisible = "Hidden";
            LoadingTextBlockVisibility = "Hidden";
        }
        private void ShowMainView()
        {
            ServiceControlVisibility = "Hidden";
            DataGridVisibility = "Visible";
            MainControlsVisible = "Visible";
        }
        #endregion


        #region Commands
        public ICommand CancelCommand { get; private set; }
        public ICommand GetAllServicesCommand { get; private set; }
        public ICommand AddServiceCommand { get; private set; }
        public ICommand UpdateServiceCommand { get; private set; }
        public ICommand RemoveServiceCommand { get; private set; }
        public ICommand MakeUnavaliableServiceCommand { get; private set; }
        public ICommand SortServicesCommand { get; private set; }
        public ICommand ShowArhiveServiceCommand { get; private set; }
        public ICommand SaveServiceCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }
        #endregion
    }

}
