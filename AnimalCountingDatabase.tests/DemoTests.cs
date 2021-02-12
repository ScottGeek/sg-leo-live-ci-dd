using AnimalCountingDatabase.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AnimalCountingDatabase.tests
{
    public class DemoTests
    {
        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }

        [Fact]
        public async Task CustomerIntegratonTest()
        {
            //Create DB Context
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnecton"]);

            var context = new CustomerContext(optionsBuilder.Options);

            //  Delete All in DB
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            //Create Controller

            var controller = new CustomersController(context);

            //Add customer

            await controller.Add(new Customer { CustomerName = "Foo Barring" });

            // Check Does GetAll work

            var result = (await controller.GetAll()).ToArray();

            Assert.Single(result);
            Assert.Equal("Foo Barring", result[0].CustomerName);
      

        }
    }
}
