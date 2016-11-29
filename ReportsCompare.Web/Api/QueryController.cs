using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportsCompare.Web.Helper;

namespace ReportsCompare.Web.Api
{
    [Route("api/[controller]")]
    public class QueryController : Controller
    {
        [HttpGet]
        public async Task<JsonResult> Get(string keyword)
        {
            var client = new CninfoClient();

            var result = await client.Query(keyword);
            var content = await result.Content.ReadAsStringAsync();

            return new JsonResult(content);
        }
    }
}
