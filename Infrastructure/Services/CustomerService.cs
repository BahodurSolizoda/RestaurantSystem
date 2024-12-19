using System.Net;
using Dapper;
using Domain.Entities;
using Infrastructure.DataContext;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class CustomerService(DapperContext context):IGenericService<Customer>
{
    public async Task<ApiResponse<List<Customer>>> GetAll()
    {
        using var connection = context.Connection;
        var sql="select * from customers";
        var result=await connection.QueryAsync<Customer>(sql);
        return new ApiResponse<List<Customer>>(result.ToList());
    }

    public async Task<ApiResponse<Customer>> GetById(int id)
    {
        using var connection = context.Connection;
        string sql = "select * from customers where Id = @Id";
        var result = await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });
        if (result == null) return new ApiResponse<Customer>(HttpStatusCode.NotFound, "Customer not found");
        return new ApiResponse<Customer>(result);

    }

    public async Task<ApiResponse<bool>> Add(Customer data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     insert into customers(name, phonenumber)
                                    values(@name, @phonenumber);
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Update(Customer data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     update customers set name = @name, phonenumber = @phonenumber where Id = @Id;
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Delete(int id)
    {
        using var connection = context.Connection;
        string sql = "delete from customers where Id = @Id";
        var res = await connection.ExecuteAsync(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "Customer not found");
        return new ApiResponse<bool>(res != 0);
    }
    
   

}