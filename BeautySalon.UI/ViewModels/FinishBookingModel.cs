using BeautySalon.BLL.DTO;
using BeautySalon.BLL.Services;
using BeautySalon.UI.Extensions;
using BeautySalon.UI.Infrastructure;
using BeautySalon.UI.Views;
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
    public class FinishBookingModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private GeneralService generalService;
        private ClientDTO selectedClient;
        private ServiceDTO selectedService;
        private MasterDTO selectedMaster;
        private VisitDTO selectedVisit;
        private MaterialDTO selectedMaterial;
        private MaterialDTO selectedCalculationMaterial;
        private PaymentTypeDTO selectedPaymentType;
        private static BookingDTO SelectedBooking;
        private ObservableCollection<ClientDTO> clients;
        private ObservableCollection<MasterDTO> masters;
        private ObservableCollection<ServiceDTO> services;
        private ObservableCollection<MaterialDTO> materials;
        private ObservableCollection<MaterialDTO> calculationMaterials;
        private ObservableCollection<PaymentTypeDTO> paymentTypes;
        private string addControlsVisible = "Visible";
        private decimal totalVisitAmount;
        private decimal gramAmount;
        private decimal additionalCost;
        private decimal tips;
        private UserControl currentView;
        private decimal materialTotalAmount;

        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
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
        public MaterialDTO SelectedCalculationMaterial
        {
            get => selectedCalculationMaterial;
            set
            {
                selectedCalculationMaterial = value;
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

        public decimal TotalVisitAmount
        {
            get => totalVisitAmount;
            set
            {
                totalVisitAmount = value;
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
        public string AddControlsVisible
        {
            get => addControlsVisible;
            set
            {
                addControlsVisible = value;
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
        public decimal AdditionalCost
        {
            get => additionalCost;
            set
            {
                additionalCost = value;
                NotifyOfPropertyChanged();
            }
        }
        public decimal Tips
        {
            get => tips;
            set
            {
                tips = value;
                NotifyOfPropertyChanged();
            }
        }

        #endregion
        public FinishBookingModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
            LoadingDataAsync();
        }

        private void InitCommands()
        {
            SaveFinishVisitCommand = new RelayCommand(async param =>
            {

                if (SelectedPaymentType?.Id == null)
                {
                    InformationMessage("Select payment type");
                    return;
                }
                try
                {
                    SelectedVisit = new VisitDTO
                    {
                        Date = SelectedBooking.Date,
                        MastedId = (int)SelectedBooking?.MasterId,
                        ClientId = (int)SelectedBooking.ClientId,
                        ServiceId = (int)SelectedBooking.ServiceID,
                        PaymentTypeId = SelectedPaymentType.Id,
                        MaterialPrice = MaterialTotalAmount,
                        AdditionalCost = AdditionalCost,
                        Tips = Tips
                    };
                    foreach(var item in CalculationMaterials)
                    {
                        var changeMaterial = Materials.FirstOrDefault(x=>x.Name == item.Name && x.Number == item.Number);
                        changeMaterial.TotalAmount = (changeMaterial.GramAmount - item.GramAmount)/changeMaterial.Volume;
                        await generalService.MaterialService.UpdateAsync(changeMaterial);
                    }
                    await generalService.VisitService.CreateAsync(SelectedVisit);
                    await generalService.BookingService.DeleteAsync(SelectedBooking);
                    AddControlsVisible = "Hidden";
                    CurrentView = new BookingView();
                }
                catch
                {

                }
            });
            AddAdditionalCostCommand = new RelayCommand(param => TotalVisitAmount += AdditionalCost);
            AddToCalculationMaterialCommand = new RelayCommand(param =>
            {
               if (SelectedMaterial.GramAmount < GramAmount)
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
                CalculationMaterials.Remove(SelectedCalculationMaterial);
            });
            CancelFinishVisitCommand = new RelayCommand(param =>
            {
                AddControlsVisible = "Hidden";
                CurrentView = new BookingView();
            });
        }

        private async void LoadingDataAsync()
        {
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
            SelectedMaster = Masters.FirstOrDefault(x => x.Id == SelectedBooking.MasterId);
            SelectedClient = Clients.FirstOrDefault(x => x.Id == SelectedBooking.ClientId);
            SelectedService = Services.FirstOrDefault(x => x.Id == SelectedBooking.ServiceID);
            TotalVisitAmount = SelectedService.Price;
            var materials = await generalService.MaterialService.GetAllAsync();
            Materials = new ObservableCollection<MaterialDTO>(materials);
            CalculationMaterials = new ObservableCollection<MaterialDTO>();
        }
        private void InformationMessage(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }
        public static void GetSelectedBooking(BookingDTO bookingDTO)
        {
            SelectedBooking = new BookingDTO();
            SelectedBooking = bookingDTO;
        }

        #region Commands
        public ICommand AddAdditionalCostCommand { get; private set; }
        public ICommand SaveFinishVisitCommand { get; private set; }
        public ICommand CancelFinishVisitCommand { get; private set; }
        public ICommand AddToCalculationMaterialCommand { get; private set; }
        public ICommand RemoveFromCalculationMaterialCommand { get; private set; }
        #endregion

    }
}
