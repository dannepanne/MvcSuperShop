using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcSuperShop.Controllers;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Infrastructure.Profiles;
using MvcSuperShop.Services;
using MvcSuperShop.ViewModels;

namespace MvcSuperShop.Tests.Controllers;

[TestClass]
public class HomeControllerTests : BaseControllerTest
{
    private IMapper _mapper;
    private Mock<ICategoryService> categoryServiceMock;
    private ApplicationDbContext context;
    private Mock<IMapper> mapperMock;
    private Mock<IProductService> productServiceMock;
    private HomeController sut;


    [TestInitialize]
    public void Initialize()
    {
        categoryServiceMock = new Mock<ICategoryService>();
        productServiceMock = new Mock<IProductService>();
        //mapperMock = new Mock<IMapper>();

        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("testdb")
            .Options;

        context = new ApplicationDbContext(contextOptions);
        context.Database.EnsureCreated();

        var config = new MapperConfiguration(c =>
        {
            c.AddProfile<CategoryProfile>();
            c.AddProfile<ProductProfile>();
        });
        var mapper = config.CreateMapper();

        _mapper = mapper;

        sut = new HomeController(categoryServiceMock.Object,
            productServiceMock.Object,
            mapper,
            context
        );
    }

    [TestMethod]
    public void Index_should_show_3_categories()
    {
        //ARRANGE
        sut.ControllerContext = SetupControllerContext();

        categoryServiceMock.Setup(e => e.GetTrendingCategories(3)).Returns(new List<Category>
        {
            new(),
            new(),
            new()
        });

       

        //ACT
        var result = sut.Index() as ViewResult;

        var model = result.Model as HomeIndexViewModel;
        //ASSERT
        Assert.AreEqual(3, model.TrendingCategories.Count);
    }


    [TestMethod]
    public void Index_should_return_correct_view()
    {
        //ARRANGE
        sut.ControllerContext = SetupControllerContext();
        //ACT
        var result = sut.Index() as ViewResult;
        var viewName = result.ViewName;
        //ASSERT
        Assert.IsTrue(string.IsNullOrEmpty(viewName) || viewName == "Index");
    }

    [TestMethod]
    public void When_índex_is_called_should_return_10_products()
    {
        //ARRANGE
        sut.ControllerContext = SetupControllerContext();

        var customerContext = fixture.Create<CurrentCustomerContext>();

        productServiceMock.Setup(e => e.GetNewProducts(10, customerContext)).Returns(new List<ProductServiceModel>
        {
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>(),
            fixture.Create<ProductServiceModel>()
        }.ToList());

        //ACT

        var result = sut.Index() as ViewResult;
        var model = result.Model as HomeIndexViewModel;

        var products = productServiceMock.Object.GetNewProducts(10, customerContext);

        model.NewProducts = _mapper.Map<List<ProductBoxViewModel>>(products);


        //ASSERT
        Assert.AreEqual(10, model.NewProducts.Count);
    }
}