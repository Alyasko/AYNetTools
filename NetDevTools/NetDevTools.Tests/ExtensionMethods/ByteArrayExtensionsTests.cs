using System.Text;
using NetDevTools.ExtensionMethods;
using Xunit;

namespace NetDevTools.Tests.ExtensionMethods
{
    public class ByteArrayExtensionsTests
    {
        [Fact]
        public void GetString_NormalDefaultEncodingBytes_CorrectString()
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
        public void GetString_NormalUnicodeEncodingBytes_CorrectString()
        {
            // Arrange.
            var expString = "hello";
            var bytes = Encoding.Unicode.GetBytes(expString);

            // Act.
            var actString = bytes.GetStringUnicode();

            // Assert.
            Assert.Equal(expString, actString);
        }
    }
}
