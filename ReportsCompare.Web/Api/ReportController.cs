using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace ReportsCompare.Web.Api
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        [HttpGet]
        public JsonResult Get([FromQuery] string file)
        {
            // 解决中文乱码问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var list = new List<string[]>();
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var csv = new CsvReader(new StreamReader(stream, Encoding.GetEncoding(0) /*Encoding.Default*/));
                csv.Configuration.HasHeaderRecord = false;
                while (csv.Read())
                {
                    list.Add(csv.CurrentRecord);
                }
            }

            return new JsonResult(list);
        }
    }
}
