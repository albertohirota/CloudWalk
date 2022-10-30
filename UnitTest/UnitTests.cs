using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudWalkProject;

namespace CloudWalkProject.Tests
{
    [TestClass()]
    public class UnitTests
    {
        [TestMethod()]
        public void ReadFile_CheckExists_True()
        {
            string filePath = @"C:\Users\alber\source\repos\CloudWalk\UnitTest\TestLog\TestLogSample.txt";

            bool exists = CreateReport.CheckFileExists(filePath);

            Assert.IsTrue(exists);
        }

        [TestMethod()]
        public void ReadFile_CheckExists_False()
        {
            string filePath = "C:\\Users\\TestLogSample.txt";

            bool exists = CreateReport.CheckFileExists(filePath);

            Assert.IsFalse(exists);
        }
    }
}
