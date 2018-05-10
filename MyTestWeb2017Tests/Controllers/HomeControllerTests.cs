using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTestWeb2017.Controllers;

namespace MyTestWeb2017Tests.Controllers
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void RedictTest()
        {
            new HomeController().Redict(1);
        }
    }
}