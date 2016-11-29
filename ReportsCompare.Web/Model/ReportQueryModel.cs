using System.Collections.Generic;

namespace ReportsCompare.Web.Model
{
    public sealed class ReportQueryModel
    {
        public string Market { get; set; }
        //stock code
        public string Code { get; set; }
        //lrb 利润表  fzb 资产表 llb 现金表
        public string Type { get; set; }
        public string OrgId { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public string ReDownload { get; set; }

        public IDictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>()
            {
                {"market", Market},
                {"type", Type},
                {"code", Code},
                {"orgid", OrgId},
                {"minYear", MinYear.ToString()},
                {"maxYear", MaxYear.ToString()}
            };
        }
    }
}
