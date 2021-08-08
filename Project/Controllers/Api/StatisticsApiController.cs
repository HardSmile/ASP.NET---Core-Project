namespace Project.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Project.Services.Statistics;
   

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statistics;
        public StatisticsApiController(IStatisticsService statistics)
            => this.statistics = statistics;
        [HttpGet]
        public StatisticsServiceModel GetStatistics() => this.statistics.Total();
    }
}
