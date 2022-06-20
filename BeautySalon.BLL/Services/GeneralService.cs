using BeautySalon.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.Services
{
    public class GeneralService
    {
        private InitializationGeneralRepository initializationGeneralRepository;
        private ArchiveService archiveService;
        private BookingService bookingService;
        private ClientService clientService;
        private HallTypeService hallTypeService;
        private MasterService masterService;
        private MasterCategoryService masterCategoryService;
        private MaterialCategoryService materialCategoryService;
        private MaterialManufacturerService materialManufacturerService;
        private MaterialService materialService;
        private PaymentService paymentService;
        private SalaryService salaryService;
        private ServiceService serviceService;
        private ServiceCategoryService serviceCategoryService;
        private VisitService visitService;
        private UserService userService;
        public GeneralService(InitializationGeneralRepository initializationGeneralRepository)
        {
            this.initializationGeneralRepository = initializationGeneralRepository;
        }
        #region publicProperties
        public ArchiveService ArchiveService
        {
            get
            {

                if (this.archiveService == null)
                {
                    this.archiveService = new ArchiveService(initializationGeneralRepository);
                }
                return archiveService;
            }
        }

        public BookingService BookingService
        {
            get
            {

                if (this.bookingService == null)
                {
                    this.bookingService = new BookingService(initializationGeneralRepository);
                }
                return bookingService;
            }
        }
        public ClientService ClientService
        {
            get
            {

                if (this.clientService == null)
                {
                    this.clientService = new ClientService(initializationGeneralRepository);
                }
                return clientService;
            }
        }
        public HallTypeService HallTypeService
        {
            get
            {

                if (this.hallTypeService == null)
                {
                    this.hallTypeService = new HallTypeService(initializationGeneralRepository);
                }
                return hallTypeService;
            }
        }
        public MasterService MasterService
        {
            get
            {

                if (this.masterService == null)
                {
                    this.masterService = new MasterService(initializationGeneralRepository);
                }
                return masterService;
            }
        }
        public MasterCategoryService MasterCategoryService
        {
            get
            {

                if (this.masterCategoryService == null)
                {
                    this.masterCategoryService = new MasterCategoryService(initializationGeneralRepository);
                }
                return masterCategoryService;
            }
        }
        public MaterialCategoryService MaterialCategoryService
        {
            get
            {

                if (this.materialCategoryService == null)
                {
                    this.materialCategoryService = new MaterialCategoryService(initializationGeneralRepository);
                }
                return materialCategoryService;
            }
        }
        public MaterialManufacturerService MaterialManufacturerService
        {
            get
            {

                if (this.materialManufacturerService == null)
                {
                    this.materialManufacturerService = new MaterialManufacturerService(initializationGeneralRepository);
                }
                return materialManufacturerService;
            }
        }
        public MaterialService MaterialService
        {
            get
            {

                if (this.materialService == null)
                {
                    this.materialService = new MaterialService(initializationGeneralRepository);
                }
                return materialService;
            }
        }
        public PaymentService PaymentService
        {
            get
            {

                if (this.paymentService == null)
                {
                    this.paymentService = new PaymentService(initializationGeneralRepository);
                }
                return paymentService;
            }
        }
        public SalaryService SalaryService
        {
            get
            {

                if (this.salaryService == null)
                {
                    this.salaryService = new SalaryService(initializationGeneralRepository);
                }
                return salaryService;
            }
        }
        public ServiceService ServiceService
        {
            get
            {

                if (this.serviceService == null)
                {
                    this.serviceService = new ServiceService(initializationGeneralRepository);
                }
                return serviceService;
            }
        }
        public ServiceCategoryService ServiceCategoryService
        {
            get
            {

                if (this.serviceCategoryService == null)
                {
                    this.serviceCategoryService = new ServiceCategoryService(initializationGeneralRepository);
                }
                return serviceCategoryService;
            }
        }
        public VisitService VisitService
        {
            get
            {

                if (this.visitService == null)
                {
                    this.visitService = new VisitService(initializationGeneralRepository);
                }
                return visitService;
            }
        }
        public UserService UserService
        {
            get
            {

                if (this.userService == null)
                {
                    this.userService = new UserService(initializationGeneralRepository);
                }
                return userService;
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
                    initializationGeneralRepository.Dispose();
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
