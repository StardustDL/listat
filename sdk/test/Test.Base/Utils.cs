using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Base
{
    class Utils
    {
        public static HttpClient CreateTestClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:4500/")
            };
            return client;
        }
    }
}
