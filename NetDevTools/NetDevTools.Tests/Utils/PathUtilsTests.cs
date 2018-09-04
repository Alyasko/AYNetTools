using System;
using System.Collections.Generic;
using System.Text;
using NetDevTools.Utils;
using Xunit;

namespace NetDevTools.Tests.Utils
{
    public class PathUtilsTests
    {
        [Theory]
        [InlineData("P:\\Projects\\Git\\NetDevTools\\NetDevTools\\NetDevTools.ProjectRunner\\bin\\Release\\netcoreapp2.0\\NetDevTools.ProjectRunner.dll", 29, "P:\\...\\NetDevTools.ProjectRunner.dll")]
        public void ShortenLongPath_DifferentInput_ExpectedPathEqualsToActual(string pathLong, int maxCharsCount, string pathShort)
        {
            var actual = PathUtils.ShortenLongPath(pathLong, maxCharsCount);

            Assert.Equal(pathShort, actual);
        }
    }
}
