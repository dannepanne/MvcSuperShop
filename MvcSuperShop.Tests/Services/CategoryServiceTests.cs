using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcSuperShop.Data;
using MvcSuperShop.Services;

namespace MvcSuperShop.Tests.Services;
[TestClass]
public class CategoryServiceTests : BaseTest
{
    private CategoryService _sut;
    private ApplicationDbContext _context;

    [TestInitialize]
    public void Initialize()
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("testdb")
            .Options;

        _context = new ApplicationDbContext(contextOptions);
        _context.Database.EnsureCreated();

        _sut = new CategoryService(_context);

    }


    [TestMethod]
    public void GetTrendingCategories_should_always_return_amount_of_categories_sent_into_function()
    {
        //ARRANGE
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.SaveChanges();
        //ACT
        var categories = _sut.GetTrendingCategories(4).ToList();
        var result = categories.Count();
        //ASSERT
        Assert.AreEqual(4, result);
    }

    [TestMethod]
    public void GetTrendingCategories_should_return_all_existing_categories_if_higher_than_that_is_sent_into_function()
    {
        //ARRANGE
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.Categories.Add(fixture.Create<Category>());
        _context.SaveChanges();
        //ACT
        var categories = _sut.GetTrendingCategories(7).ToList();
        var result = categories.Count();
        //ASSERT
        Assert.AreEqual(5, result);
    }

}    