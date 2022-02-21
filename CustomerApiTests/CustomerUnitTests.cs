using CustomerAPI.Controllers;
using CustomerAPI.Models;
using CustomerAPI.Repositories;
using CustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CustomerApiTests
{
    public class CustomerUnitTests
    {

        private CustomerController _controller;
        private IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;

        public CustomerUnitTests()
        {
            _unitOfWork = GetInMemoryRepository();
            _customerService = new CustomerService(_unitOfWork);
        }
        private IUnitOfWork GetInMemoryRepository()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                             .UseInMemoryDatabase(databaseName: "MockDB")
                             .Options;

            var initContext = new ApplicationDbContext(options);

            initContext.Database.EnsureDeleted();

            //Populate Test data
            Populate(initContext);
           
            var repository = new CustomerRepository(initContext);
            var unitOfWork = new UnitOfWork(initContext, repository);
            return unitOfWork;
        }
        private void Populate(ApplicationDbContext context)
        {

            var customers = new Customer[] {
                new Customer{ CustomerId = 1, FirstName = "John", LastName = "Smith", DateOfBirth = DateTime.Now },
                new Customer{ CustomerId = 2, FirstName = "Tom", LastName = "Jose", DateOfBirth = DateTime.Now }};
         
            context.Customers.AddRange(customers);

            context.SaveChanges();
        }

        [Fact]
        public async void Success_GetCustomers()
        {
            var c = new CustomerController(_customerService);
           
            //Act
            var actionResult = await c.GetAllAsync();

            // Assert 
            var okObjectResult = actionResult as OkObjectResult;
            var model = okObjectResult.Value;
            var customers = await _unitOfWork.Customers.GetAllAsync();

            Assert.NotNull(okObjectResult);            
            Assert.NotNull(model);            
            Assert.Equal(2, customers.Count);
        }
        [Fact]
        public async void Success_GetCustomer()
        {
            var c = new CustomerController(_customerService);
            var customerId = 1;

            //Act
            var actionResult = await c.GetAsync(customerId);

            // Assert 
            var okObjectResult = actionResult as OkObjectResult;
            var model = okObjectResult.Value as Customer;

            Assert.NotNull(okObjectResult);
            Assert.NotNull(model);
            Assert.Equal("John", model.FirstName);
        }
        [Fact]
        public async void Get_Customer_Returns_NotFount_For_Non_Existing_Customer()
        {
            var c = new CustomerController(_customerService);
            var nonExistingCustomerId = 10;

            //Act
            var actionResult = await c.GetAsync(nonExistingCustomerId);

            // Assert             
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public async void Success_AddCustomers()
        {
            var c = new CustomerController(_customerService);
            var newCustomer = new Customer
            {
                CustomerId = 3,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.Now
            };
            //Act
            var actionResult = await c.AddAsync(newCustomer);

            // Assert 
            var okResult = actionResult as OkResult;            
            var customers = await _unitOfWork.Customers.GetAllAsync();
            Assert.NotNull(okResult);
            Assert.Equal(3, customers.Count);
        }
        [Fact]
        public async void Add_Customer_Returns_BadRequest_For_Existing_Customer()
        {
            var c = new CustomerController(_customerService);
            var newCustomer = new Customer
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.Now
            };
            //Act
            var actionResult = await c.AddAsync(newCustomer);

            // Assert             
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public async void Add_Customer_Returns_BadRequest_For_Empty_Customer()
        {
            var c = new CustomerController(_customerService);
         
            //Act
            var actionResult = await c.AddAsync(null);

            // Assert             
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public async void Success_Delete_Customer()
        {
            var c = new CustomerController(_customerService);
      
            //Act
            var actionResult = await c.DeleteAsync(1);

            // Assert 
            var okResult = actionResult as OkResult;
            var customers = await _unitOfWork.Customers.GetAllAsync();
            Assert.NotNull(okResult);
            Assert.Single(customers);            
        }
        [Fact]
        public async void Delete_Customer_Returns_NotFount_For_Non_Existing_Customer()
        {
            var c = new CustomerController(_customerService);
            var nonExistingCustomerId = 10;
            //Act
            var actionResult = await c.DeleteAsync(nonExistingCustomerId);

            // Assert             
            Assert.IsType<NotFoundResult>(actionResult);
        }
       [Fact]
        public async void Delete_Customer_Returns_BadRequest_For_Empty_Customer()
        {
            var c = new CustomerController(_customerService);

            //Act
            var actionResult = await c.DeleteAsync(null);

            // Assert             
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public async void Success_Update_Customer()
        {
            var c = new CustomerController(_customerService);
            var customerInfo = new Customer
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Jose",
                DateOfBirth = DateTime.Now
            };
            //Act
            var actionResult = await c.UpdateAsync(customerInfo);

            // Assert 
            var okResult = actionResult as OkResult;
            var customers = await _unitOfWork.Customers.GetAllAsync();
            Assert.NotNull(okResult);
            Assert.Equal(2, customers.Count);
            Assert.Equal("Jose", customers.First().LastName);
        }
        [Fact]
        public async void Update_Customer_Returns_NotFount_For_Non_Existing_Customer()
        {
            var c = new CustomerController(_customerService);
            var newCustomer = new Customer
            {
                CustomerId = 10,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.Now
            };
            //Act
            var actionResult = await c.UpdateAsync(newCustomer);

            // Assert             
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public async void Update_Customer_Returns_BadRequest_For_Empty_Customer()
        {
            var c = new CustomerController(_customerService);

            //Act
            var actionResult = await c.UpdateAsync(null);

            // Assert             
            Assert.IsType<BadRequestResult>(actionResult);
        }
    }
}
