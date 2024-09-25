using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Enuns;
using MinimalAPI.Domain.ModelViews;
using Test.Helpers;

namespace Test.Request;

[TestClass]
public class AdminRequestTest
{
    [ClassInitialize]

    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [TestMethod]
    public async Task TestGetSetLoginAdmin()
    {
        //Arrange
        var loginDTO = new LoginDTO()
        {
            Email = "adm@test.com",
            Password = "12345678"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

        //Act
        var response = await Setup.client.PostAsync("/login", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdminLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado?.Email ?? "");
        Assert.IsNotNull(admLogado?.Profile ?? "");
        Assert.IsNotNull(admLogado?.Token ?? "");
    }

    [TestMethod]
    public async Task TestGetAllAdmins()
    {
        // Arrange
        var loginDTO = new LoginDTO()
        {
            Email = "adm@test.com",
            Password = "12345678"
        };

        var loginContent = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

        // Act: Login to get the token
        var loginResponse = await Setup.client.PostAsync("/login", loginContent);
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResult = await loginResponse.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdminLogado>(loginResult, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado?.Token);

        Setup.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", admLogado.Token);

        var response = await Setup.client.GetAsync("/Administradores");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task TestGetAdminById()
    {
        // Arrange
        int adminId = 1;

        // Act
        var response = await Setup.client.GetAsync($"/administradores/{adminId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admin = JsonSerializer.Deserialize<AdminModelView>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admin);
        Assert.AreEqual(adminId, admin.Id);
    }

    [TestMethod]
    public async Task TestPostAddAdmin()
    {
        // Arrange
        var newAdmin = new AdminDTO
        {
            Email = "newadmin@test.com",
            Password = "12345678",
            Profile = (Profile?)Enum.Parse(typeof(Profile), "Adm")
        };

        var content = new StringContent(JsonSerializer.Serialize(newAdmin), Encoding.UTF8, "application/json");

        // Act
        var response = await Setup.client.PostAsync("/Administradores", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var addedAdmin = JsonSerializer.Deserialize<AdminModelView>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(addedAdmin);
        Assert.AreEqual(newAdmin.Email, addedAdmin.Email);
    }
}