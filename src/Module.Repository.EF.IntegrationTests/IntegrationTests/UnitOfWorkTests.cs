using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.UnitOfWorkInterfaces;
using TrackableEntities;

namespace Module.Repository.EF.IntegrationTests.IntegrationTests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        public TestContext TestContext { get; set; }

        //[TestMethod]
        //public void UnitOfWork_Transaction_Test()
        //{
        //    using(var context = new NorthwindContext())
        //    {
        //        IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
        //        IRepositoryAsync<Customer> customerRepository = new Repository<Customer>(context, unitOfWork);
        //        IService<Customer> customerService = new CustomerService(customerRepository);

        //        try
        //        {
        //            unitOfWork.BeginTransaction();

        //            customerService.Insert(new Customer
        //            {
        //                CustomerID = "YODA",
        //                CompanyName = "SkyRanch",
        //                TrackingState = TrackingState.Added
        //            });
        //            customerService.Insert(new Customer
        //            {
        //                CustomerID = "JEDI",
        //                CompanyName = "SkyRanch",
        //                TrackingState = TrackingState.Added
        //            });

        //            var customer = customerService.Find("YODA");
        //            Assert.AreSame(customer.CustomerID, "YODA");

        //            customer = customerService.Find("JEDI");
        //            Assert.AreSame(customer.CustomerID, "JEDI");

        //            // save
        //            var saveChangesAsync = unitOfWork.SaveChanges();
        //            //Assert.AreSame(saveChangesAsync, 2);

        //            // Will cause an exception, cannot insert customer with the same CustomerId (primary key constraint)
        //            customerService.Insert(new Customer
        //            {
        //                CustomerID = "JEDI",
        //                CompanyName = "SkyRanch",
        //                TrackingState = TrackingState.Added
        //            });
        //            //save 
        //            unitOfWork.SaveChanges();

        //            unitOfWork.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            unitOfWork.Rollback();
        //        }
        //    }
        //}
    }
}