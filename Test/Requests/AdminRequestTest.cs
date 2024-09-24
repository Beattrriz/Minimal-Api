using MinimalAPI.Domain.Entities;

namespace Test.Request;

[TestClass]
public class AdminRequestTest
{
    [TestMethod]
    public void TestGetSetProperties()
    {
        //Arrange
        var adm = new Admin();

        //Act
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Password = "teste";
        adm.Profile = "Adm";

        //Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("teste@teste.com", adm.Email);
        Assert.AreEqual("teste", adm.Password);
        Assert.AreEqual("Adm", adm.Profile);
    }
}