using Microsoft.AspNetCore.Mvc;
using СarCatalog.Services;
using System.Threading.Tasks;
using СarCatalog.Models;
using СarCatalog.Interfaces;

namespace СarCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ICarRepository _repo { get; }

        public ValuesController(ICarRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var result = await _repo.GetCars();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _repo.Get(id);
            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Сreate([FromBody] Car car)
        {
            var newCar = await _repo.Create(car);
            return Ok(newCar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Car car)
        {
            var usedCar = await _repo.Get(car.Id);
            if (usedCar == null)
            {
                return NotFound();
            }

            await _repo.Update(car);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usedCar = await _repo.Get(id);
            if (usedCar == null)
            {
                return NotFound();
            }

            await _repo.Delete(id);
            return NoContent();
        }
    }
}
