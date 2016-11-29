using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ReportsCompare.Web.Model;

namespace ReportsCompare.Web.Helper
{
    public sealed class CninfoClient : IDisposable
    {
        private readonly HttpClient client;
        private readonly string baseAddress = "http://www.cninfo.com.cn";

        public CninfoClient()
        {
            client = new HttpClient {BaseAddress = new Uri(baseAddress)};
        }

        private Task<HttpResponseMessage> PostAsync(string uri, IDictionary<string, string> formData)
        {
            var content = new MultipartFormDataContent("------WebKitFormBoundary0w85EIWMyqabaQ2F--");
            foreach (var data in formData)
            {
                content.Add(new StringContent(data.Value), data.Key);
            }

            return client.PostAsync(uri, content);
        }

        public Task<HttpResponseMessage> DownloadAsync(ReportQueryModel model)
        {
            return PostAsync("/cninfo-new/data/download", model.Serialize());
        }

        public Task<HttpResponseMessage> Query(string keyword)
        {
            return PostAsync("/cninfo-new/data/query", new Dictionary<string, string>()
            {
                {"keyWord", keyword},
                {"maxNum", "10"},
                // hq_or_cw==1 行情数据，hq_or_cw==2 财务数据
                {"hq_or_cw", "2"}
            });
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
