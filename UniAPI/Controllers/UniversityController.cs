using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UniAPI.Models;
using UniAPI.Services;

namespace UniAPI.Controllers
{

    [Route("api/university")]
    [ApiController]
    public class UniversityController:ControllerBase
    {
        private readonly IUniversityService _service;
       

        public UniversityController(IUniversityService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UniversityDto>> GetAll()
        {


            var uni = _service.GetAll();
            return Ok(uni);
        }

        [HttpGet("{id}")]
        public ActionResult<UniversityDto> GetOne([FromRoute] int id)
        {
            var uni = _service.GetById(id);

            return Ok(uni);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            _service.Delete(id);

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateUniversityDto dto)
        {
         
            var id = _service.Create(dto);

            return Created($"/api/university/{id}",null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateUniversityDto dto,[FromRoute] int id)
        {
           
            _service.Update(id, dto);

            return Ok();
        }
    }
}
