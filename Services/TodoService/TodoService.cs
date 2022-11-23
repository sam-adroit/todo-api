using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Week8.Data;
using Week8.Dto;
using Week8.Model;

namespace Week8.Services.TodoService
{
    public class TodoService : ITodoService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TodoService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            
        }
        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private List<GetTodo> MappedTodo(List<Todo> todo) {
            var todoList = new List<GetTodo>();
            foreach(var t in todo) {
                var td = new GetTodo {
                    Id=t.Id,
                    Task=t.Task,
                    Category=t.Category,
                    Completed=t.Completed,
                    CreatedAt=t.CreatedAt,
                    Description=t.Description,
                    EndDate=t.EndDate
                };
                todoList.Add(td);
            }
            return todoList;
        }
        private GetTodo MappedATodo(Todo todo) {
            var todoMap = new GetTodo {
                    Id=todo.Id,
                    Task=todo.Task,
                    Category=todo.Category,
                    Completed=todo.Completed,
                    CreatedAt=todo.CreatedAt,
                    Description=todo.Description,
                    EndDate=todo.EndDate
                };
     
            return todoMap;
        }

        public async Task<ServiceResponse<List<GetTodo>>> AddTodo(AddTodoDto todo)
        {
            var response = new ServiceResponse<List<GetTodo>>();
            var todoModel = new Todo();
            todoModel.Id = Guid.NewGuid();
            todoModel.Task = todo.Task;
            todoModel.Description = todo.Description;
            todoModel.Completed = todo.Completed;
            todoModel.EndDate = todo.EndDate;
            todoModel.Category = todo.Category;
            todoModel.User = await _context.Users.Where(u => u.Id == GetUserId()).FirstAsync();
            await _context.Todos.AddAsync(todoModel);
            await _context.SaveChangesAsync();

            var getTodo = await _context.Todos.ToListAsync();
            response.Data = MappedTodo(getTodo);

            return response;
            
        }

        public async Task<ServiceResponse<List<GetTodo>>> Delete(Guid id)
        {
            var response = new ServiceResponse<List<GetTodo>>();
            try{
                var todo = await _context.Todos.Where(t => t.Id == id && t.User.Id == GetUserId()).FirstAsync();
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

            }catch(Exception ex){
                response.Message=ex.Message;
                response.Success= false;
            }
            //var getTodo = await _context.Todos.ToListAsync();
            var getTodo = await _context.Todos.Where(t=>t.User.Id==GetUserId()).ToListAsync();
            response.Data = MappedTodo(getTodo);

            return response;
        }

        public async Task<ServiceResponse<GetTodo>> Todo(Guid id)
        {
            var response = new ServiceResponse<GetTodo>();
            try{
                var getTodo = await _context.Todos.Where(t=>t.Id==id && t.User.Id == GetUserId()).FirstAsync();
                response.Data = MappedATodo(getTodo);
            }catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message + ":" +"Task does not exist";
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetTodo>>> Todos()
        {
            var response = new ServiceResponse<List<GetTodo>>();
            // var getTodo = await _context.Todos.ToListAsync();
            var getTodo = await _context.Todos.Where(t=>t.User.Id==GetUserId()).ToListAsync();
            response.Data = MappedTodo(getTodo);
            return response;
        }

        public async Task<ServiceResponse<GetTodo>> Update(UpdateDto todo)
        {
            var response = new ServiceResponse<GetTodo>();
            try{
                var getTodo = await _context.Todos.Where(t=>t.Id==todo.Id&&t.User.Id == GetUserId()).FirstAsync();
                getTodo.Task = todo.Task;
                getTodo.Completed = todo.Completed;
                getTodo.Category = todo.Category;
                getTodo.Description = todo.Description;
                getTodo.EndDate = todo.EndDate;
                await _context.SaveChangesAsync();
                response.Data = MappedATodo(getTodo);
            }catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message + ":" +"Tak does not exist";
            }
            return response;
        }
    }
}