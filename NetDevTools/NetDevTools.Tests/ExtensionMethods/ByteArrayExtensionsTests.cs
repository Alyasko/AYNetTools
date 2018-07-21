using System;
using System.Linq;
using System.Text;
using NetDevTools.ExtensionMethods;
using Xunit;

namespace NetDevTools.Tests.ExtensionMethods
{
    public class ByteArrayExtensionsTests
    {
        [Fact]
        public void GetString_NormalDefaultEncodedBytes_CorrectString()
        {
            // Arrange.
            var expString = "hello";
            var bytes = Encoding.Default.GetBytes(expString);

            // Act.
            var actString = bytes.GetString();

            // Assert.
            Assert.Equal(expString, actString);
        }

        [Fact]
        public void GetStringUnicode_NormalUnicodeEncodedBytes_CorrectString()
        {
            // Arrange.
            var expString = "hello";
            var bytes = Encoding.Unicode.GetBytes(expString);

            // Act.
            var actString = bytes.GetStringUnicode();

            // Assert.
            Assert.Equal(expString, actString);
        }

        [Fact]
        public void DecodeString_ZeroBytes_EmptyString()
        {
            // Arrange.
            var expString = "";
            var bytes = Enumerable.Repeat((byte) 0, 10).ToArray();

            // Act.
            var actString = bytes.DecodeString();

            // Assert.
            Assert.Equal(expString, actString);
        }

        [Fact]
        public void DecodeString_DefaultNormalWithZeroBytes_NormalString()
        {
            // Arrange.
            var expString = "hello";
            var bytes =  Encoding.Default.GetBytes(expString).Concat(Enumerable.Repeat((byte)0, 10)).ToArray();

            // Act.
            var actString = bytes.DecodeString();

            // Assert.
            Assert.Equal(expString, actString);
        }

        [Fact]
        public void DecodeString_DefaultNormalWithZeroWithNormalBytes_NormalString()
        {
            // Arrange.
            var expString = "hello";
            var bytes = Encoding.Default.GetBytes(expString).Concat(Enumerable.Repeat((byte)0, 10)).Concat(Encoding.Default.GetBytes("second")).ToArray();

            // Act.
            var actString = bytes.DecodeString();

            // Assert.
            Assert.Equal(expString, actString);
        }
    }
}
