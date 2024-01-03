using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HulkBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransController : ControllerBase
    {
        [HttpGet]
        [Route("Query/{transId}")]
        public string Query(string transId) 
        {
            return $"this is transaction: {transId}";
        }
    }
}
