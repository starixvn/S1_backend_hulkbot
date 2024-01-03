using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;
using System.Text;

namespace HulkBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransController(IOptions<Elastic> elasticOption) : ControllerBase
    {
        private readonly Elastic _elasticOption = elasticOption.Value;

        [HttpGet]
        [Route("Query/{transId}")]
        public string Query(string transId) 
        {
            var elasticClient = GetElasticClient();
            var response = elasticClient.Search<dynamic>(a => a
            .From(0)
            .Size(10)
            .Sort(ss => ss.Descending("Timestamp"))
            .Index("prod-payment-*")
            .Query(q => q
                .Bool(b => b
                    .Must(
                        q => q.Match(mt => mt
                        .Field("MessageTemplate")
                        .Query(transId)),
                        q => q.DateRange(dr => dr
                        .Field("Timestamp")
                        .GreaterThanOrEquals(DateTime.Now.AddDays(-1))
                        .LessThanOrEquals(DateTime.Now)))
                    )
                ));

            if (response.IsValid)
            {
                var sb = new StringBuilder();
                foreach (var item in response.Documents)
                {
                    sb.AppendLine(item["MessageTemplate"]);
                }
                return sb.ToString();
            }
            return $"query error";
        }

        private ElasticClient GetElasticClient()
        {
            var connectionSetting = new ConnectionSettings(new Uri(_elasticOption.ConnectionString))
                .BasicAuthentication(_elasticOption.Username, _elasticOption.Password);
            return new ElasticClient(connectionSetting);
        }
    }
}
