using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Mappers;

namespace zencodeguy.ExcelImporter.Tests.Mappers
{
    [TestClass]
    public class ToDataSetTests
    {
        #region Constructor Tests
        [TestMethod]
        public void ConstructorThrowsExceptionWhenImportDefinitionIsNull()
        {
            try
            {
                var mapper = new ToDataSet(null);
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("ImportDefinition", ex.ParamName);
            }
            catch(ArgumentException ex)
            {
                Assert.Fail("Expected ArgumentNullException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ConstructorReturnsMapper()
        {
            // Arrange
            var id = new ImportDefinition();

            // Act
            var mapper = new ToDataSet(id);

            // Assert
            Assert.AreSame(id, mapper.ImportDefinition);
        }
        #endregion

        #region Map
        [TestMethod]
        public void MapThrowsExceptionWhenImportResultIsNull()
        {
            // Arrange
            var id = new ImportDefinition();
            var mapper = new ToDataSet(id);

            // Act
            try
            {
                var dt = mapper.Map(null);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("ImportResult", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentNullException exxpected, " +
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
            var mapper = new ToDataSet(id);

            // Act
            try
            {
                mapper.Map(new ImportResult(), null);
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
            var mapper = new ToDataSet(id);
            var substitutions = new Dictionary<string, string>()
            {
                { "AKey", "AValue" }
            };

            // Act
            try
            {
                var dt = mapper.Map(new ImportResult(), null);
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

            var mapper = new ToDataSet(id);

            // Act
            try
            {
                mapper.Map(ir, substitutions);
                Assert.Fail("Expected ArgumentException, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("The substitutions dictionary is missing entries for the following keys: first",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expect ArgumentException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void MapReturnsDataSet()
        {
            // Arrange
            var id = MapperTestHelpers.CreateImportDefinition();
            var ir = MapperTestHelpers.CreateImportResult();
            var substitutions = MapperTestHelpers.CreateSubstitutions();

            var mapper = new ToDataSet(id);

            // Act
            var result = mapper.Map(ir, substitutions);

            // Assert
            Assert.AreEqual(1, result.Rows.Count);
            var dr = result.Rows[0];
            Assert.AreEqual(dr["ABoolean"], true);
            Assert.AreEqual(dr["ABoolean"].GetType(), typeof(bool));
            Assert.AreEqual((byte)dr["AByte"], 1);
            Assert.AreEqual(dr["AByte"].GetType(), typeof(byte));
            Assert.AreEqual((char)dr["ACharacter"], 'z');
            Assert.AreEqual(dr["ACharacter"].GetType(), typeof(char));
            Assert.AreEqual((DateTime)dr["ADateTime"], new DateTime(1929, 06, 12, 11, 30, 15, DateTimeKind.Utc));
            Assert.AreEqual(dr["ADateTime"].GetType(), typeof(DateTime));
            Assert.AreEqual((Decimal)dr["ADecimal"], 654.321m);
            Assert.AreEqual(dr["ADecimal"].GetType(), typeof(decimal));
            Assert.AreEqual(Math.Round((double)dr["ADouble"], 3), Math.Round(-123.456f, 3));
            Assert.AreEqual(dr["ADouble"].GetType(), typeof(double));
            Assert.AreEqual((Guid)dr["AGuid"], Guid.Parse("C506A057-9EAD-422D-BADE-C5E3E1A97F62"));
            Assert.AreEqual(dr["AGuid"].GetType(), typeof(Guid));
            Assert.AreEqual((Int16)dr["AnInt16"], -32768);
            Assert.AreEqual(dr["AnInt16"].GetType(), typeof(Int16));
            Assert.AreEqual((Int32)dr["AnInt32"], -2147483648);
            Assert.AreEqual(dr["AnInt32"].GetType(), typeof(Int32));
            Assert.AreEqual((Int64)dr["AnInt64"], -9223372036854775808);
            Assert.AreEqual(dr["AnInt64"].GetType(), typeof(Int64));
            Assert.AreEqual((string)dr["AString"], "This is an imported string.");
            Assert.AreEqual(dr["AString"].GetType(), typeof(string));

            Assert.AreEqual((bool)dr["DestinationBoolean"], true);
            Assert.AreEqual(dr["DestinationBoolean"].GetType(), typeof(bool));
            Assert.AreEqual((byte)dr["DestinationByte"], 254);
            Assert.AreEqual(dr["DestinationByte"].GetType(), typeof(byte));
            Assert.AreEqual((char)dr["DestinationChar"], 'a');
            Assert.AreEqual(dr["DestinationChar"].GetType(), typeof(char));
            Assert.AreEqual((DateTime)dr["DestinationDateTime"], new DateTime(2018, 01, 01, 05, 00, 00, DateTimeKind.Utc));
            Assert.AreEqual(dr["DestinationDateTime"].GetType(), typeof(DateTime));
            Assert.AreEqual((Decimal)dr["DestinationDecimal"], 123.456m);
            Assert.AreEqual(dr["DestinationDecimal"].GetType(), typeof(Decimal));
            Assert.AreEqual(Math.Round((double)dr["DestinationDouble"], 2), Math.Round(1.23f, 2));
            Assert.AreEqual(dr["DestinationDouble"].GetType(), typeof(Double));
            Assert.AreEqual((Guid)dr["DestinationGuid"], Guid.Parse("4732B277-11B3-440D-9327-1E2E6613746C"));
            Assert.AreEqual(dr["DestinationGuid"].GetType(), typeof(Guid));
            Assert.AreEqual((Int16)dr["DestinationINT16"], 32767);
            Assert.AreEqual(dr["DestinationINT16"].GetType(), typeof(Int16));
            Assert.AreEqual((Int32)dr["DestinationINT32"], 2147483647);
            Assert.AreEqual(dr["DestinationINT32"].GetType(), typeof(Int32));
            Assert.AreEqual((Int64)dr["DestinationINT64"], 9223372036854775807);
            Assert.AreEqual(dr["DestinationINT64"].GetType(), typeof(Int64));
            Assert.AreEqual((string)dr["DestinationString"], "This is a string.");
            Assert.AreEqual(dr["DestinationString"].GetType(), typeof(string));

            Assert.AreNotEqual((Guid)dr["DestinationGenerateGuid"], Guid.Empty);
            Assert.AreEqual(dr["DestinationGenerateGuid"].GetType(), typeof(Guid));
            Assert.AreNotEqual((DateTime)dr["DestinationGenerateDateOnly"], DateTime.MinValue);
            Assert.AreEqual(dr["DestinationGenerateDateOnly"].GetType(), typeof(DateTime));
            Assert.AreNotEqual((DateTime)dr["DestinationGenerateDateTime"], DateTime.MinValue);
            Assert.AreEqual(dr["DestinationGenerateDateTime"].GetType(), typeof(DateTime));
        }

        #endregion

    }
}
