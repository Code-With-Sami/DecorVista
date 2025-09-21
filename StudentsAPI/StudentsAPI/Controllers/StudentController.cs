using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsAPI.Models;

namespace StudentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private MyDbContext _db;
        public StudentController(MyDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents ()
        {
            var data = await _db.Students.ToListAsync();
            return Ok(data);
        }
    }
}
