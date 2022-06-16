using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcSuperShop.Data;

namespace MvcSuperShop.Tests.Controllers;

public class BaseControllerTest : BaseTest
{
    protected ControllerContext SetupControllerContext()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "test@test.se")
        }, "TestAuthentication"));


        var controllerContext = new ControllerContext();
        controllerContext.HttpContext = new DefaultHttpContext()
        {
            User = user
        };

        return controllerContext;
    }

}