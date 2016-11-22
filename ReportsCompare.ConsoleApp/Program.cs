using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReportsCompare.ConsoleApp
{
    static class Program
    {

        static void Main(string[] args)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://www.cninfo.com.cn");

            /*
             *  ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="K_code"


                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="market"

                sz
                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="type"

                lrb
                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="code"

                000860
                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="orgid"

                gssz0000860
                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="minYear"

                2014
                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="maxYear"

                2016
                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="hq_code"


                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="hq_k_code"


                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="cw_code"

                000860
                ------WebKitFormBoundary0w85EIWMyqabaQ2F
                Content-Disposition: form-data; name="cw_k_code"


                ------WebKitFormBoundary0w85EIWMyqabaQ2F--
             */

            var content = new MultipartFormDataContent("------WebKitFormBoundary0w85EIWMyqabaQ2F--")
            {
                {new StringContent("sz"), "market"},
                //lrb 利润表  fzb 资产表 llb 现金表
                {new StringContent("lrb"), "type"},
                // stock code
                {new StringContent("000860"), "code"},
                // gssz+{stock code}
                {new StringContent("gssz0000860"), "orgid"},
                {new StringContent("2014"), "minYear"},
                {new StringContent("2016"), "maxYear"}
                //content.Add(new StringContent("000860"), "cw_code");
            };

            var result = client.PostAsync("/cninfo-new/data/download", content).Result;

            var file = result.Content.ReadAsStreamAsync().Result;
            using (var stream = new FileStream("000860.zip", FileMode.Create, FileAccess.ReadWrite))
            {
                file.CopyToAsync(stream).ContinueWith(t => Console.WriteLine("download successful"));
            }


            Console.ReadLine();
        }
    }
}
