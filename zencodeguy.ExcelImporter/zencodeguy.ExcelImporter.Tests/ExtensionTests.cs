using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace zencodeguy.ExcelImporter.Tests
{
    [TestClass]
    public class ExtensionTests
    {
        #region GetNextWord
        [TestMethod]
        public void GetNextWordReturnsFirstWord()
        {
            // Arrange
            var s = "This is a collection of words";
            // Act
            var firstWord = s.GetNextWord(0);
            // Assert 
            Assert.AreEqual("This", firstWord);
        }

        [TestMethod]
        public void GetNextWordReturnsNextWord()
        {
            // Arrange
            var s = "This is a collection of words";
            // Act
            var firstWord = s.GetNextWord(4);
            // Assert 
            Assert.AreEqual("is", firstWord);
        }

        [TestMethod]
        public void GetNextWordReturnsLastWord()
        {
            // Arrange
            var s = "This is a collection of words";
            // Act
            var firstWord = s.GetNextWord(23);
            // Assert 
            Assert.AreEqual("words", firstWord);
        }
        #endregion

        #region ExtractDelimitedSection
        [TestMethod]
        public void ExtractDelimitedSectionReturnsDelimitedText()
        {
            // Arrange
            var s = "This contains |a string inside pipes| with stuff on either side.";

            // Act
            var e = s.ExtractDelimitedSection('|', '|', 0);

            // Assert
            Assert.AreEqual("a string inside pipes", e.Item1);
        }

        [TestMethod]
        public void ExtractDelimitedSectionReturnsTextWithDifferentOpenAndCloseDelimiters()
        {
            // Arrange
            var s = "This contains {a string in the middle} of the text.";
            // Act
            var e = s.ExtractDelimitedSection('{', '}', 0);
            // Assert
            Assert.AreEqual("a string in the middle", e.Item1);
        }

        [TestMethod]
        public void ExtractDelimitedSelectionReturnsTextAtStartOfString()
        {
            // Arrange
            var s = "{This is} the start of the string.";
            // Act
            var e = s.ExtractDelimitedSection('{', '}', 0);
            // Arrange
            Assert.AreEqual("This is", e.Item1);
        }

        [TestMethod]
        public void ExtractDelimitedSelectionReturnsTextAtEndOfString()
        {
            // Arrange
            var s = "This is the {end of string}";
            // Act
            var e = s.ExtractDelimitedSection('{', '}', 0);
            // Assert
            Assert.AreEqual("end of string", e.Item1);
        }

        [TestMethod]
        public void ExtractDelimitedSelectionCorrectlyHandlesEscapedCharacters()
        {
            // Arrange
            var s = @"This {contains \{escaped\} characters} in the string.";
            // Act
            var e = s.ExtractDelimitedSection('{', '}', 0);
            // Assert
            Assert.AreEqual("contains {escaped} characters", e.Item1);
        }

        [TestMethod]
        public void ExtractListItemsReturnsValue()
        {
            //Arrange
            var s = "This is a list: { \"one\", \"two\", \"three\" } of things";

            // Act
            var e = s.ExtractDelimitedSection('{', '}', 0);
            var l = e.Item1.ExtractListItems('"', '"', ',', 0);

            // Assert
            Assert.AreEqual(3, l.Count);
            Assert.IsTrue(l.Contains("one"));
            Assert.IsTrue(l.Contains("two"));
            Assert.IsTrue(l.Contains("three"));
        }
        #endregion

        #region ReadLine
        [TestMethod]
        public void ReadLineReturnsTwoLinesAsOne()
        {
            // Arrange
            var inputLine = " Line 1  \n\tLine 2;";
            string outputLine = string.Empty;
            
            // Act
            foreach (var l in inputLine.ReadLine())
            {
                outputLine = l;
            }

            // Assert
            Assert.AreEqual("Line 1 Line 2", outputLine);
        }

        [TestMethod]
        public void ReadLineSkipsBlankLines()
        {
            // Arrange
            var inputLine = "\n   \nThis is the line;";
            string outputLine = string.Empty;

            // Act
            foreach(var l in inputLine.ReadLine())
            {
                outputLine = l;
            }

            // Assert
            Assert.AreEqual("This is the line", outputLine);
        }


        [TestMethod]
        public void ReadLineReturnsThreeLinesAsOne()
        {
            // Arrange
            var inputLine = "Line 1\n\tLine 2\n\t\t\tLine 3;";

            // Act
            string outputLine = string.Empty;
            foreach (var l in inputLine.ReadLine())
            {
                outputLine = l;
            }

            // Assert
            Assert.AreEqual("Line 1 Line 2 Line 3", outputLine);
        }

        #endregion


    }
}
