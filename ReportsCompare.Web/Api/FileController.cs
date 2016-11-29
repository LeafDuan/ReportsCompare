using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportsCompare.Web.Helper;
using ReportsCompare.Web.Model;

namespace ReportsCompare.Web.Api
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] ReportQueryModel model)
        {
            var minYear = Math.Min(model.MinYear, model.MaxYear);
            var maxYear = Math.Max(model.MinYear, model.MaxYear);
            var pattern = $"{model.Code}_{model.Type}_{minYear}_{maxYear}";
            var path = $"data/{pattern}";
            if (model.ReDownload != "redo" && Directory.Exists(path))
            {
                var files = Directory.GetFiles(path).Select(p => p.Replace("\\", "/"));
                return new JsonResult(files);
            }

            var client = new CninfoClient();
            var result = await client.DownloadAsync(model);
            var file = await result.Content.ReadAsStreamAsync();

            var filename = $"data/{pattern}.zip";
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                return await file.CopyToAsync(stream)
                    .ContinueWith(t =>
                    {
                        stream.Dispose();
                        ZipHelper.Extract(filename, path);
                        var list = Directory.GetFiles(path).Select(p => p.Replace("\\", "/"));
                        return new JsonResult(list);
                    });
            }
        }
    }
}
