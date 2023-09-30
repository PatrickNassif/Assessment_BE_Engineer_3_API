using System.Security.AccessControl;
using static Assessment_BE_Engineer_3_Utility.SD;

namespace Assessment_BE_Engineer_3_Web.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}
