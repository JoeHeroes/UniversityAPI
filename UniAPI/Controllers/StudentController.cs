using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UniAPI.Models;
using UniAPI.Services;

namespace UniAPI.Controllers
{

    [Route("api/student")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
       

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<StudentDto>> GetAll()
        {
            var uni = _service.GetAll();
            return Ok(uni);
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDto> GetOne([FromRoute] int id)
        {
            var uni = _service.GetById(id);

            return Ok(uni);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin,Manage")]
        public ActionResult Delete([FromRoute]int id)
        {
            _service.Delete(id);

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateStudent([FromBody] CreateStudentDto dto)
        {
         
            var id = _service.Create(dto);

            return Created($"/api/student/{id}",null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateStudentDto dto,[FromRoute] int id)
        {
           
            _service.Update(id, dto);

            return Ok();
        }
    }
}
