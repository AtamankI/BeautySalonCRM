using BeautySalon.BLL.Modules;
using BeautySalon.UI.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.UI.Infrastructure
{
    public class ViewModelLocator
    {
        private IKernel container;
        public ViewModelLocator()
        {
            container = new StandardKernel(new SalonModule());

        }
        public MainViewModel MainViewModel => container.Get<MainViewModel>();
        public BookingViewModel BookingViewModel => container.Get<BookingViewModel>();
        public FinishBookingModel FinishBookingModel => container.Get<FinishBookingModel>();
        public ClientViewModel ClientViewModel => container.Get<ClientViewModel>();
        public MasterViewModel MasterViewModel => container.Get<MasterViewModel>();
        public MaterialViewModel MaterialViewModel => container.Get<MaterialViewModel>();
        public ServiceViewModel ServiceViewModel => container.Get<ServiceViewModel>();
        public VisitViewModel VisitViewModel => container.Get<VisitViewModel>();
        public MenuViewModel MenuViewModel => container.Get<MenuViewModel>();
        public OptionsViewModel OptionsViewModel => container.Get<OptionsViewModel>();
        public SettingsViewModel SettingsViewModel => container.Get<SettingsViewModel>();
        public ArchiveViewModel ArchiveViewModel => container.Get<ArchiveViewModel>();

    }
}
