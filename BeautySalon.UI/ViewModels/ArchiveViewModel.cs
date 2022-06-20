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
    public class ArchiveViewModel : BaseNotifyPropertyChanged
    {
        #region Properties
        private string mainControlsVisible = "Visible";
        private string datagridVisibility = "hidden";
        private string loadingTextBlockVisibility = "hidden";
        private GeneralService generalService;
        private ObservableCollection<ArchiveDTO> archive;
        private UserControl currentView;
        private DateTime selectedDate;
        public ObservableCollection<ArchiveDTO> Archive
        {
            get => archive;
            set
            {
                archive = value;
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
        #endregion

        public ArchiveViewModel(GeneralService generalService)
        {
            this.generalService = generalService;
            InitCommands();
        }
        private void InitCommands()
        {
            BackMainMenuCommand = new RelayCommand(param =>
            {
                MainControlsVisible = "Hidden";
                DataGridVisibility = "Hidden";
                CurrentView = new MenuView();

            });
            GetAllArchiveCommand = new RelayCommand(async param =>
            {
                await LoadArchiveAsync();
            });
            GetByDateArchiveCommand = new RelayCommand(async param =>
            {
                await LoadArchiveAsync();
                try
                {
                    Archive = Archive.Where(x => x.Date == SelectedDate)?.ToObservableCollection();
                }
                catch
                { }
            });
            GetByMonthArchiveCommand = new RelayCommand(async param =>
            {
                await LoadArchiveAsync();
                try
                {
                    Archive = Archive.Where(x => x.Date.Month == SelectedDate.Month && x.Date.Year == SelectedDate.Year)?.ToObservableCollection();
                }
                catch
                { }
            });
            GetByYearArchiveCommand = new RelayCommand(async param =>
            {
                await LoadArchiveAsync();
                try
                {
                    Archive = Archive.Where(x => x.Date.Year == SelectedDate.Year)?.ToObservableCollection();
                }
                catch
                { }
            });
            SortArchiveCommand = new RelayCommand(param => SortArchive(param));
        }
        private void SortArchive(object param)
        {
            switch (param)
            {
                case "MasterNameAsc":
                    Archive = Archive.OrderBy(x => x.MasterLastname).ThenBy(x => x.MasterName).ToObservableCollection();
                    break;
                case "SortArchiveCommand":
                    Archive = Archive.OrderByDescending(x => x.MasterLastname).ThenBy(x => x.MasterName).ToObservableCollection();
                    break;
                case "DateAsc":
                    Archive = Archive.OrderBy(x => x.Date).ToObservableCollection();
                    break;
                case "DateDesc":
                    Archive = Archive.OrderByDescending(x => x.Date).ToObservableCollection();
                    break;
            }
        }
        private async Task LoadArchiveAsync()
        {
            DataGridVisibility = "Hidden";
            LoadingTextBlockVisibility = "Visible";
            var archive = await generalService.ArchiveService.GetAllAsync();
            Archive = new ObservableCollection<ArchiveDTO>(archive);
            DataGridVisibility = "Visible";
            LoadingTextBlockVisibility = "Hidden";
        }
        #region Commands
        public ICommand GetByDateArchiveCommand { get; private set; }
        public ICommand GetAllArchiveCommand { get; private set; }
        public ICommand GetByMonthArchiveCommand { get; private set; }
        public ICommand GetByYearArchiveCommand { get; private set; }
        public ICommand SortArchiveCommand { get; private set; }
        public ICommand BackMainMenuCommand { get; private set; }
        #endregion
    }

}
