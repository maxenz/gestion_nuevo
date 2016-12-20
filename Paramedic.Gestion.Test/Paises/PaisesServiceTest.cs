using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using Paramedic.Gestion.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Test.Paises
{

    [TestClass]
    public class PaisesServiceTest
    {
        private Mock<IPaisRepository> _mockRepository;
        private IPaisService _service;
        Mock<IUnitOfWork> _mockUnitWork;
        List<Pais> listCountry;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IPaisRepository>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _service = new PaisService(_mockUnitWork.Object, _mockRepository.Object);
            listCountry = new List<Pais>() {
           new Pais() { Id = 1, Codigo = "US" },
           new Pais() { Id = 2, Codigo = "India" },
           new Pais() { Id = 3, Codigo = "Russia" }
          };
        }

        [TestMethod]
        public void Paises_Get_All()
        {
            //Arrange
            _mockRepository.Setup(x => x.GetAll()).Returns(listCountry);

            //Act
            List<Pais> results = _service.GetAll() as List<Pais>;

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
        }


        [TestMethod]
        public void Can_Add_Pais()
        {
            //Arrange
            int Id = 1;
            Pais emp = new Pais() { Codigo = "UK" };
            _mockRepository.Setup(m => m.Add(emp)).Returns((Pais e) =>
            {
                e.Id = Id;
                return e;
            });


            //Act
            _service.Create(emp);

            //Assert
            Assert.AreEqual(Id, emp.Id);
            _mockUnitWork.Verify(m => m.Commit(), Times.Once);
        }


    }
}
