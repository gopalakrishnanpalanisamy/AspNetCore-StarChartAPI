using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
        [HttpGet("{id:int}",Name="GetById")]
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
        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(obj => obj.Name == name).ToList();
            if (result.Count==0)
            {
                return NotFound();
            }
            result.ForEach(r =>
            {
                r.Satellites = _context.CelestialObjects.Where(obj => obj.OrbitedObjectId == r.Id).ToList();
            });
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

         [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id },celestialObject);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id,[FromBody]CelestialObject celestialObject)
        {
            var result = _context.CelestialObjects.Where(obj => obj.Id == id).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            result.Name = celestialObject.Name;
            result.OrbitalPeriod = celestialObject.OrbitalPeriod;
            result.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.Where(obj => obj.Id == id).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            result.Name = name;
            _context.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _context.CelestialObjects.Where(obj => obj.Id == id).ToList();
            if (result.Count==0)
            {
                return NotFound();
            }
            _context.RemoveRange(result);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
