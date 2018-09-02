using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.EFDataContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module.Repository.EF.Repositories;
using Module.Repository.EF.RowLevelSecurity;
using Module.Repository.EF.UnitOfWorkInterfaces;
using TrackableEntities;

namespace Module.Repository.EF.IntegrationTests
{
    [TestClass]
    public class RepositoryPatternIntegrationTests
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {

        }

        #region Row Level Security

        [TestMethod]
        public void Can_Fetch_Only_The_Rows_The_User_Is_Allowed_By_Row_Level_Security()
        {
            // TODO: Setup test?

            using (var context = new AppDbContext("SimpleMembership"))
            {
                IUnitOfWorkAsync unitOfWork = new UnitOfWork(context , new RowAuthPoliciesContainer());

                IRowAuthPoliciesContainer container = RowAuthPoliciesContainer.ConfigureRowAuthPolicies();

                container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);

                IRepositoryAsync<Operation> operationRepository = new Repository<Operation>(context , unitOfWork , container);

                var coperationVanilla = operationRepository.Find(1);

                var coperation = operationRepository.FindWithRowLevelSecurity(1);
                var coperations = operationRepository.QueryableWithRowLevelSecurity().ToList();

                Assert.IsTrue(coperations.Count > 0);
                Assert.IsNotNull(coperations);

                TestContext.WriteLine("Operation found: {0}" , coperation.Name);
                TestContext.WriteLine("Operations found: {0}" , coperations.Count);

                // TODO: idempotent teardown
            }
        }

        private string GetMatchingCriteria()
        {
            return "R";
        }

        private bool GetWhenCriteria()
        {
            return true;
        }

        #endregion Row Level Security

        #region Soft Delete

        #endregion Soft Delete

        #region Simple Audit

        #endregion Simple Audit

        #region Audit Trail

        #endregion Audit Trail

        #region Optimistic concurrency

        #endregion Optimistic concurrency

        #region TestRepository Tests

        // Insert 
        [TestMethod]
        public void Insert_Several_Operations()
        {
            // TODO: idempotent teardown
            // TODO: Setup test?

            using (var context = new AppDbContext())
            {
                IUnitOfWorkAsync unitOfWork = new UnitOfWork(context , new RowAuthPoliciesContainer());
                IRowAuthPoliciesContainer container = RowAuthPoliciesContainer.ConfigureRowAuthPolicies();

                container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);
                IRepositoryAsync<Operation> operationRepository = new Repository<Operation>(context , unitOfWork , container);

                var operations = new[]
                {
                    new Operation {Name = "Operation1Test1", Description = "Operation1Test1", TrackingState = TrackingState.Added},
                    new Operation {Name = "Operation2Test2", Description = "Operation2Test2", TrackingState = TrackingState.Added},
                };

                foreach (var operation in operations)
                {
                    try
                    {
                        operationRepository.Insert(operation);
                        unitOfWork.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        ReportError(ex);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        TestContext.WriteLine(ex.Message);
                    }
                }

                var insertedOperation = operationRepository.Query(x => x.Name == "One").Select().FirstOrDefault();
                Assert.IsTrue(insertedOperation?.Name == "One");
                var insertedOperation2 = operationRepository.Query(x => x.Name == "Two").Select().FirstOrDefault();
                Assert.IsTrue(insertedOperation2?.Name == "Two");
            }
        }

        // Insert Graph
        [TestMethod]
        public void Insert_Graph_With_Many_To_Many_Relation()
        {
            // TODO: idempotent teardown

            using (var context = new AppDbContext())
            {
                IUnitOfWorkAsync unitOfWork = new UnitOfWork(context , new RowAuthPoliciesContainer());
                IRowAuthPoliciesContainer container = RowAuthPoliciesContainer.ConfigureRowAuthPolicies();

                container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);
                IRepositoryAsync<Operation> operationRepository =
                    new Repository<Operation>(context , unitOfWork , container);
                IRepositoryAsync<Resource> resourceRepository =
                    new Repository<Resource>(context , unitOfWork , container);

                var testResource = new Resource
                {
                    Name = "TestResource15" ,
                    Description = "TestResource15" ,
                    Active = true ,
                    TrackingState = TrackingState.Added ,
                    Operations = new[]
                    {
                        new Operation
                        {
                            Name = "Test11",
                            Description = "Test11",
                            TrackingState = TrackingState.Added
                        },
                        new Operation
                        {
                            Name = "Test12",
                            Description = "Test12",
                            TrackingState = TrackingState.Added
                        },
                        new Operation
                        {
                            Name = "Test13",
                            Description = "Test13",
                            TrackingState = TrackingState.Added
                        }
                    }
                };

                try
                {
                    // unitOfWork.Repository<Resource>().ApplyChanges(testResource);
                    unitOfWork.Repository<Resource>().Insert(testResource , false);
                    unitOfWork.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    ReportError(ex);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    TestContext.WriteLine(ex.Message);
                }


                var insertedResource = resourceRepository.Query(x => x.Name == "TestResource15").Select().FirstOrDefault();
                Assert.IsTrue(insertedResource?.Name == "TestResource15");
                var singleOrDefault = insertedResource.Operations.SingleOrDefault(op => op.Name == "Test13");
                Assert.IsNotNull(singleOrDefault);
            }
        }

        // This does not seem to work with many to many (link table) relations, because there are no FKs defined in the joined tables
        [TestMethod]
        public void Insert_Graph_With_Many_To_Many_Relation_With_Traversal()
        {
            // TODO: idempotent teardown

            using (var context = new AppDbContext())
            {
                IUnitOfWorkAsync unitOfWork = new UnitOfWork(context , new RowAuthPoliciesContainer());
                IRowAuthPoliciesContainer container = RowAuthPoliciesContainer.ConfigureRowAuthPolicies();

                container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);
                IRepositoryAsync<Operation> operationRepository = new Repository<Operation>(context , unitOfWork , container);
                IRepositoryAsync<Resource> resourceRepository = new Repository<Resource>(context , unitOfWork , container);

                var testResource = new Resource
                {
                    Id = 1000 ,
                    Name = "TestResource15" ,
                    Description = "TestResource15" ,
                    Active = true ,
                    TrackingState = TrackingState.Added ,
                    Operations = new[] {
                        new Operation
                        {
                            Id= 1000,
                            Name = "Test11" ,
                            Description = "Test11" ,
                            TrackingState = TrackingState.Added
                        }
                    ,
                        new Operation
                        {   Id= 1001,
                            Name = "Test12" ,
                            Description = "Test12" ,
                            TrackingState = TrackingState.Added
                        }
                    ,
                        new Operation
                        {   Id= 1002,
                            Name = "Test13" ,
                            Description = "Test13" ,
                            TrackingState = TrackingState.Added
                        }
                    }
                };

                try
                {
                    // unitOfWork.Repository<Resource>().ApplyChanges(testResource);
                    unitOfWork.Repository<Resource>().Insert(testResource , true);
                    unitOfWork.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    ReportError(ex);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    TestContext.WriteLine(ex.Message);
                }
            }

            //  var insertedOperation = operationRepository.Query(x => x.Name == "One").Select().FirstOrDefault();
            //  Assert.IsTrue(insertedOperation?.Name == "One");
        }
        // Delete
        // Delete Many to many

        [TestMethod]
        public void Delete_One_Side_From_Many_To_Many()
        {
            // TODO: idempotent teardown

            using (var context = new AppDbContext())
            {
                IUnitOfWorkAsync unitOfWork = new UnitOfWork(context , new RowAuthPoliciesContainer());
                IRowAuthPoliciesContainer container = RowAuthPoliciesContainer.ConfigureRowAuthPolicies();

                container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);
                IRepositoryAsync<Operation> operationRepository =
                    new Repository<Operation>(context , unitOfWork , container);
                IRepositoryAsync<Resource> resourceRepository =
                    new Repository<Resource>(context , unitOfWork , container);

                var operationToRemove = operationRepository.Queryable().SingleOrDefault(op => op.Name == "Test11");

                var resource = resourceRepository.Queryable().SingleOrDefault(res => res.Name == "TestResource15");

                if (resource.Operations.Contains(operationToRemove))
                {
                    resource.Operations.Remove(operationToRemove);
                }

                try
                {
                    unitOfWork.Repository<Resource>().Update(resource , false);
                    unitOfWork.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    ReportError(ex);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    TestContext.WriteLine(ex.Message);
                }


                var resourceToRemoveFrom = resourceRepository.Query(x => x.Name == "TestResource15").Select().FirstOrDefault();
                Assert.IsTrue(resourceToRemoveFrom?.Name == "TestResource15");

                var removedOperation = resourceToRemoveFrom.Operations.SingleOrDefault(op => op.Name == "Test11");
                Assert.IsNull(removedOperation);

                var operationShouldNotHaveBeenDeleted = operationRepository.Query(x => x.Name == "Test11").Select().FirstOrDefault();
                Assert.IsNotNull(operationShouldNotHaveBeenDeleted);
            }
        }

        [TestMethod]
        public void Add_Additional_Many_To_Many_Relation()
        {
            // TODO: idempotent teardown

            using (var context = new AppDbContext())
            {
                IUnitOfWorkAsync unitOfWork = new UnitOfWork(context , new RowAuthPoliciesContainer());
                IRowAuthPoliciesContainer container = RowAuthPoliciesContainer.ConfigureRowAuthPolicies();

                container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);
                IRepositoryAsync<Operation> operationRepository =
                    new Repository<Operation>(context , unitOfWork , container);
                IRepositoryAsync<Resource> resourceRepository =
                    new Repository<Resource>(context , unitOfWork , container);

                var operationToAdd = operationRepository.Queryable().SingleOrDefault(op => op.Name == "Test11");

                var resource = resourceRepository.Queryable().SingleOrDefault(res => res.Name == "TestResource15");

                if (!resource.Operations.Contains(operationToAdd))
                {
                    resource.Operations.Add(operationToAdd);
                }

                try
                {
                    unitOfWork.Repository<Resource>().Update(resource , false);
                    unitOfWork.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    ReportError(ex);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    TestContext.WriteLine(ex.Message);
                }


                var resourceResult = resourceRepository.Query(x => x.Name == "TestResource15").Select().FirstOrDefault();
                Assert.IsTrue(resourceResult?.Name == "TestResource15");
                var singleOrDefault = resourceResult.Operations.SingleOrDefault(op => op.Name == "Test11");
                Assert.IsNotNull(singleOrDefault);
            }
        }

        // Update
        [TestMethod]
        public void Update_Both_Parent_And_Child_Entity_In_Many_To_Many()
        {
            // TODO: idempotent teardown

            using (var context = new AppDbContext())
            {
                IUnitOfWorkAsync unitOfWork = new UnitOfWork(context , new RowAuthPoliciesContainer());
                IRowAuthPoliciesContainer container = RowAuthPoliciesContainer.ConfigureRowAuthPolicies();

                container.Register<Operation , string>(o => o.Name).When(GetWhenCriteria).Match(GetMatchingCriteria);
                IRepositoryAsync<Operation> operationRepository =
                    new Repository<Operation>(context , unitOfWork , container);
                IRepositoryAsync<Resource> resourceRepository =
                    new Repository<Resource>(context , unitOfWork , container);

                var operationToedit = operationRepository.Queryable().SingleOrDefault(op => op.Name == "Test11");
                operationToedit.Name = "Changed";

                var resource = resourceRepository.Queryable().SingleOrDefault(res => res.Name == "TestResource15");
                resource.Name = "Changed";

                try
                {
                    unitOfWork.Repository<Resource>().Update(resource , false);
                    unitOfWork.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    ReportError(ex);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    TestContext.WriteLine(ex.Message);
                }

                var resourceResult = resourceRepository.Query(x => x.Name == "Changed").Select().FirstOrDefault();
                Assert.IsTrue(resourceResult?.Name == "Changed");
                var singleOrDefault = resourceResult.Operations.SingleOrDefault(op => op.Name == "Changed");
                Assert.IsNotNull(singleOrDefault);
            }
        }

        // Fetch complex graph (multiple levels of associations, join)

        #endregion TestRepository Tests


        #region Helper methods

        private void ReportError( DbEntityValidationException ex )
        {
            var sb = new StringBuilder();

            foreach (var failure in ex.EntityValidationErrors)
            {
                sb.AppendFormat("{0} failed validation\n" , failure.Entry.Entity.GetType());

                foreach (var error in failure.ValidationErrors)
                {
                    sb.AppendFormat("- {0} : {1}" , error.PropertyName , error.ErrorMessage);
                    sb.AppendLine();
                }
            }

            Debug.WriteLine(sb.ToString());
            TestContext.WriteLine(sb.ToString());
        }

        #endregion Helper methods
    }
}
