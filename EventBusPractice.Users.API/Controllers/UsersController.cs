using EventBusPractice.Users.API.Data;
using EventBusPractice.Users.API.Entities;
using EventBusPractice.Users.API.EventBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EventBusPractice.Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDbContext _context;
        private readonly IEventBus _eventBus;

        public UsersController(UsersDbContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = user.ID,
                newname = user.Name
            });
            _eventBus.Publish("user.update", integrationEventData);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = user.ID,
                name = user.Name
            });
            _eventBus.Publish("user.add", integrationEventData);

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }
    }
}
