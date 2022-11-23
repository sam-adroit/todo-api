using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week8.Dto;

namespace Week8.Services.TodoService
{
    public interface ITodoService
    {
        Task<ServiceResponse<List<GetTodo>>> AddTodo(AddTodoDto todo);
        Task<ServiceResponse<List<GetTodo>>> Todos();
        Task<ServiceResponse<GetTodo>> Todo(Guid id);
        Task<ServiceResponse<GetTodo>> Update(UpdateDto todo);
        Task<ServiceResponse<List<GetTodo>>> Delete(Guid id);
    }
}