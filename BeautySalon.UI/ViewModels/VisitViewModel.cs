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
    public class VisitViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private string mainControlsVisible = "Visible";
        private string datagridVisibility = "hidden";
        private string loadingTextBlockVisibility = "hidden";
        private string addControlsVisible = "hidden";
        private GeneralService generalService;
        private VisitDTO selectedVisit;
        private ClientDTO selectedClient;
        private ServiceDTO selectedService;
        private MasterDTO selectedMaster;
        private MaterialDTO selectedMaterial;
        private MaterialDTO selectedCalculationMaterial;
        private PaymentTypeDTO selectedPaymentType;
        private ObservableCollection<ClientDTO> clients;
        private ObservableCollection<MasterDTO> masters;
        private ObservableCollection<ServiceDTO> services;
        private ObservableCollection<VisitDTO> visits;
        private ObservableCollection<PaymentTypeDTO> paymentTypes;
        private ObservableCollection<MaterialDTO> materials;
        private ObservableCollection<MaterialDTO> calculationMaterials;
        private decimal gramAmount;
        private decimal additionalCost;
        private decimal materialCost;
        private UserControl currentView;
        private decimal totalVisitAmount;
        private DateTime selectedDate;
        private decimal materialTotalAmount;

        public MaterialDTO SelectedCalculationMaterial
        {
            get => selectedCalculationMaterial;
            set
            {
                selectedCalculationMaterial = value;
                NotifyOfPropertyChanged();
            }
        }
        public MaterialDTO SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                selectedMaterial = value;
                NotifyOfPropertyChanged();
            }
        }
        public VisitDTO SelectedVisit
        {
            get => selectedVisit;
            set
            {
                selectedVisit = value;
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
        public PaymentTypeDTO SelectedPaymentType
        {
            get => selectedPaymentType;
            set
            {
                selectedPaymentType = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<VisitDTO> Visits
        {
            get => visits;
            set
            {
                visits = value;
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
        public ObservableCollection<PaymentTypeDTO> PaymentTypes
        {
            get => paymentTypes;
            set
            {
                paymentTypes = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<MaterialDTO> CalculationMaterials
        {
            get => calculationMaterials;
            set
            {
                calculationMaterials = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<MaterialDTO> Materials
        {
            get => materials;
            set
            {
                materials = value;
                NotifyOfPropertyChanged();
            }
        }
        public decimal TotalVisitAmount
        {
            get => totalVisitAmount;
            set
            {
                totalVisitAmount = value;
                NotifyOfPropertyChanged();
            }
        }
        public decimal MaterialCost
        {
            get => materialCost;
            set
            {
                materialCost = value;
                NotifyOfPropertyChanged();
            }
        }
        public decimal AdditionalCost
        {
            get => additionalCost;
            set
            {
                additionalCost = value;
                NotifyOfPropertyChanged();
            }
        }
        public decimal GramAmount
        {
            get => gramAmount;
            set
            {
                gramAmount = value;
                NotifyOfPropertyChanged();
            }
        }
        public decimal MaterialTotalAmount
        {
            get => materialTotalAmount;
            set
            {
                materialTotalAmount = value;
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
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
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
        public string AddControlsVisible
        {
            get => addControlsVisible;
            set
            {
                addControlsVisible = value;
                NotifyOfPropertyChanged();
            }
        }
        #endregion

        public VisitViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
            LoadingDataAsync();
        }
        private void InitCommands()
        {
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                CurrentView = new MenuView();

            });
            GetAllVisitsCommand = new RelayCommand(async param =>
            {
                await LoadVisitsAsync();
            });
            GetByDateVisitsCommand = new RelayCommand(async param =>
            {
                await LoadVisitsAsync();
                try
                {
                    Visits = Visits.Where(x => x.Date == SelectedDate)?.ToObservableCollection();
                }
                catch
                { }
            });
            AddVisitCommand = new RelayCommand(async param =>
            {
                LoadingTextBlockVisibility = "Visible";
                SelectedVisit = new VisitDTO();
                SelectedVisit.Date = DateTime.Now;
                CurrentView = new UpdateVisitView();
                CurrentView.DataContext = this;
                await Task.Run(() => GetAdditionDataForVisit());
                ShowUserControlView();
                LoadingTextBlockVisibility = "Hidden";
            });
            UpdateVisitCommand = new RelayCommand(async param =>
            {
                if (SelectedVisit == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                CurrentView = new UpdateVisitView();
                CurrentView.DataContext = this;
                await Task.Run(() => GetAdditionDataForVisit());
                ShowUserControlView();
            });
            RemoveVisitCommand = new RelayCommand(async param =>
            {
                if (SelectedVisit == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                await generalService.VisitService.DeleteAsync(SelectedVisit);
                Visits.Remove(SelectedVisit);
            });
            AddToCalculationMaterialCommand = new RelayCommand(param =>
            {
                CalculationMaterials = new ObservableCollection<MaterialDTO>();
                if (SelectedMaterial == null || SelectedMaterial?.GramAmount < GramAmount || GramAmount == 0)
                {
                    InformationMessage("Select available amount");
                    return;
                }
                SelectedCalculationMaterial = new MaterialDTO
                {
                    Name = SelectedMaterial.Name,
                    Number = SelectedMaterial.Number,
                    GramAmount = GramAmount,
                    Price = GramAmount * SelectedMaterial.PriceGram
                };
                MaterialTotalAmount += (decimal)SelectedCalculationMaterial.Price;
                CalculationMaterials.Add(SelectedCalculationMaterial);
            });
            RemoveFromCalculationMaterialCommand = new RelayCommand(param =>
            {
                if (SelectedMaterial == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                CalculationMaterials.Remove(SelectedCalculationMaterial);
            });
            SortVisitsCommand = new RelayCommand(param => SortVisits(param));
            SaveVisitCommand = new RelayCommand(param =>
            {
                SaveVisit();
                ResetSelectedItem();
                ShowMainView();
            });
            CountVisitCommand = new RelayCommand(param => 
                                                        TotalVisitAmount = AdditionalCost + MaterialCost + SelectedService.Price, 
                                                        x=>SelectedService?.Price != 0);
            CancelCommand = new RelayCommand(async param => 

            {
                ShowMainView();
                if (SelectedVisit != null && SelectedVisit?.Id != 0)
                    await LoadVisitsAsync();
                ResetSelectedItem();

            });
        }
        #region VisitMethods
        private async void SaveVisit()
        {
            if (SelectedMaster == null || SelectedClient == null || SelectedService == null || SelectedPaymentType == null)
            {
                InformationMessage("Visit wasn't added. Master, client, service and paymentType must be filled!");
                return;
            }
            SelectedVisit.MastedId = SelectedMaster.Id;
            SelectedVisit.MasterLastname = SelectedMaster.Lastname;
            SelectedVisit.MasterName = SelectedMaster.Name;
            SelectedVisit.ClientId = SelectedClient.Id;
            SelectedVisit.ClientLastname = SelectedClient.Lastname;
            SelectedVisit.ClientName = SelectedClient.Name;
            SelectedVisit.AdditionalCost = AdditionalCost;
            SelectedVisit.Amount = TotalVisitAmount;
            SelectedVisit.MaterialPrice = MaterialCost;
            SelectedVisit.ServiceId = SelectedService.Id;
            SelectedVisit.PaymentTypeId = SelectedPaymentType.Id;
            foreach (var item in CalculationMaterials)
            {
                var changeMaterial = Materials.FirstOrDefault(x => x.Name == item.Name && x.Number == item.Number);
                changeMaterial.TotalAmount = (changeMaterial.GramAmount - item.GramAmount) / changeMaterial.Volume;
                await generalService.MaterialService.UpdateAsync(changeMaterial);
            }
            if (SelectedVisit?.Id != 0)
            {
                var tmpVisit = SelectedVisit;
                int index = Visits.IndexOf(SelectedVisit);
                Visits.Remove(SelectedVisit);
                await generalService.VisitService.UpdateAsync(tmpVisit);
                Visits.Insert(index, tmpVisit);
                return;
            }
            var visitDto = await generalService.VisitService.CreateAsync(SelectedVisit);
            if (Visits == null)
            {
                InformationMessage("Visit was added. To see all visits - press \"Get All\" button");
                ResetSelectedItem();
                return;
            }
            Visits.Add(visitDto);
            ResetSelectedItem();
        }
        private void SortVisits(object param)
        {
            switch (param)
            { 
                case "DateAsc":
                    Visits = Visits.OrderBy(x => x.Date).ToObservableCollection();
                    break;
                case "DateDesc":
                    Visits = Visits.OrderByDescending(x => x.Date).ToObservableCollection();
                    break;
                case "MasterNameAsc":
                    Visits = Visits.OrderBy(x => x.MasterLastname).ThenBy(x => x.MasterName).ToObservableCollection();
                    break;
                case "MasterNameDesc":
                    Visits = Visits.OrderByDescending(x => x.MasterLastname).ThenBy(x => x.MasterName).ToObservableCollection();
                    break;
            }
        }
        private void GetAdditionDataForVisit()
        {
            if (SelectedVisit?.MastedId != null)
            {
                SelectedMaster = Masters.FirstOrDefault(x => x.Id == SelectedVisit.MastedId);
            }
            if (SelectedVisit?.ClientId != null)
            {
                SelectedClient = Clients.FirstOrDefault(x => x.Id == SelectedVisit.ClientId);
            }
            if (SelectedVisit?.AdditionalCost != null)
            {
                AdditionalCost = SelectedVisit.AdditionalCost;
            }
            if (SelectedVisit?.MaterialPrice != null)
            {
                MaterialCost = SelectedVisit.MaterialPrice;
            }
            if (SelectedVisit?.ServiceId != 0)
            {
                SelectedService = Services.FirstOrDefault(x => x.Id == SelectedVisit.ServiceId);
                SelectedService.Price = Services.FirstOrDefault(x => x.Id == SelectedVisit.ServiceId).Price;
                TotalVisitAmount = AdditionalCost + SelectedService.Price;
            }
            if (SelectedVisit?.PaymentTypeId != 0)
            {
                SelectedPaymentType = PaymentTypes.FirstOrDefault(x => x.Id == SelectedVisit.PaymentTypeId);
            }
            
        }
        private async Task LoadVisitsAsync()
        {
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Visible";
            var visits = await generalService.VisitService.GetAllAsync();
            Visits = new ObservableCollection<VisitDTO>(visits);
            DataGridVisibility = "Visible";
            LoadingTextBlockVisibility = "Hidden";
        }
        private async void LoadingDataAsync()
        {
            SelectedDate = DateTime.Now;
            var payments = await generalService.PaymentService.GetAllAsync();
            PaymentTypes = new ObservableCollection<PaymentTypeDTO>(payments);
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
            var materials = await generalService.MaterialService.GetAllAsync();
            Materials = new ObservableCollection<MaterialDTO>(materials);
        }
        #endregion
        #region OtherMethods
        private void ResetSelectedItem()
        {
            SelectedVisit = null;
            SelectedService = null;
            SelectedMaster = null;
            SelectedMaterial = null;
            SelectedPaymentType = null;
            TotalVisitAmount = 0;
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
            AddControlsVisible = "Visible";
        }
        private void ShowMainView()
        {
            DataGridVisibility = "Visible";
            MainControlsVisible = "Visible";
            AddControlsVisible = "Hidden";
        }

        #endregion
        #region Commands
        public ICommand CancelCommand { get; private set; }
        public ICommand GetByDateVisitsCommand { get; private set; }
        public ICommand GetAllVisitsCommand { get; private set; }
        public ICommand AddVisitCommand { get; private set; }
        public ICommand UpdateVisitCommand { get; private set; }
        public ICommand CountVisitCommand { get; private set; }
        public ICommand AddToCalculationMaterialCommand { get; private set; }
        public ICommand RemoveFromCalculationMaterialCommand { get; private set; }
        public ICommand RemoveVisitCommand { get; private set; }
        public ICommand SortVisitsCommand { get; private set; }
        public ICommand SaveVisitCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }
        #endregion
    }
}
