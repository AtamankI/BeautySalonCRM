using BeautySalon.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.DAL.Repositories
{
    public class InitializationGeneralRepository
    {
        #region privateProperties
        private SalonContext context = new SalonContext();
        private GeneralRepository<Archive> archiveRepository;
        private GeneralRepository<Booking> bookingRepository;
        private GeneralRepository<Client> clientRepository;
        private GeneralRepository<HallType> hallTypeRepository;
        private GeneralRepository<Master> masterRepository;
        private GeneralRepository<MasterCategory> masterCategoryRepository;
        private GeneralRepository<Material> materialRepository;
        private GeneralRepository<MaterialCategory> materialCategoryRepository;
        private GeneralRepository<MaterialManufacturer> materialManufacturerRepository;
        private GeneralRepository<PaymentType> paymentTypeRepository;
        private GeneralRepository<SalaryType> salaryTypeRepository;
        private GeneralRepository<Service> serviceRepository;
        private GeneralRepository<ServiceCategory> serviceCategoryRepository;
        private GeneralRepository<Visit> visitRepository;
        private GeneralRepository<User> userRepository;

        #endregion

        #region publicProperties
        public GeneralRepository<Archive> ArchiveRepository
        {
            get
            {

                if (this.archiveRepository == null)
                {
                    this.archiveRepository = new GeneralRepository<Archive>(context);
                }
                return archiveRepository;
            }
        }
        public GeneralRepository<Booking> BookingRepository
        {
            get
            {

                if (this.bookingRepository == null)
                {
                    this.bookingRepository = new GeneralRepository<Booking>(context);
                }
                return bookingRepository;
            }
        }
        public GeneralRepository<Client> ClientRepository
        {
            get
            {

                if (this.clientRepository == null)
                {
                    this.clientRepository = new GeneralRepository<Client>(context);
                }
                return clientRepository;
            }
        }
        public GeneralRepository<HallType> HallTypeRepository
        {
            get
            {

                if (this.hallTypeRepository == null)
                {
                    this.hallTypeRepository = new GeneralRepository<HallType>(context);
                }
                return hallTypeRepository;
            }
        }
        public GeneralRepository<Master> MasterRepository
        {
            get
            {

                if (this.masterRepository == null)
                {
                    this.masterRepository = new GeneralRepository<Master>(context);
                }
                return masterRepository;
            }
        }
        public GeneralRepository<MasterCategory> MasterCategoryRepository
        {
            get
            {

                if (this.masterCategoryRepository == null)
                {
                    this.masterCategoryRepository = new GeneralRepository<MasterCategory>(context);
                }
                return masterCategoryRepository;
            }
        }
        public GeneralRepository<Material> MaterialRepository
        {
            get
            {

                if (this.materialRepository == null)
                {
                    this.materialRepository = new GeneralRepository<Material>(context);
                }
                return materialRepository;
            }
        }
        public GeneralRepository<MaterialCategory> MaterialCategoryRepository
        {
            get
            {

                if (this.materialCategoryRepository == null)
                {
                    this.materialCategoryRepository = new GeneralRepository<MaterialCategory>(context);
                }
                return materialCategoryRepository;
            }
        }
        public GeneralRepository<MaterialManufacturer> MaterialManufacturerRepository
        {
            get
            {

                if (this.materialManufacturerRepository == null)
                {
                    this.materialManufacturerRepository = new GeneralRepository<MaterialManufacturer>(context);
                }
                return materialManufacturerRepository;
            }
        }
        public GeneralRepository<PaymentType> PaymentTypeRepository
        {
            get
            {

                if (this.paymentTypeRepository == null)
                {
                    this.paymentTypeRepository = new GeneralRepository<PaymentType>(context);
                }
                return paymentTypeRepository;
            }
        }
        public GeneralRepository<SalaryType> SalaryTypeRepository
        {
            get
            {

                if (this.salaryTypeRepository == null)
                {
                    this.salaryTypeRepository = new GeneralRepository<SalaryType>(context);
                }
                return salaryTypeRepository;
            }
        }
        public GeneralRepository<Service> ServiceRepository
        {
            get
            {

                if (this.serviceRepository == null)
                {
                    this.serviceRepository = new GeneralRepository<Service>(context);
                }
                return serviceRepository;
            }
        }
        public GeneralRepository<ServiceCategory> ServiceCategoryRepository
        {
            get
            {

                if (this.serviceCategoryRepository == null)
                {
                    this.serviceCategoryRepository = new GeneralRepository<ServiceCategory>(context);
                }
                return serviceCategoryRepository;
            }
        }
        public GeneralRepository<Visit> VisitRepository
        {
            get
            {

                if (this.visitRepository == null)
                {
                    this.visitRepository = new GeneralRepository<Visit>(context);
                }
                return visitRepository;
            }
        }
        public GeneralRepository<User> UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new GeneralRepository<User>(context);
                }
                return userRepository;
            }
        }

        #endregion


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
