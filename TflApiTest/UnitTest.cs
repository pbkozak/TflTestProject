using Microsoft.VisualStudio.TestTools.UnitTesting;
using TflProject;
using System.Configuration;

namespace TflApiTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestConnection()
        {
            var ta = new TflApi("", "");
            var response = ta.CheckRoadStatus("A2");
             
            Assert.AreEqual(response.Error && !response.Success && response.Messages[0] == "No credentials provided", true);
        }

        [TestMethod]
        public void TestKnownRoadStatus()
        {
            var tflApiKey = ConfigurationManager.AppSettings["tflAppKey"];
            var tflApiId = ConfigurationManager.AppSettings["tflAppId"];
            var ta = new TflApi(tflApiId, tflApiKey);

            var status = ta.CheckRoadStatus("A2");
            Assert.AreEqual(!status.Error && status.Success, true);
        }

        [TestMethod]
        public void TestUnknownRoadStatus()
        {
            var tflApiKey = ConfigurationManager.AppSettings["tflAppKey"];
            var tflApiId = ConfigurationManager.AppSettings["tflAppId"];
            var ta = new TflApi(tflApiId, tflApiKey);

            var status = ta.CheckRoadStatus("A233");
            Assert.AreEqual(!status.Error && !status.Success, true);
        }
    }
}
