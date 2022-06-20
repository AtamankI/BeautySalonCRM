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
    public class MasterViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private MasterDTO selectedMaster;
        private MasterCategoryDTO selectedMasterCategory;
        private SalaryTypeDTO selectedSalaryType;
        private bool isMastersChecked = false;
        private string mainControlsVisible = "Visible";
        private string loadingTextBlockVisibility = "hidden";
        private string datagridVisibility = "hidden";
        private ObservableCollection<MasterDTO> masters;
        private ObservableCollection<MasterCategoryDTO> masterCategories;
        private ObservableCollection<SalaryTypeDTO> salaryTypes;
        private GeneralService generalService;
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


        public bool IsMastersChecked
        {
            get => isMastersChecked;
            set
            {
                isMastersChecked = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<MasterCategoryDTO> MasterCategories
        {
            get => masterCategories;
            set
            {
                masterCategories = value;
                NotifyOfPropertyChanged();
            }
        }
        public ObservableCollection<SalaryTypeDTO> SalaryTypes
        {
            get => salaryTypes;
            set
            {
                salaryTypes = value;
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

        public MasterDTO SelectedMaster
        {
            get => selectedMaster;
            set
            {
                selectedMaster = value;
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
        public SalaryTypeDTO SelectedSalaryType
        {
            get => selectedSalaryType;
            set
            {
                selectedSalaryType = value;
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

        #endregion
        public MasterViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
            LoadAddDataAsync();
        }
        private void InitCommands()
        {
            GetAllMastersCommand = new RelayCommand(param =>
            {
                IsMastersChecked = false;
                LoadMastersAsync();
            });

            AddMasterCommand = new RelayCommand(async param =>
            {
                LoadingTextBlockVisibility = "Visible";
                SelectedMaster = new MasterDTO();
                CurrentOptionalView = new UpdateMasterView();
                CurrentOptionalView.DataContext = this;
                await Task.Run(() => GetAdditionDataForMaster());
                ShowUserControlView();
                LoadingTextBlockVisibility = "Hidden";
            });
            UpdateMasterCommand = new RelayCommand(async param =>
            {
                if (SelectedMaster == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                CurrentOptionalView = new UpdateMasterView();
                CurrentOptionalView.DataContext = this;
                await Task.Run(() => GetAdditionDataForMaster());
                ShowUserControlView();
            });
            RemoveMasterCommand = new RelayCommand(async param =>
            {
                if (SelectedMaster == null)
                {
                    InformationMessage("You have to select the item!");
                    return;
                }
                await generalService.MasterService.DeleteAsync(SelectedMaster);
                Masters.Remove(SelectedMaster);
            });
            SortMastersCommand = new RelayCommand(param => SortMasters(param));
            ShowRetiredMastersCommand = new RelayCommand(param => LoadMastersAsync());
            SaveMasterCommand = new RelayCommand(param =>
            {
                SaveMaster();
                ResetSelectedItem();
                ShowMainView();
            });
            CancelCommand = new RelayCommand(param =>
            {
                ShowMainView();
                if (SelectedMaster != null && SelectedMaster?.Id != 0)
                    LoadMastersAsync();
                ResetSelectedItem();

            });
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                CurrentOptionalView = new OptionsView();

            });
        }

        #region MasterMethods
        private async void LoadMastersAsync()
        {
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Visible";
            var masters = await generalService.MasterService.GetAllAsync();
            Masters = new ObservableCollection<MasterDTO>(masters);
            if (IsMastersChecked == false)
                Masters = Masters.Where(x => x.RetireDate == null).ToObservableCollection();
            DataGridVisibility = "Visible";
            LoadingTextBlockVisibility = "Hidden";
        }
        private async void SaveMaster()
        {
            if (SelectedMaster.Name == null || SelectedMaster.Lastname == null)
            {
                InformationMessage("Master wasn't added. Master's name and lastname must be filled!");
                return;
            }
            SelectedMaster.Category = SelectedMasterCategory?.Name;
            SelectedMaster.CategoryId = SelectedMasterCategory?.Id;
            SelectedMaster.SalaryPercent = SelectedSalaryType?.Percent;
            SelectedMaster.SalaryTypeId = SelectedSalaryType?.Id;
            if (SelectedMaster?.Id != 0)
            {
                var tmpMaster = SelectedMaster;
                int index = Masters.IndexOf(SelectedMaster);
                Masters.Remove(SelectedMaster);
                await generalService.MasterService.UpdateAsync(tmpMaster);
                Masters.Insert(index, tmpMaster);
                return;
            }
            var masterDto = await generalService.MasterService.CreateAsync(SelectedMaster);
            if (Masters == null)
            {
                InformationMessage("Master was added. To see all masters - press \"Get All\" button");
                ResetSelectedItem();
                return;
            }
            Masters.Add(masterDto);
            ResetSelectedItem();
        }
        private void GetAdditionDataForMaster()
        {
            if (SelectedMaster?.Category != null)
            {
                SelectedMasterCategory = MasterCategories.FirstOrDefault(x => x.Id == SelectedMaster.CategoryId);
            }
            if (SelectedMaster?.SalaryPercent != null)
            {
                SelectedSalaryType = SalaryTypes.FirstOrDefault(x => x.Id == SelectedMaster.SalaryTypeId);
            }
        }

        private async void LoadAddDataAsync()
        {
            var masterCategories = await generalService.MasterCategoryService.GetAllAsync();
            MasterCategories = new ObservableCollection<MasterCategoryDTO>(masterCategories);
            var salaryTypes = await generalService.SalaryService.GetAllAsync();
            SalaryTypes = new ObservableCollection<SalaryTypeDTO>(salaryTypes);
        }

        private void SortMasters(object param)
        {
            switch (param)
            {
                case "MasterNameAsc":
                    Masters = Masters.OrderBy(x => x.Lastname).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "MasterNameDesc":
                    Masters = Masters.OrderByDescending(x => x.Lastname).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "MasterCategoryAsc":
                    Masters = Masters.OrderBy(x => x.Category).ToObservableCollection();
                    break;
                case "MasterCategoryDesc":
                    Masters = Masters.OrderByDescending(x => x.Category).ToObservableCollection();
                    break;
                case "HiredateAsc":
                    Masters = Masters.OrderBy(x => x.HireDate).ToObservableCollection();
                    break;
                case "HiredateDesc":
                    Masters = Masters.OrderByDescending(x => x.HireDate).ToObservableCollection();
                    break;
            }
        }

        #endregion
        private void ResetSelectedItem()
        {
            SelectedMaster = null;
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

        public ICommand CancelCommand { get; private set; }
        public ICommand GetAllMastersCommand { get; private set; }
        public ICommand AddMasterCommand { get; private set; }
        public ICommand UpdateMasterCommand { get; private set; }
        public ICommand RemoveMasterCommand { get; private set; }
        public ICommand SortMastersCommand { get; private set; }
        public ICommand ShowRetiredMastersCommand { get; private set; }
        public ICommand SaveMasterCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }


    }
}
