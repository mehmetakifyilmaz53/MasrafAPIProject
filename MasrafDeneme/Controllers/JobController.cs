using Microsoft.AspNetCore.Mvc;

namespace MasrafDeneme.Controllers
{
    public class JobController : ControllerBase
    {
        [HttpGet]
        public string Status()
        {
            return null;
            //return JsonConvert.SerializeObject(CronJobManager.GetJobStatus(), Formatting.Indented);
        }
    }
}
