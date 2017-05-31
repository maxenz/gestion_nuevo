using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Paramedic.Gestion.Service;
using System.Collections.Generic;
using Paramedic.Gestion.Model;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Web.Controllers;

namespace Paramedic.Gestion.Test
{
    [TestClass]
    public class PaisesControllerTest
    {
        private Mock<IPaisService> _paisServiceMock;
        PaisesController objController;
        List<Pais> listPaises;

        [TestInitialize]
        public void Initialize()
        {
            _paisServiceMock = new Mock<IPaisService>();
            objController = new PaisesController(_paisServiceMock.Object);
            listPaises = new List<Pais>()
            {
                new Pais() { Id = 1, Descripcion = "ESTADOS UNIDOS", Codigo = "USA" },
                new Pais() { Id = 2, Descripcion = "INDIA", Codigo = "IND" },
                new Pais() { Id = 3, Descripcion = "ARGELIA", Codigo = "ARG" }
            };
        }

        [TestMethod]
        public void Paises_Get_All()
        {
            //Arrange
            _paisServiceMock.Setup(x => x.GetAll()).Returns(listPaises);

            //Act
            var result = ((objController.Index("",1) as ViewResult).Model) as IPagedList<Pais>;

            //Assert
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual("ARGELIA", result[0].Descripcion);

        }

        [TestMethod]
        public void Valid_Pais_Create()
        {
            //Arrange
            Pais p = new Pais() { Id = 1, Codigo = "ARG", Descripcion =  "ARGENTINA" };

            //Act
            var result = (RedirectToRouteResult)objController.Create(p);

            //Assert
            _paisServiceMock.Verify(m => m.Create(p), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [TestMethod]
        public void Invalid_Country_Create()
        {
            //Arrange 
            Pais p = new Pais() { Descripcion = "" };
            objController.ModelState.AddModelError("Error", "Algo anduvo mal");

            //Act
            var result = (ViewResult)objController.Create(p);

            //Assert
            _paisServiceMock.Verify(m => m.Create(p), Times.Never);
            Assert.AreEqual("", result.ViewName);
        }

    }
}
