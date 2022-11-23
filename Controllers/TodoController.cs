using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Week8.Dto;
using Week8.Services.TodoService;

namespace Week8.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }
        [HttpGet("todos")]
        public async Task<ActionResult<ServiceResponse<List<GetTodo>>>> Get(){
            return Ok(await _todoService.Todos());
        }

        [HttpGet("todo/{id}")]
        public async Task<ActionResult<ServiceResponse<GetTodo>>> Get(Guid id){
            var response = await _todoService.Todo(id);
            if(response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<List<GetTodo>>>> Add(AddTodoDto todo){
            return Ok(await _todoService.AddTodo(todo));
        }

        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<GetTodo>>> Update(UpdateDto todo){
            var response = await _todoService.Update(todo);
            if(response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetTodo>>>> Delete(Guid id){
            var response = await _todoService.Delete(id);
            if(response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}