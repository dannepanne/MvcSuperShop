using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
namespace MvcSuperShop.IntergrationTests;

[TestClass]
public class ImageTests
{
    private ApplicationDbContext _context;

    [TestInitialize]
    public void Initialize()
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost;Database=StefanSuperShop;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        _context = new ApplicationDbContext(contextOptions);

    }



    [TestMethod]
    public void All_product_image_urls_returns_status_code_ok()
    {

        var products = _context.Products;

        foreach (var prod in products)
        {

                HttpClient client = new HttpClient();

                var response = client.GetAsync(prod.ImageUrl).Result;

                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

        }

    }

}