using System.Net;
using Dapper;
using Domain.Entities;
using Infrastructure.DataContext;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class OrderService(DapperContext context):IGenericService<Order>
{
    public async Task<ApiResponse<List<Order>>> GetAll()
    {
        using var connection = context.Connection;
        var sql="select * from orderitems";
        var result=await connection.QueryAsync<Order>(sql);
        return new ApiResponse<List<Order>>(result.ToList());
    }

    public async Task<ApiResponse<Order>> GetById(int id)
    {
        using var connection = context.Connection;
        string sql = "select * from orders where Id = @Id";
        var result = await connection.QuerySingleOrDefaultAsync<Order>(sql, new { Id = id });
        if (result == null) return new ApiResponse<Order>(HttpStatusCode.NotFound, "Order not found");
        return new ApiResponse<Order>(result);
    }

    public async Task<ApiResponse<bool>> Add(Order data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     insert into orders(customerId, tableId, status) values (@customerId, @tableId, @status);
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Update(Order data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     update orders set customerId = @tableId, status = @status where Id = @Id;
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Delete(int id)
    {
        using var connection = context.Connection;
        string sql = "delete from orders where Id = @Id";
        var res = await connection.ExecuteAsync(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "Order not found");
        return new ApiResponse<bool>(res != 0);
    }
    
    //order is pending
    public async Task<ApiResponse<List<Order>>> GetPendingOrders()
    {
        using var connection = context.Connection;
        string sql = @"""
        SELECT * 
        FROM Orders 
        WHERE Status = 'Pending';
    """;
        var result = await connection.QueryAsync<Order>(sql);
        return new ApiResponse<List<Order>>(result.ToList());
    }

    
    //total sum of orders
    public async Task<ApiResponse<decimal>> GetTotalOrderSumByCustomerId(int customerId)
    {
        using var connection = context.Connection;
        string sql = @"""
        select sum(TotalAmount) 
        FROM Orders 
        WHERE CustomerId = @CustomerId;
    """;
        var result = await connection.ExecuteScalarAsync<decimal>(sql, new { CustomerId = customerId });
        return new ApiResponse<decimal>(result);
    }
}