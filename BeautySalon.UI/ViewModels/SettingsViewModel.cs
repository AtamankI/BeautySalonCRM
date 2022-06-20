using BeautySalon.BLL.DTO;
using BeautySalon.BLL.Services;
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
    public class SettingsViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private GeneralService generalService;

        private PaymentTypeDTO selectedPayment;
        private MasterCategoryDTO selectedMasterCategory;
        private MaterialCategoryDTO selectedMaterialCategory;
        private MaterialManufacturerDTO selectedManufacturerCategory;
        private ServiceCategoryDTO selectedServiceCategory;
        private PaymentTypeDTO newPayment;
        private MasterCategoryDTO newMasterCategory;
        private MaterialCategoryDTO newMaterialCategory;
        private MaterialManufacturerDTO newManufacturerCategory;
        private ServiceCategoryDTO newServiceCategory;

        private ObservableCollection<PaymentTypeDTO> payments;
        private ObservableCollection<MasterCategoryDTO> masterCategoties;
        private ObservableCollection<MaterialCategoryDTO> materialCategories;
        private ObservableCollection<MaterialManufacturerDTO> materialManufacturers;
        private ObservableCollection<ServiceCategoryDTO> serviceCategories;


        private string mainControlsVisible = "Visible";
        public string MainControlsVisible
        {
            get => mainControlsVisible;
            set
            {
                mainControlsVisible = value;
                NotifyOfPropertyChanged();
            }
        }
        private UserControl currentOptionalView;
        public UserControl CurrentOptionalView
        {
            get => currentOptionalView;
            set
            {
                currentOptionalView = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<PaymentTypeDTO> Payments
        {
            get => payments;
            set
            {
                payments = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<MasterCategoryDTO> MasterCategoties
        {
            get => masterCategoties;
            set
            {
                masterCategoties = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<MaterialCategoryDTO> MaterialCategories
        {
            get => materialCategories;
            set
            {
                materialCategories = value;
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
        public ObservableCollection<MaterialManufacturerDTO> MaterialManufacturers
        {
            get => materialManufacturers;
            set
            {
                materialManufacturers = value;
                NotifyOfPropertyChanged();
            }
        }
        public ServiceCategoryDTO NewServiceCategory
        {
            get => newServiceCategory;
            set
            {
                newServiceCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public MaterialManufacturerDTO NewManufacturerCategory
        {
            get => newManufacturerCategory;
            set
            {
                newManufacturerCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public MaterialCategoryDTO NewMaterialCategory
        {
            get => newMaterialCategory;
            set
            {
                newMaterialCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public MasterCategoryDTO NewMasterCategory
        {
            get => newMasterCategory;
            set
            {
                newMasterCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public PaymentTypeDTO NewPayment
        {
            get => newPayment;
            set
            {
                newPayment = value;
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
        public MaterialManufacturerDTO SelectedManufacturerCategory
        {
            get => selectedManufacturerCategory;
            set
            {
                selectedManufacturerCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public MaterialCategoryDTO SelectedMaterialCategory
        {
            get => selectedMaterialCategory;
            set
            {
                selectedMaterialCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public MasterCategoryDTO SelectedMasterCategory
        {
            get => selectedMasterCategory;
            set
            {
                selectedMasterCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public PaymentTypeDTO SelectedPayment
        {
            get => selectedPayment;
            set
            {
                selectedPayment = value;
                NotifyOfPropertyChanged();
            }
        }
        #endregion

        public SettingsViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
            LoadOptionsAsync();
        }
        private void InitCommands()
        {
            AddPaymentTypeCommand = new RelayCommand(async param =>
            {
                if (NewPayment?.Type != null && Payments.FirstOrDefault(x => x.Type == NewPayment?.Type) == null)
                {
                    await generalService.PaymentService.CreateAsync(NewPayment);
                    Payments.Add(NewPayment);
                    NewPayment = new PaymentTypeDTO();
                }
            });
            ChangePaymentTypeCommand = new RelayCommand(param =>
            {
                if (SelectedPayment?.Type != null)
                {
                    NewPayment = SelectedPayment;
                }
            });
            UpdatePaymentTypeCommand = new RelayCommand(async param =>
            {
                if (NewPayment?.Type != null)
                {
                    int index = Payments.IndexOf(SelectedPayment);
                    Payments.Remove(SelectedPayment);
                    await generalService.PaymentService.UpdateAsync(NewPayment);
                    Payments.Insert(index, NewPayment);
                    NewPayment = new PaymentTypeDTO();
                }
            });
            AddMasterCategoryCommand = new RelayCommand(async param =>
            {
                if (NewMasterCategory?.Name != null && MasterCategoties.FirstOrDefault(x => x.Name == NewMasterCategory?.Name) == null)
                {
                    await generalService.MasterCategoryService.CreateAsync(NewMasterCategory);
                    MasterCategoties.Add(NewMasterCategory);
                    NewMasterCategory = new MasterCategoryDTO();
                }
            });
            ChangeMasterCategoryCommand = new RelayCommand(param =>
            {
                if (SelectedMasterCategory?.Name != null)
                {
                    NewMasterCategory = SelectedMasterCategory;
                }
            });
            UpdateMasterCategoryCommand = new RelayCommand(async param =>
            {
                if (NewMasterCategory?.Name != null)
                {
                    int index = MasterCategoties.IndexOf(SelectedMasterCategory);
                    MasterCategoties.Remove(SelectedMasterCategory);
                    await generalService.MasterCategoryService.UpdateAsync(NewMasterCategory);
                    MasterCategoties.Insert(index, NewMasterCategory);
                    NewMasterCategory = new MasterCategoryDTO();
                }
            });
            AddMaterialCategoryCommand = new RelayCommand(async param =>
            {
                if (NewMaterialCategory?.Name != null && MaterialCategories.FirstOrDefault(x => x.Name == NewMaterialCategory?.Name) == null)
                {
                    await generalService.MaterialCategoryService.CreateAsync(NewMaterialCategory);
                    MaterialCategories.Add(NewMaterialCategory);
                    NewMaterialCategory = new MaterialCategoryDTO();
                }
            });
            ChangeMaterialCategoryCommand = new RelayCommand(param =>
            {
                if (SelectedMaterialCategory?.Name != null)
                {
                    NewMaterialCategory = SelectedMaterialCategory;
                }
            });
            UpdateMaterialCategoryCommand = new RelayCommand(async param =>
            {
                if (NewMaterialCategory?.Name != null)
                {
                    int index = MaterialCategories.IndexOf(SelectedMaterialCategory);
                    MaterialCategories.Remove(SelectedMaterialCategory);
                    await generalService.MaterialCategoryService.UpdateAsync(NewMaterialCategory);
                    MaterialCategories.Insert(index, NewMaterialCategory);
                    NewMaterialCategory = new MaterialCategoryDTO();
                }
            });
            AddMaterialManufacturerCommand = new RelayCommand(async param =>
            {
                if (NewManufacturerCategory?.Name != null)
                {
                    await generalService.MaterialManufacturerService.CreateAsync(NewManufacturerCategory);
                    MaterialManufacturers.Add(NewManufacturerCategory);
                    NewManufacturerCategory = new MaterialManufacturerDTO();
                }
            });
            ChangeManufacturerCategoryCommand = new RelayCommand(param =>
            {
                if (SelectedManufacturerCategory?.Name != null)
                {
                    NewManufacturerCategory = SelectedManufacturerCategory;
                }
            });
            UpdateMaterialManufacturerCommand = new RelayCommand(async param =>
            {
                if (NewManufacturerCategory?.Name != null)
                {
                    NewManufacturerCategory.Id = SelectedManufacturerCategory.Id;
                    int index = MaterialManufacturers.IndexOf(SelectedManufacturerCategory);
                    MaterialManufacturers.Remove(SelectedManufacturerCategory);
                    await generalService.MaterialManufacturerService.UpdateAsync(NewManufacturerCategory);
                    MaterialManufacturers.Insert(index, NewManufacturerCategory);
                    NewManufacturerCategory = new MaterialManufacturerDTO();

                }
            });

            AddServiceCategoryCommand = new RelayCommand(async param =>
            {
                if (NewServiceCategory?.Name != null && ServiceCategories.FirstOrDefault(x => x.Name == NewServiceCategory?.Name)?.Name == null)
                {
                    await generalService.ServiceCategoryService.CreateAsync(NewServiceCategory);
                    ServiceCategories.Add(NewServiceCategory);
                    NewServiceCategory = new ServiceCategoryDTO();

                }

            });
            ChangeServiceCategoryCommand = new RelayCommand(param =>
            {
                if (SelectedServiceCategory?.Name != null)
                {
                    NewServiceCategory = SelectedServiceCategory;
                }
            });
            UpdateServiceCategoryCommand = new RelayCommand(async param =>
            {
                if (NewServiceCategory?.Name != null)
                {
                    int index = ServiceCategories.IndexOf(SelectedServiceCategory);
                    ServiceCategories.Remove(SelectedServiceCategory);
                    await generalService.ServiceCategoryService.UpdateAsync(NewServiceCategory);
                    ServiceCategories.Insert(index, NewServiceCategory);
                    NewServiceCategory = new ServiceCategoryDTO();
                }

            });
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                CurrentOptionalView = new OptionsView();

            });

        }
        private async void LoadOptionsAsync()
        {
            NewManufacturerCategory = new MaterialManufacturerDTO();
            NewPayment = new PaymentTypeDTO();
            NewMasterCategory = new MasterCategoryDTO();
            NewMaterialCategory = new MaterialCategoryDTO();
            NewServiceCategory = new ServiceCategoryDTO();
            var masterCategoties = await generalService.MasterCategoryService.GetAllAsync();
            MasterCategoties = new ObservableCollection<MasterCategoryDTO>(masterCategoties);

            var payments = await generalService.PaymentService.GetAllAsync();
            Payments = new ObservableCollection<PaymentTypeDTO>(payments);

            var materialCategories = await generalService.MaterialCategoryService.GetAllAsync();
            MaterialCategories = new ObservableCollection<MaterialCategoryDTO>(materialCategories);

            var serviceCategories = await generalService.ServiceCategoryService.GetAllAsync();
            ServiceCategories = new ObservableCollection<ServiceCategoryDTO>(serviceCategories);

            var materialManufacturers = await generalService.MaterialManufacturerService.GetAllAsync();
            MaterialManufacturers = new ObservableCollection<MaterialManufacturerDTO>(materialManufacturers);

        }

        #region Commands
        public ICommand AddPaymentTypeCommand { get; private set; }
        public ICommand ChangePaymentTypeCommand { get; private set; }
        public ICommand UpdatePaymentTypeCommand { get; private set; }
        public ICommand AddMasterCategoryCommand { get; private set; }
        public ICommand ChangeMasterCategoryCommand { get; private set; }
        public ICommand UpdateMasterCategoryCommand { get; private set; }
        public ICommand AddMaterialCategoryCommand { get; private set; }
        public ICommand ChangeMaterialCategoryCommand { get; private set; }
        public ICommand UpdateMaterialCategoryCommand { get; private set; }
        public ICommand AddMaterialManufacturerCommand { get; private set; }
        public ICommand ChangeManufacturerCategoryCommand { get; private set; }
        public ICommand UpdateMaterialManufacturerCommand { get; private set; }
        public ICommand AddServiceCategoryCommand { get; private set; }
        public ICommand ChangeServiceCategoryCommand { get; private set; }
        public ICommand UpdateServiceCategoryCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }
        #endregion

    }
}
