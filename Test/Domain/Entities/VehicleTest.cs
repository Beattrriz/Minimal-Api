using MinimalAPI.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class VehicleTest
{
    [TestMethod]
    public void TestGetSetProperties()
    {
        //Arrange
        var adm = new Vehicle();

        //Act
        adm.Id = 1;
        adm.Name = "Uno";
        adm.Mark = "Fiat";
        adm.Year = 2024;

        //Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("Uno", adm.Name);
        Assert.AreEqual("Fiat", adm.Mark);
        Assert.AreEqual(2024, adm.Year);
    }
}