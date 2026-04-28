using NUnit.Framework;
using System.Collections.Generic;
using Ultetes.Dnn.Project_Ultetes_Dnn.Components;
using Ultetes.Dnn.Project_Ultetes_Dnn.Models; // Ellenőrizd, hogy a DTO-k itt vannak-e

namespace Project_Ultetes_Dnn.Tests
{
    [TestFixture]
    public class CalendarCalculatorTests
    {
        private CalendarCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            // Arrange - A kalkulátor példányosítása minden teszt előtt
            _calculator = new CalendarCalculator();
        }

        [Test]
        public void CalculateMonthColors_UresListak_UresSzotarralTerVissza()
        {
            // Arrange
            var products = new List<ProductTypeViewModel>();
            var properties = new List<TypePropertyDTO>();

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void CalculateMonthColors_VetesMarciusban_BeallitjaAzElsoBitet()
        {
            // Arrange
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "1", ProductTypeName = "Paradicsom" }
            };
            var properties = new List<TypePropertyDTO>
            {
                // Március a 3. hónap
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "vetes_3" }
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsTrue(result.ContainsKey("Paradicsom"));
            // Index 2 a március (mert 0-tól indul a tömb), az értéke 1 (Vetés)
            Assert.AreEqual(1, result["Paradicsom"][2]);
        }

        [Test]
        public void CalculateMonthColors_AratasAugusztusban_BeallitjaAMasodikBitet()
        {
            // Arrange
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "2", ProductTypeName = "Paprika" }
            };
            var properties = new List<TypePropertyDTO>
            {
                // Augusztus a 8. hónap
                new TypePropertyDTO { ProductTypeId = "2", PropertyName = "aratas_8" }
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsTrue(result.ContainsKey("Paprika"));
            // Index 7 az augusztus, értéke 2 (Aratás)
            Assert.AreEqual(2, result["Paprika"][7]);
        }

        [Test]
        public void CalculateMonthColors_VetesEsAratasUgyanabbanAHonapban_OsszeadjaABiteket()
        {
            // Arrange
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "3", ProductTypeName = "Retek" }
            };
            var properties = new List<TypePropertyDTO>
            {
                new TypePropertyDTO { ProductTypeId = "3", PropertyName = "vetes_5" },  // Május vetés
                new TypePropertyDTO { ProductTypeId = "3", PropertyName = "aratas_5" }   // Május aratás
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsTrue(result.ContainsKey("Retek"));
            // Index 4 a május, az értéke 1 (vetés) + 2 (aratás) = 3
            Assert.AreEqual(3, result["Retek"][4]);

        }
        [Test]
        public void CalculateMonthColors_TobbKulonbozoTermek_HelyesenKiszamoljaMindet()
        {
            // Arrange: Két különböző termék, különböző vetési időkkel
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "1", ProductTypeName = "Paradicsom" },
                new ProductTypeViewModel { ProductTypeId = "2", ProductTypeName = "Uborka" }
            };
            var properties = new List<TypePropertyDTO>
            {
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "vetes_3" },
                new TypePropertyDTO { ProductTypeId = "2", PropertyName = "aratas_7" }
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.AreEqual(2, result.Count); // Két terméknek kell lennie a szótárban
            Assert.AreEqual(1, result["Paradicsom"][2]); // Március vetés
            Assert.AreEqual(2, result["Uborka"][6]);     // Július aratás
        }

        [Test]
        public void CalculateMonthColors_AzonosTermekTipusKetszer_CsakAzElsotDolgozzaFel()
        {
            // Arrange: Kétszer szerepel a "Sárgarépa", a kódnak a másodikat ki kell hagynia
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "1", ProductTypeName = "Sárgarépa" },
                new ProductTypeViewModel { ProductTypeId = "2", ProductTypeName = "Sárgarépa" }
            };
            var properties = new List<TypePropertyDTO>
            {
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "vetes_4" }
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.AreEqual(1, result.Count); // Bár két elem volt, csak egy maradhat a Dictionary-ben
            Assert.AreEqual(1, result["Sárgarépa"][3]);
        }

        [Test]
        public void CalculateMonthColors_HelytelenPropertyNevFormatozas_NemDobHibatCsakKihagyja()
        {
            // Arrange: Nincs aláhúzás a property névben (hibás adat az adatbázisban)
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "1", ProductTypeName = "Borsó" }
            };
            var properties = new List<TypePropertyDTO>
            {
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "vetes4" } // HIBÁS! Nincs "_"
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsTrue(result.ContainsKey("Borsó"));
            Assert.AreEqual(0, result["Borsó"][3]); // Mivel hibás volt, nem állította be a bitet
        }

        [Test]
        public void CalculateMonthColors_HelytelenHonapFormatum_NemDobHibatCsakKihagyja()
        {
            // Arrange: A hónap száma helyett betű van
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "1", ProductTypeName = "Hagyma" }
            };
            var properties = new List<TypePropertyDTO>
            {
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "vetes_X" } // HIBÁS SZÁM!
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsTrue(result.ContainsKey("Hagyma"));
            // Minden hónapnak üresnek (0) kell lennie, mert az int.TryParse elkapta a hibát
            foreach (var month in result["Hagyma"])
            {
                Assert.AreEqual(0, month);
            }
        }

        [Test]
        public void CalculateMonthColors_IrrelevansTulajdonsagok_NemBefolyasoljakANaptarat()
        {
            // Arrange: Olyan tulajdonság, ami nem vetés vagy aratás (pl. szín, méret)
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "1", ProductTypeName = "Virág" }
            };
            var properties = new List<TypePropertyDTO>
            {
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "szin_piros" },
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "meret_5" }
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsTrue(result.ContainsKey("Virág"));
            Assert.AreEqual(0, result["Virág"][4]); // Az 5. hónap nem lett beállítva, mert nem vetés/aratás volt a szó
        }

        [Test]
        public void CalculateMonthColors_HatarertekDecember_HelyesIndexreKerul()
        {
            // Arrange: Az év legutolsó hónapja (határérték teszt)
            var products = new List<ProductTypeViewModel>
            {
                new ProductTypeViewModel { ProductTypeId = "1", ProductTypeName = "Káposzta" }
            };
            var properties = new List<TypePropertyDTO>
            {
                new TypePropertyDTO { ProductTypeId = "1", PropertyName = "aratas_12" } // 12 = December
            };

            // Act
            var result = _calculator.CalculateMonthColors(products, properties);

            // Assert
            Assert.IsTrue(result.ContainsKey("Káposzta"));
            Assert.AreEqual(2, result["Káposzta"][11]); // 11-es index az utolsó elem a tömbben (december)
        }
    }
}