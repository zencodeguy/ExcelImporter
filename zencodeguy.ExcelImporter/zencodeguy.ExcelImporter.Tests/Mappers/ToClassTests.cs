using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter;
using zencodeguy.ExcelImporter.Mappers;

namespace zencodeguy.ExcelImporter.Tests.Mappers
{
    [TestClass]
    public class ToClassTests
    {
        #region constructor tests
        [TestMethod]
        public void ThrowsExceptionWhenImportDefinitionIsNull()
        {
            try
            {
                var mapper = new ToClass<Object>(null);
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("ImportDefinition", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected ArgumentNullException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        public void CreatesMapper()
        {
            // Arrange
            var id = new ImportDefinition();

            // Act
            var mapper = new ToClass<Object>(id);

            // Assert
            Assert.AreSame(id, mapper.ImportDefinition);
        }
        #endregion

        #region Map exception tests
        [TestMethod]
        public void MapThrowsExceptionWhenImportResultIsNull()
        {
            // Arrange
            var id = new ImportDefinition();
            var mapper = new ToClass<Object>(id);

            // Act
            try
            {
                var result = mapper.Map(null);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("ImportResult", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void MapThrowsExceptionWhenSubstitutionsAreDefinedButNotProvided()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.STRING,
                Substitute = true,
                SubstitutionName = "username"
            });
            var mapper = new ToClass<Object>(id);

            // Act
            try
            {
                var result = mapper.Map(new ImportResult(), null);
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Substitutions", ex.ParamName);
                Assert.AreEqual("Substitutions dictionary must be provided when Destination Properties require them.\r\nParameter name: Substitutions",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected ArgumentNullException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }
    
        [TestMethod]
        public void MapThrowsExceptionWhenSubstitutionsAreNotDefinedButAreProvided()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "AProperty",
                DataType = Enums.DataTypes.STRING
            });
            var mapper = new ToClass<Object>(id);
            var substitutions = new Dictionary<string, string>()
            {
                { "AKey", "AValue" }
            };

            // Act
            try
            {
                var result = mapper.Map(new ImportResult(), null);
                Assert.Fail("Expected ArgumentException, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Substitution values were provided, but ImportDefinition does not " +
                    "contain any Destination properties with substitutions defined.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected ArgumentException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }

        }

        [TestMethod]
        public void MapThrowsExceptionWhenSubstitutionsIsMissingRequiredKeys()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "First",
                DataType = Enums.DataTypes.STRING,
                Substitute = true,
                SubstitutionName = "first"
            });
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "Second",
                DataType = Enums.DataTypes.STRING,
                Substitute = true,
                SubstitutionName = "second"
            });
            var ir = new ImportResult();
            var substitutions = new Dictionary<string, string>()
            {
                { "second", "a value" }
            };

            var mapper = new ToClass<Object>(id);

            // Act
            try
            {
                var result = mapper.Map(ir, substitutions);
                Assert.Fail("Expected ArgumentException, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("The substitutions dictionary is missing entries for the following keys: first",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expect ArgumentException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }
        #endregion

        [TestMethod]
        public void MapperReturnsObject()
        {
            // Arrange
            var id = MapperTestHelpers.CreateImportDefinition();
            var ir = MapperTestHelpers.CreateImportResult();
            var substitutions = MapperTestHelpers.CreateSubstitutions();

            var mapper = new ToClass<ToClassTestClass>(id);

            // Act
            var result = mapper.Map(ir, substitutions);

            // Assert 
            Assert.AreEqual(1, result.Count);
            var item = result[0];

            Assert.IsTrue(item.ABoolean);
            Assert.AreEqual(1, item.AByte);
            Assert.AreEqual('z', item.ACharacter);
            Assert.AreEqual(new DateTime(1929, 06, 12, 11, 30, 15, DateTimeKind.Utc), item.ADateTime);
            Assert.AreEqual(654.321m, item.ADecimal);
            Assert.AreEqual(-123.456d, Math.Round(item.ADouble, 3));
            Assert.AreEqual(Guid.Parse("C506A057-9EAD-422D-BADE-C5E3E1A97F62"), item.AGuid);
            Assert.AreEqual(-32768, item.AnInt16);
            Assert.AreEqual(-2147483648, item.AnInt32);
            Assert.AreEqual(-9223372036854775808, item.AnInt64);
            Assert.AreEqual("This is an imported string.", item.AString);

            Assert.AreEqual(true, item.DestinationBoolean);
            Assert.AreEqual(254, item.DestinationByte);
            Assert.AreEqual('a', item.DestinationChar);
            Assert.AreEqual(new DateTime(2018,01,01,05,00,00, DateTimeKind.Utc), item.DestinationDateTime);
            Assert.AreEqual(123.456m, item.DestinationDecimal);
            Assert.AreEqual(1.23d, Math.Round(item.DestinationDouble, 2));
            Assert.AreEqual(Guid.Parse("4732B277-11B3-440D-9327-1E2E6613746C"), item.DestinationGuid);
            Assert.AreEqual(32767, item.DestinationINT16);
            Assert.AreEqual(2147483647, item.DestinationINT32);
            Assert.AreEqual(9223372036854775807, item.DestinationINT64);
            Assert.AreEqual("This is a string.", item.DestinationString);

            Assert.AreEqual(true, item.DestinationSubstituteBoolean);
            Assert.AreEqual(100, item.DestinationSubstituteByte);
            Assert.AreEqual('G', item.DestinationSubstituteChar);
            Assert.AreEqual(new DateTime(1776,07,04,12,00,00, DateTimeKind.Utc), item.DestinationSubstituteDateTime);
            Assert.AreEqual(987.123m, item.DestinationSubstituteDecimal);
            Assert.AreEqual(852.147d, Math.Round(item.DestinationSubstituteDouble, 3));
            Assert.AreEqual(Guid.Parse("C02070CE-698B-4DC3-8D67-C182A772AE7D"), item.DestinationSubstituteGuid);
            Assert.AreEqual(32000, item.DestinationSubstituteInt16);
            Assert.AreEqual(2147483000, item.DestinationSubstituteInt32);
            Assert.AreEqual(9223372036000000000, item.DestinationSubstituteInt64);
            Assert.AreEqual("This is a substituted string.", item.DestinationSubstituteString);

            Assert.IsTrue(item.DestinationGenerateGuid != Guid.Empty);
            Assert.IsTrue(item.DestinationGenerateDateOnly != DateTime.MinValue);
            Assert.IsTrue(item.DestinationGenerateDateTime != DateTime.MinValue);
        }
    }

    public class ToClassTestClass
    {
        public bool ABoolean { get; set; }
        public byte AByte { get; set; }
        public char ACharacter { get; set; }
        public DateTime ADateTime { get; set; }
        public Decimal ADecimal { get; set; }
        public Double ADouble { get; set; }
        public Guid AGuid { get; set; }
        public Int16 AnInt16 { get; set; }
        public Int32 AnInt32 { get; set; }
        public Int64 AnInt64 { get; set; }
        public string AString { get; set; }

        public bool DestinationBoolean { get; set; }
        public byte DestinationByte { get; set; }
        public char DestinationChar { get; set; }
        public DateTime DestinationDateTime { get; set; }
        public Decimal DestinationDecimal { get; set; }
        public Double DestinationDouble { get; set; }
        public Guid DestinationGuid { get; set; }
        public Int16 DestinationINT16 { get; set; }
        public Int32 DestinationINT32 { get; set; }
        public Int64 DestinationINT64 { get; set; }
        public string DestinationString { get; set; }

        public bool DestinationSubstituteBoolean { get; set; }
        public byte DestinationSubstituteByte { get; set; }
        public char DestinationSubstituteChar { get; set; }
        public DateTime DestinationSubstituteDateTime { get; set; }
        public Decimal DestinationSubstituteDecimal { get; set; }
        public Double DestinationSubstituteDouble { get; set; }
        public Guid DestinationSubstituteGuid { get; set; }
        public Int16 DestinationSubstituteInt16 { get; set; }
        public Int32 DestinationSubstituteInt32 { get; set; }
        public Int64 DestinationSubstituteInt64 { get; set; }
        public string DestinationSubstituteString { get; set; }

        public Guid DestinationGenerateGuid { get; set; }
        public DateTime DestinationGenerateDateOnly { get; set; }
        public DateTime DestinationGenerateDateTime { get; set; }
    }
}
