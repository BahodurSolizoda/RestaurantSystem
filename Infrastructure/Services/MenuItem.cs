using System.Net;
using Dapper;
using Domain.Entities;
using Infrastructure.DataContext;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class MenuItem(DapperContext context):IGenericService<MenuItem>
{
    public async Task<ApiResponse<List<MenuItem>>> GetAll()
    {
        using var connection = context.Connection;
        var sql="select * from menu_items";
        var result=await connection.QueryAsync<MenuItem>(sql);
        return new ApiResponse<List<MenuItem>>(result.ToList());
    }

    public async Task<ApiResponse<MenuItem>> GetById(int id)
    {
        using var connection = context.Connection;
        string sql = "select * from menu_items where Id = @Id";
        var result = await connection.QuerySingleOrDefaultAsync<MenuItem>(sql, new { Id = id });
        if (result == null) return new ApiResponse<MenuItem>(HttpStatusCode.NotFound, "MenuItem not found");
        return new ApiResponse<MenuItem>(result);
    }

    public async Task<ApiResponse<bool>> Add(MenuItem data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     insert into menu_items(name, price, description) values (@Name, @Price, @Description);
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Update(MenuItem data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     update menu_items set name = @name, price = @price where Id = @Id;
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Delete(int id)
    {
        using var connection = context.Connection;
        string sql = "delete from menu_items where Id = @Id";
        var res = await connection.ExecuteAsync(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "MenuItem not found");
        return new ApiResponse<bool>(res != 0);
    }
}
