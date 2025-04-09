using System;
using API.Common;
using API.DTOs;

namespace API.Services.Interface;

public interface ITodosService
{
    Task<ResponseDto> GetAllTodoItems();
    Task<ResponseDto> GetTodoItemById(int id);
    Task<ResponseDto> AddTodoItem(TodoItemAddDto todoAddDto);
    Task<ResponseDto> UpdateTodoItem(TodoItemUpdateDto todoItemDto);
    Task<ResponseDto> DeleteTodoItem(int id);

}
