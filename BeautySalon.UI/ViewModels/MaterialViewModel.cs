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
    public  class MaterialViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private GeneralService generalService;
        private MaterialDTO selectedMaterial;
        private MaterialManufacturerDTO selectedMaterialManufacturer;
        private MaterialCategoryDTO selectedMaterialCategory;

        private string mainControlsVisible = "Visible";
        private string loadingTextBlockVisibility = "hidden";
        private string datagridVisibility = "hidden";
        private string selectedMenuControlVisibility = "visible";

        private ObservableCollection<MaterialDTO> materials;
        private ObservableCollection<MaterialManufacturerDTO> materialManufacturers;
        private ObservableCollection<MaterialCategoryDTO> materialCategories;

        private UserControl currentOptionalView;
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
        public ObservableCollection<MaterialManufacturerDTO> MaterialManufacturers
        {
            get => materialManufacturers;
            set
            {
                materialManufacturers = value;
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
        public ObservableCollection<MaterialDTO> Materials
        {
            get => materials;
            set
            {
                materials = value;
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
        public MaterialCategoryDTO SelectedMaterialCategory
        {
            get => selectedMaterialCategory;
            set
            {
                selectedMaterialCategory = value;
                NotifyOfPropertyChanged();
            }
        }
        public MaterialManufacturerDTO SelectedMaterialManufacturer
        {
            get => selectedMaterialManufacturer;
            set
            {
                selectedMaterialManufacturer = value;
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

        #endregion
        public MaterialViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
            LoadAddDataAsync();
        }

        private void InitCommands()
        {
            GetAllMaterialsCommand = new RelayCommand(param => LoadMaterialsAsync());

            AddMaterialCommand = new RelayCommand(async param =>
            {
                LoadingTextBlockVisibility = "Visible";
                SelectedMaterial = new MaterialDTO();
                CurrentOptionalView = new UpdateMaterialView();
                CurrentOptionalView.DataContext = this;
                await Task.Run(() => GetAdditionDataForMaterial());
                ShowUserControlView();

                LoadingTextBlockVisibility = "Hidden";
            });
            UpdateMaterialCommand = new RelayCommand(async param =>
            {
                if (SelectedMaterial == null)
                {
                    InformationMessage("You have to select the item to edit!");
                    return;
                }
                CurrentOptionalView = new UpdateMaterialView();
                CurrentOptionalView.DataContext = this;
                await Task.Run(() => GetAdditionDataForMaterial());
                ShowUserControlView();
            });
            RemoveMaterialCommand = new RelayCommand(async param =>
            {
                if (SelectedMaterial == null)
                {
                    InformationMessage("You have to select the item to remove!");
                    return;
                }
                await generalService.MaterialService.DeleteAsync(SelectedMaterial);
                Materials.Remove(SelectedMaterial);
            });
            SortMaterialsCommand = new RelayCommand(param => SortMaterials(param));
            SaveMaterialCommand = new RelayCommand(param =>
                {
                    SaveMaterial();
                    ResetSelectedItem();
                    ShowMainView();
                });

            CancelCommand = new RelayCommand(param =>
            {
                ShowMainView();
                if (SelectedMaterial != null && SelectedMaterial?.Id != 0)
                    LoadMaterialsAsync();
                ResetSelectedItem();
            });
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                CurrentOptionalView = new OptionsView();

            });
        }
        
        #region MaterialMethods
        private async void LoadMaterialsAsync()
        {
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Visible";
            var materials = await generalService.MaterialService.GetAllAsync();
            Materials = new ObservableCollection<MaterialDTO>(materials);
            DataGridVisibility = "Visible";
            LoadingTextBlockVisibility = "Hidden";
        }
        private void GetAdditionDataForMaterial()
        {
            if (SelectedMaterial?.CategoryName != null)
            {
                SelectedMaterialCategory = MaterialCategories.FirstOrDefault(x => x.Id == SelectedMaterial.CategoryId);
            }
            if (SelectedMaterial?.ManufacturerName != null)
            {
                SelectedMaterialManufacturer = MaterialManufacturers.FirstOrDefault(x => x.Id == SelectedMaterial.ManufacturerId);
            }
        }

        private async void LoadAddDataAsync()
        {
            var materialCategories = await generalService.MaterialCategoryService.GetAllAsync();
            MaterialCategories = new ObservableCollection<MaterialCategoryDTO>(materialCategories);
            var materialManufacturers = await generalService.MaterialManufacturerService.GetAllAsync();
            MaterialManufacturers = new ObservableCollection<MaterialManufacturerDTO>(materialManufacturers);
        }

        private async void SaveMaterial()
        {
            if (SelectedMaterial.Name == null)
            {
                InformationMessage("Material wasn't added. Material's name must be filled!");
                return;
            }
            SelectedMaterial.ManufacturerName = SelectedMaterialManufacturer?.Name;
            SelectedMaterial.ManufacturerId = SelectedMaterialManufacturer?.Id;
            SelectedMaterial.CategoryId = SelectedMaterialCategory?.Id;
            SelectedMaterial.CategoryName = SelectedMaterialCategory?.Name;
            if (SelectedMaterial?.Id != 0)
            {
                var tmpMaterial = SelectedMaterial;
                int index = Materials.IndexOf(SelectedMaterial);
                Materials.Remove(SelectedMaterial);
                await generalService.MaterialService.UpdateAsync(tmpMaterial);
                Materials.Insert(index, tmpMaterial);
                return;
            }
            var materialDto = await generalService.MaterialService.CreateAsync(SelectedMaterial);
            if (Materials == null)
            {
                InformationMessage("Material was added. To see all materials - press \"Get All\" button");
                ResetSelectedItem();
                return;
            }
            Materials.Add(materialDto);
            ResetSelectedItem();
        }
        private void SortMaterials(object param)
        {
            switch (param)
            {
                case "MaterialTitleAsc":
                    Materials = Materials.OrderBy(x => x.Name).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "MaterialTitleDesc":
                    Materials = Materials.OrderByDescending(x => x.Name).ThenBy(x => x.Name).ToObservableCollection();
                    break;
                case "CategoryAsc":
                    Materials = Materials.OrderBy(x => x.CategoryName).ToObservableCollection();
                    break;
                case "CategoryDesc":
                    Materials = Materials.OrderByDescending(x => x.CategoryName).ToObservableCollection();
                    break;
                case "ManufacturerAsc":
                    Materials = Materials.OrderBy(x => x.ManufacturerName).ToObservableCollection();
                    break;
                case "ManufacturerDesc":
                    Materials = Materials.OrderByDescending(x => x.ManufacturerName).ToObservableCollection();
                    break;
            }
        }
        #endregion
        #region MainMethods
        private void ResetSelectedItem()
        {
            SelectedMaterial = null;
            SelectedMaterialCategory = null;
            SelectedMaterialManufacturer = null;
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
        public ICommand GetAllMaterialsCommand { get; private set; }
        public ICommand AddMaterialCommand { get; private set; }
        public ICommand UpdateMaterialCommand { get; private set; }
        public ICommand RemoveMaterialCommand { get; private set; }
        public ICommand SortMaterialsCommand { get; private set; }
        public ICommand SaveMaterialCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }


        #endregion

    }
}
