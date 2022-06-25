using СarCatalog.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace СarCatalog.Interfaces
{
    public interface ICarRepository
    {
        Task<Car> Create(Car car);
        Task Delete(int id);
        Task<Car> Get(int id);
        Task<IEnumerable<Car>> GetCars();
        Task Update(Car car);
    }
}
