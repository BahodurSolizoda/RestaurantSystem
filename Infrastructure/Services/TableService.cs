using System.Net;
using Dapper;
using Domain.Entities;
using Infrastructure.DataContext;
using Infrastructure.Interfaces;
using Infrastructure.Responses;

namespace Infrastructure.Services;

public class TableService(DapperContext context):IGenericService<Table>
{
    public async Task<ApiResponse<List<Table>>> GetAll()
    {
        using var connection = context.Connection;
        var sql="select * from tables";
        var result=await connection.QueryAsync<Table>(sql);
        return new ApiResponse<List<Table>>(result.ToList());
    }

    public async Task<ApiResponse<Table>> GetById(int id)
    {
        using var connection = context.Connection;
        string sql = "select * from tables where Id = @Id";
        var result = await connection.QuerySingleOrDefaultAsync<Table>(sql, new { Id = id });
        if (result == null) return new ApiResponse<Table>(HttpStatusCode.NotFound, "Table not found");
        return new ApiResponse<Table>(result);
    }

    public async Task<ApiResponse<bool>> Add(Table data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     insert into tables(tableNumber, IsOccupied) values (@tableNumber, @IsOccupied);
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Update(Table data)
    {
        using var connection = context.Connection;
        string sql = @"""
                     update tables set tableNumber = @tableNumber, IsOccupied = @IsOccupied where Id = @Id;
                     """;
        var res = await connection.ExecuteAsync(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public async Task<ApiResponse<bool>> Delete(int id)
    {
        using var connection = context.Connection;
        string sql = "delete from tables where Id = @Id";
        var res = await connection.ExecuteAsync(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "Table not found");
        return new ApiResponse<bool>(res != 0);
    }
}