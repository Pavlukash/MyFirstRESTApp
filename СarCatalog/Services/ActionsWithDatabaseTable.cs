using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using СarCatalog.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using СarCatalog.Interfaces;
using СarCatalog.Contexts;
using СarCatalog.ResponseException;

namespace СarCatalog.Services
{
    public class ActionsWithDatabaseTable : ICarRepository
    {
        private readonly DapperContext _context;

        public ActionsWithDatabaseTable(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetCars()
        {
            using (var connection = _context.CreateConnection())
            {
                var cars = await connection.QueryAsync<Car>("SELECT * FROM Cars");
                return cars.ToList();
            }
        }

        public async Task<Car> Get(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Car>("SELECT * FROM Cars WHERE Id = @id", new { id });
            }
        }

        public async Task<Car> Create(Car car)
        {
            var sqlQuery = "INSERT INTO Cars (Brand, Model, CarType, Setup, Price) VALUES(@Brand, @Model, @CarType, @Setup, @Price)";

            var parameters = new DynamicParameters();
            parameters.Add("brand", car.Brand);
            parameters.Add("model", car.Model);
            parameters.Add("cartype", car.CarType);
            parameters.Add("setup", car.Setup);
            parameters.Add("price", car.Price);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteAsync(sqlQuery, parameters);

                var createdCar = new Car
                {
                    Id = id,
                    Brand = car.Brand,
                    Model = car.Model,
                    Setup = car.Setup,
                    Price = car.Price,
                    CarType = car.CarType,
                };
                
                return createdCar;
            }
        }

        public async Task Update(Car car)
        {
            var sqlQuery = "UPDATE Cars SET Brand = @Brand, Model = @Model, CarType = @CarType, Setup = @Setup, Price = @Price WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", car.Id);
            parameters.Add("brand", car.Brand);
            parameters.Add("model", car.Model);
            parameters.Add("cartype", car.CarType);
            parameters.Add("setup", car.Setup);
            parameters.Add("price", car.Price);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sqlQuery, parameters);
            }
        }

        public async Task Delete(int id)
        {
            var sqlQuery = "DELETE FROM Cars WHERE Id = @id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sqlQuery, new { id });
            }
        }
    }
}

