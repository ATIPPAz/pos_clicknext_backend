using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosApi.Context;
using PosApi.Models;
namespace PosApi.Controllers 
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UnitController : ControllerBase
    { 
        readonly posContext _posContext;
        readonly ILogger logger;
        public UnitController(posContext posContext,ILogger<UnitController> logger) {
            _posContext = posContext;
            this.logger = logger;
        }


        [HttpGet]
        public IActionResult getUnits()
        {
            logger.LogInformation("unit");
            List<unit> units = new List<unit>();
            var hh= new int[5];
            IEnumerable<int> x = hh.Skip(5);
            
            foreach (var item in _posContext.units)
            {
                units.Add(item);
            }

            return new JsonResult(new ResponseObject<List<unit>>()
            {
                data = units,
                statusCode=Models.StatusCode.successReturn
            });
        }

        [HttpPut]
        public IActionResult updateUnit(unit unit)
        {
            _posContext.units.Update(unit);
            _posContext.SaveChanges();
            return new JsonResult(unit);
        }

        [HttpDelete("{id:int}")]
        public IActionResult deleteUnit( int id ) {
            unit unitData = new unit() { unitId = id };
            _posContext.units.Attach(unitData);
            _posContext.units.Remove(unitData);
            _posContext.SaveChanges();
            return Ok(unitData);
        }

        [HttpPost]
        public IActionResult createUnit([FromBody] unit unitData)
        {
            if(unitData.unitName!="")
            {
                try
                {
                    _posContext.units.Add(unitData);
                    _posContext.SaveChanges();
                    return Ok(unitData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                    return  BadRequest("ชื่อไม่สมบูรณ์");
            }
        }   
    }
}
