using BeautySalon.BLL.Services;
using BeautySalon.DAL.Context;
using BeautySalon.DAL.Repositories;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.Modules
{
    public class SalonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<GeneralRepository<Archive>>().To<GeneralRepository<Archive>>();
            Bind<GeneralRepository<Booking>>().To<GeneralRepository<Booking>>();
            Bind<GeneralRepository<Client>>().To<GeneralRepository<Client>>();
            Bind<GeneralRepository<HallType>>().To<GeneralRepository<HallType>>();
            Bind<GeneralRepository<MasterCategory>>().To<GeneralRepository<MasterCategory>>();
            Bind<GeneralRepository<Master>>().To<GeneralRepository<Master>>();
            Bind<GeneralRepository<Material>>().To<GeneralRepository<Material>>();
            Bind<GeneralRepository<MaterialCategory>>().To<GeneralRepository<MaterialCategory>>();
            Bind<GeneralRepository<MaterialManufacturer>>().To<GeneralRepository<MaterialManufacturer>>();
            Bind<GeneralRepository<PaymentType>>().To<GeneralRepository<PaymentType>>();
            Bind<GeneralRepository<SalaryType>>().To<GeneralRepository<SalaryType>>();
            Bind<GeneralRepository<Service>>().To<GeneralRepository<Service>>();
            Bind<GeneralRepository<ServiceCategory>>().To<GeneralRepository<ServiceCategory>>();
            Bind<GeneralRepository<Visit>>().To<GeneralRepository<Visit>>();

            Bind<ArchiveService>().To<ArchiveService>();
            Bind<BookingService>().To<BookingService>();
            Bind<ClientService>().To<ClientService>();
            Bind<HallTypeService>().To<HallTypeService>();
            Bind<MasterCategoryService>().To<MasterCategoryService>();
            Bind<MasterService>().To<MasterService>();
            Bind<MaterialService>().To<MaterialService>();
            Bind<MaterialCategoryService>().To<MaterialCategoryService>();
            Bind<MaterialManufacturerService>().To<MaterialManufacturerService>();
            Bind<PaymentService>().To<PaymentService>();
            Bind<SalaryService>().To<SalaryService>();
            Bind<ServiceService>().To<ServiceService>();
            Bind<ServiceCategoryService>().To<ServiceCategoryService>();
            Bind<VisitService>().To<VisitService>();

            Bind<DbContext>().To<SalonContext>();
        }

    }
}
