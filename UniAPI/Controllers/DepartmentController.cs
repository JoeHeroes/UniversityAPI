using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UniAPI.Models;
using UniAPI.Services;

namespace UniAPI.Controllers
{

    [Route("api/university/{id}/department")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;
       

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<DepartmentDto>> GetAll([FromRoute] int id)
        {
            var uni = _service.GetAll(id);
            return Ok(uni);
        }

        [HttpGet("{depId}")]
        public ActionResult<DepartmentDto> GetOne([FromRoute] int id, [FromRoute] int depId)
        {
            var uni = _service.GetById(id, depId);

            return Ok(uni);
        }

        [HttpDelete("{depId}")]
        //[Authorize(Roles ="Admin,Manage")]
        public ActionResult Delete([FromRoute] int id, [FromRoute] int depId)
        {
            _service.Delete(id, depId);

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateDepartment([FromRoute] int id,[FromBody] DepartmentDto dto)
        {
            HttpContext.User.IsInRole("Admin");
            var idDepartament = _service.Create(id, dto);

            return Created($"/api/university/{id}/departament/{idDepartament}",null);
        }

    }
}
