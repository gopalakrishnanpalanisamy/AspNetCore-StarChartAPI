using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetById")]
        [Route("{id:int")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Where(obj => obj.Id == id).FirstOrDefault();
            if (result == null)
            {
                return NotFound();                
            }
            result.Satellites = _context.CelestialObjects.Where(obj => obj.OrbitedObjectId == id).ToList();
            return Ok(result);

        }

        [HttpGet("GetByName")]
        [Route("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(obj => obj.Name == name).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            result.Satellites = _context.CelestialObjects.Where(obj => obj.OrbitedObjectId == result.Id).ToList();
            return Ok(result);
        }

        [HttpGet("GetAll")]
        [Route("")]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();
            if (result == null)
            {
                return NotFound();
            }
            result.ForEach(r =>
            {
                r.Satellites = _context.CelestialObjects.Where(obj => obj.OrbitedObjectId == r.Id).ToList();
            });
            return Ok(result);
        }

    }
}
