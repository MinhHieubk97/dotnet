using brechtbaekelandt.enableODataQueryOnComputedProperties.Data.DbContext;
using brechtbaekelandt.enableODataQueryOnComputedProperties.Data.Entities;
using brechtbaekelandt.enableODataQueryOnComputedProperties.Extensions;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Controllers.Api
{
    public class PersonsController : ODataController
    {
        private readonly DataDbContext _db = new DataDbContext();

        // GET: odata/Persons
        [EnableQuery]
        public IQueryable<Person> GetPersons(ODataQueryOptions opts)
        {
            var query = this._db.Persons.AsQueryable();

            query = query.EnableODataQueryOnComputedProperties(opts);

            return query;
        }

        // GET: odata/Persons(5)
        [EnableQuery]
        public Person GetPerson([FromODataUri] int key)
        {
            return this._db.Persons.FirstOrDefault(person => person.Id == key);
        }

        // PUT: odata/Persons(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Person> patch)
        {
            this.Validate(patch.GetInstance());

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var person = await this._db.Persons.FindAsync(key);

            if (person == null)
            {
                return this.NotFound();
            }

            patch.Put(person);

            try
            {
                await this._db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.PersonExists(key))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.Updated(person);
        }

        // POST: odata/Persons
        public async Task<IHttpActionResult> Post(Person person)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            this._db.Persons.Add(person);

            await this._db.SaveChangesAsync();

            return this.Created(person);
        }

        // PATCH: odata/Persons(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Person> patch)
        {
            this.Validate(patch.GetInstance());

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var person = await this._db.Persons.FindAsync(key);

            if (person == null)
            {
                return this.NotFound();
            }

            patch.Patch(person);

            try
            {
                await this._db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.PersonExists(key))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.Updated(person);
        }

        // DELETE: odata/Persons(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var person = await this._db.Persons.FindAsync(key);

            if (person == null)
            {
                return this.NotFound();
            }

            this._db.Persons.Remove(person);

            await this._db.SaveChangesAsync();

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Persons(5)/Pets
        [EnableQuery]
        public IQueryable<Animal> GetPets([FromODataUri] int key)
        {
            return this._db.Persons.Where(m => m.Id == key).SelectMany(m => m.Pets);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._db.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool PersonExists(int key)
        {
            return this._db.Persons.Any(e => e.Id == key);
        }
    }
}
