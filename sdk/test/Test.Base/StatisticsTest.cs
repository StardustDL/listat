using Listat;
using Listat.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Base
{
    [TestClass]
    public class StatisticsTest
    {
        ListatService Service { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var client = Utils.CreateTestClient();

            Service = new ListatService(client);
        }

        [TestMethod]
        public async Task CreateGetAndDelete()
        {
            var statistic = new Statistic
            {
                Payload = "abc",
                CreationTime = DateTimeOffset.Now,
                Uri = "abc"
            };
            var id = await Service.Create(statistic);
            Assert.IsNotNull(id);

            var res = await Service.Get(id);

            Assert.AreEqual(statistic.Payload, res.Payload);
            Assert.IsTrue((statistic.CreationTime - res.CreationTime).TotalSeconds < 60);

            {
                var updated = await Service.Update(new Statistic
                {
                    Id = id,
                    Payload = "abcd",
                    Uri = "abc"
                });
                Assert.IsTrue(updated);

                var res2 = await Service.Get(id);

                Assert.AreEqual("abcd", res2.Payload);
            }

            var items = await Service.Query(new StatisticQuery
            {
                Uri = "abc"
            });
            Assert.IsTrue(items.Count > 0);

            var count = await Service.Count(new StatisticQuery
            {
                Uri = "abc"
            });
            Assert.IsTrue(count > 0);

            var del = await Service.Delete(id);
            Assert.IsTrue(del);
        }

        [TestMethod]
        public async Task Query()
        {
            var items = await Service.Query(new StatisticQuery());
            Assert.IsNotNull(items);
        }

        [TestCleanup]
        public void Clean()
        {
            Service.Client.Dispose();
        }
    }
}
