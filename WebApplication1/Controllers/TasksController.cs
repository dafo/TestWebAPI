using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = WebApplication1.Models.Task;
using Customer = WebApplication1.Models.Customer;

namespace WebApplication1.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskContext _dbContext;
        public TasksController(TaskContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET: /{tasklist}/{task}
        [HttpGet("{tasklist}/{task}")]
        public async Task<WebApplication1.Models.Task?> GetTask(string tasklist, int task)
        {
            if (_dbContext.Tasks == null)
            {
                return null;
            }

            var t = await _dbContext.Tasks.FindAsync(task);

            if (t == null || t.Parent != tasklist)
            {
                return null;
            }

            return t;
        }

        //POST: /task
        [HttpPost("task")]
        public async Task<WebApplication1.Models.Task?> InsertTask([FromBody]Task task)
        {
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return task;
        }

        //PUT: /{tasklist}/{task}
        [HttpPut("{tasklist}/{task}")]
        public async Task<WebApplication1.Models.Task?> UpdateTask(int id, Task task)
        {
            if (id != task.Id)
            {
                return null;
            }
            _dbContext.Entry(task).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        //DELETE: /{tasklist}/{task}
        [HttpDelete("{tasklist}/{task}")]
        public async Task<WebApplication1.Models.Task?> DeleteTask(string tasklist, int task)
        {
            if (_dbContext.Tasks == null)
            {
                return null;
            }

            var t = await _dbContext.Tasks.FindAsync(task);

            if (t == null || t.Parent != tasklist)
            {
                return null;
            }

            _dbContext.Tasks.Remove(t);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        //GET: /{tasklist}/
        [HttpGet("{tasklist}")]
        public async Task<ICollection<WebApplication1.Models.Task>> ListTasks(string tasklist, string status, bool deleted)
        {
            if (_dbContext.Tasks == null)
            {
                return new List<Task>();
            }

            if (!_dbContext.Tasks.AsQueryable().Where(t => t.Parent == tasklist).Any())
            {
                return new List<Task>();
            }

            var t = _dbContext.Tasks.AsQueryable().Where(t => t.Parent == tasklist).First();

            return new List<Task> { t };
        }

        //POST: /{id}
        [HttpPost("{id}")]
        public async Task<ICollection<WebApplication1.Models.Task>> AllTasks(string id)
        {
            if (_dbContext.Tasks == null)
            {
                return new List<Task>();
            }

            if (!_dbContext.Tasks.AsQueryable().Any())
            {
                return new List<Task>();
            }

            var t = _dbContext.Tasks.ToList();
            return t;
        }

        //POST: /{parentId}
        [HttpPost("{parentId}")]
        public async Task<ICollection<WebApplication1.Models.Task>> TasksByParentId(string parentId)
        {
            if (_dbContext.Tasks == null)
            {
                return new List<Task>();
            }

            if (!_dbContext.Tasks.AsQueryable().Where(t => t.Parent == parentId).Any())
            {
                return new List<Task>();
            }

            var t = _dbContext.Tasks.AsQueryable().Where(t => t.Parent == parentId);

            return t.ToList();
        }

        //GET: /{task}
        [HttpGet("{task}")]
        public async Task<Customer> GetCustomerFromTask(Task task)
        {
            return new Customer { Id = 1, Name = "Boris Petrov" };
        }

        //PATCH: /{id}/{task}
        [HttpPatch("id")]
        public async Task<WebApplication1.Models.Task?> PatchUpdateTask(int id, [FromBody]Task task)
        {
            if (id != task.Id)
            {
                return null;
            }
            _dbContext.Entry(task).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        [HttpDelete("{id}")]
        public async Task<WebApplication1.Models.Task?> DeleteCustomer(string id)
        {
            return null;
        }

        private bool TaskExists(int id)
        {
            return (_dbContext.Tasks?.Any(t => t.Id == id)).GetValueOrDefault();
        }
    }
}
