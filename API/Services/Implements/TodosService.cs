using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using API.Common;
using API.Data.UnitOfWork;
using API.DTOs;
using API.Entities;
using API.Helper;
using API.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using SQLitePCL;

namespace API.Services.Implements;

public class TodosService : ITodosService
{
    private readonly IUnitOfWork _unitOfWork;
    public TodosService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;

    }
    public async Task<ResponseDto> GetAllTodoItems()
    {
        var response = await _unitOfWork.Todos.GetAll(x => x.UserId == _unitOfWork.LoggedInUserId() && x.IsDeleted == false);


        return Utilities.SuccessResponseForGet(response);
    }

    public async Task<ResponseDto> GetTodoItemById(int id)
    {
        var response = await _unitOfWork.Todos.GetById(id);
        return Utilities.SuccessResponseForGet(response);
    }
    public async Task<ResponseDto> AddTodoItem(TodoItemAddDto todoAddDto)
    {
        var userId = _unitOfWork.LoggedInUserId();

        var todoItem = new TodoItem
        {
            Title = todoAddDto.Title,
            Description = todoAddDto.Description,
            DueDate = todoAddDto.DueDate != null ? todoAddDto.DueDate.Value.Date : null,
            UserId = userId,
            IsDeleted = false,
            IsCompleted = false,
            CompletedDate = null,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = null,

        };
        _unitOfWork.Todos.Add(todoItem);
        await _unitOfWork.SaveAsync();
        var response = new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            DueDate = todoItem.DueDate != null ? todoItem.DueDate.Value.ToString("yyyy-MM-dd") : null,
            IsCompleted = todoItem.IsCompleted,
            CompletedDate = todoItem.CompletedDate,
            CreatedDate = todoItem.CreatedDate,
        };//have to use mapper

        return Utilities.SuccessResponseForAdd(response);

    }
    public async Task<ResponseDto> UpdateTodoItem(TodoItemUpdateDto todoItemDto)
    {
        var existingTodoItem = await _unitOfWork.Todos.GetById(todoItemDto.Id);
        if (existingTodoItem == null) return Utilities.ValidationErrorResponse("Not Found");

        existingTodoItem.Title = todoItemDto.Title;
        existingTodoItem.Description = todoItemDto.Description;
        existingTodoItem.DueDate = todoItemDto.DueDate;
        existingTodoItem.IsCompleted = todoItemDto.IsCompleted;
        existingTodoItem.CompletedDate = todoItemDto.IsCompleted == true ? DateTime.Now : null;
        _unitOfWork.Todos.Update(existingTodoItem);
        await _unitOfWork.SaveAsync();

        var response = new TodoItemDto
        {
            Id = existingTodoItem.Id,
            Title = existingTodoItem.Title,
            Description = existingTodoItem.Description,
            DueDate = existingTodoItem.DueDate != null ? existingTodoItem.DueDate.Value.ToString("yyyy-MM-dd") : null,
            IsCompleted = existingTodoItem.IsCompleted,
            CompletedDate = existingTodoItem.CreatedDate,
            CreatedDate = existingTodoItem.CreatedDate,

        };//have to use mapper

        return Utilities.SuccessResponseForUpdate(response);

    }
    public async Task<ResponseDto> DeleteTodoItem(int id)
    {
        var todoItem = await _unitOfWork.Todos.GetById(id);
        if (todoItem == null) return Utilities.ValidationErrorResponse("Not Found");
        todoItem.IsDeleted = true;
        todoItem.UpdatedDate = DateTime.Now;
        _unitOfWork.Todos.Update(todoItem);

        return Utilities.SuccessResponseForDelete();

    }
    private async Task<bool> TodoItemExists(int id) // Changed parameter type to int
    {
        return await _unitOfWork.Todos.Any(todo => todo.Id == id);
    }


}
