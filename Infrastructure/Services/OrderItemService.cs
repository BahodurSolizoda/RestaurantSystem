using System.Net;
using Dapper;
using Domain.Entities;
using Infrastructure.DataContext;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class OrderItemService(DapperContext context):IGenericService<OrderItem>
{
    public async Task<ApiResponse<List<OrderItem>>> GetAll()
    {
        using var connection = context.Connection;
        var sql="select * from orderitems";
        var result=await connection.QueryAsync<OrderItem>(sql);
        return new ApiResponse<List<OrderItem>>(result.ToList());
    }

    public async Task<ApiResponse<OrderItem>> GetById(int id)
    {
        using var connection = context.Connection;
        string sql = "select * from orderitems where Id = @Id";
        var result = await connection.QuerySingleOrDefaultAsync<OrderItem>(sql, new { Id = id });
        if (result == null) return new ApiResponse<OrderItem>(HttpStatusCode.NotFound, "OrderItem not found");
        return new ApiResponse<OrderItem>(result);
    }

    public async Task<ApiResponse<bool>> Add(OrderItem data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     insert into orderitems(orderId, menuItemId) values (@orderId, @menuItemId);
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Update(OrderItem data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     update orderitems set orderId = @orderId, menuItemId = @menuItemId where Id = @Id;
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Delete(int id)
    {
        using var connection = context.Connection;
        string sql = "delete from orderitems where Id = @Id";
        var res = await connection.ExecuteAsync(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "OrderItem not found");
        return new ApiResponse<bool>(res != 0);
    }
    
    
    
}
