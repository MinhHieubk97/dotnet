using brechtbaekelandt.removeDiactritics.Data.DbContext;
using brechtbaekelandt.removeDiactritics.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Linq;


namespace brechtbaekelandt.removeDiactritics.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly DataDbContext _context;

        public DataController(DataDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public ActionResult<Collection<Word>> Get([FromQuery]string filter = null)
        {
            IOrderedQueryable<Word> query;

            if (filter == null)
            {
                query = this._context.Words.OrderBy(w => w.String);
            }
            else
            {
                query = this._context.Words.Where(w => DataDbContext.RemoveDiacritics(w.String).Contains(DataDbContext.RemoveDiacritics(filter))).OrderBy(w => w.String);
            }

            return new Collection<Word>(query.ToArray());
        }
    }
}
