using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;

namespace MvcSuperShop.Tests.Services
{
    [TestClass]
    public class PricingServiceTests : BaseTest
    {
        private PricingService _sut;

        [TestInitialize]
        public void Initialize()
        {
           
            _sut = new PricingService();
        }

        [TestMethod]
        public void When_no_agreement_exists_product_baseprice_is_used()
        {
            //ARRANGE
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel { BasePrice = 98500 }
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>()

            };

            //ACT
            var products = _sut.CalculatePrices(productList, customerContext);


            //ASSERT
            Assert.AreEqual(98500, products.First().Price);
        }

        [TestMethod]
        public void If_multiple_agreements_exists_highest_discount_is_used()
        {
            //ARRANGE
            

            var customerContext = CreateCurrentCustomerContext();
            var productList = CreateTestProductList();
            

            //ACT
            var discountAppliedCars = _sut.CalculatePrices(productList, customerContext);
            var discount1 = discountAppliedCars.FirstOrDefault(e=>e.Id == 1);
            var discount2 = discountAppliedCars.FirstOrDefault(e => e.Id == 2);


            //ASSERT
            Assert.AreEqual(90000,discount1.Price);
            Assert.AreEqual(90000,discount2.Price);
        }

        
        [TestMethod]
        public void AgreementIsValid_returns_false_if_agreemen_is_invalid()
        {
            //ARRANGE
            var customerContext = CreateCurrentCustomerContext();
            var agreement = customerContext.Agreements.FirstOrDefault(e => e.Id == 1);
            //ACT
            var result = _sut.AgreementIsValid(agreement);

            //ASSERT
            Assert.IsFalse(result);
        }
        

        [TestMethod]
        public void AgreementIsValid_returns_true_if_agreement_is_valid()
        {
            //ARRANGE
            var customerContext = CreateCurrentCustomerContext();
            var agreement = customerContext.Agreements.FirstOrDefault(e => e.Id == 2);
            //ACT
            var result = _sut.AgreementIsValid(agreement);

            //ASSERT
            Assert.IsTrue(result);
        }
        


        public CurrentCustomerContext CreateCurrentCustomerContext()
        {
            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>
                {
                    new()
                    {
                        Id = 1,
                        ValidTo = DateTime.Today.AddDays(10),
                        ValidFrom = DateTime.Today.AddDays(2),
                        AgreementRows = new List<AgreementRow>
                        {


                            new()
                            {
                                Id = 1,
                                CategoryMatch = "van", 
                                PercentageDiscount = 5
                            },
                            new()
                            {
                                Id = 2,
                                CategoryMatch = "sedan", 
                                PercentageDiscount = 10,
                                ProductMatch = "hybrid"

                            },
                            new()
                            {
                            Id = 2,
                            CategoryMatch = "sedan",
                            PercentageDiscount = 10,
                            ProductMatch = "hybrid"

                        }
                        }

                    },
                    new()
                    {
                        Id = 2,
                        ValidTo = DateTime.Today.AddDays(15),
                        ValidFrom = DateTime.Today.AddDays(-5),
                        AgreementRows = new List<AgreementRow>
                        {
                            new()
                            {
                                Id = 3,
                                CategoryMatch = "van",
                                PercentageDiscount = 10,
                                ProductMatch = "hybrid",
                            },
                            new()
                            {
                                Id = 4,
                                CategoryMatch = "sedan",
                                PercentageDiscount = 5,
                                ProductMatch = "gasoline"

                            },
                            new()
                            {
                            Id = 4,
                            CategoryMatch = "sedan",
                            PercentageDiscount = 5,
                            ProductMatch = "gasoline"

                        }
                        }
                    }
                }
            };
            return customerContext;
        }

        public List<ProductServiceModel> CreateTestProductList()
        {
            var productList = new List<ProductServiceModel>
            {
                new()
                {
                    Id = 1,
                    BasePrice = 100000, 
                    CategoryName = "van",
                    Name = "transit",
                    ManufacturerName = "ford"

                },
                new()
                {
                    Id = 2,
                    BasePrice = 100000, 
                    CategoryName = "sedan",
                    Name = "amazon",
                    ManufacturerName = "volvo"
                    
                },
                
            };

            return productList;
        }
    }
}
