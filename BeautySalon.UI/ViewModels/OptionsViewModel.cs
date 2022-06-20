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
    public class OptionsViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private string text;
        private GeneralService generalService;
        private string selectedMenuControlVisibility = "visible";
        private string menuOptionButtonsVisibility = "visible";
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
        private UserControl currentMenuView;
        public UserControl CurrentMenuView
        {
            get => currentMenuView;
            set
            {
                currentMenuView = value;
                NotifyOfPropertyChanged();
            }
        }
        public string MenuOptionButtonsVisibility
        {
            get => menuOptionButtonsVisibility;
            set
            {
                menuOptionButtonsVisibility = value;
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
        public string Text
        {
            get => text;
            set
            {
                text = value;
                NotifyOfPropertyChanged();
            }
        }
        #endregion
        public OptionsViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
        }

        private void InitCommands()
        {
            ClientsCommand = new RelayCommand(param =>
            {
                MenuOptionButtonsVisibility = "Hidden";
                CurrentMenuView = new ClientView();
                SelectedMenuControlVisibility = "Visible";
            });
            MastersCommand = new RelayCommand(param =>
            {
                MenuOptionButtonsVisibility = "Hidden";
                CurrentMenuView = new MasterView();
                SelectedMenuControlVisibility = "Visible";
            });
            MaterialsCommand = new RelayCommand(param =>
            {
                MenuOptionButtonsVisibility = "Hidden";
                CurrentMenuView = new MaterialView();
                SelectedMenuControlVisibility = "Visible";
            });
            SettingsCommand = new RelayCommand(param =>
            {
                MenuOptionButtonsVisibility = "Hidden";
                CurrentMenuView = new SettingsView();
                SelectedMenuControlVisibility = "Visible";
            });
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MenuOptionButtonsVisibility = "Hidden";
                SelectedMenuControlVisibility = "Visible";
                CurrentMenuView = new MenuView();

            });
        }
        public ICommand ClientsCommand { get; private set; }
        public ICommand MastersCommand { get; private set; }
        public ICommand MaterialsCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }


    }


}
