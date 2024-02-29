using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IOptions<HomeOptions> options;
        private readonly IMapper mapper;

        // Инициализация конфигурации при вызове конструктора
        public HomeController(IOptions<HomeOptions> options, IMapper mapper)
        {
            this.options = options;
            this.mapper = mapper;
        }

        /// <summary>
        /// Метод для получения информации о доме
        /// </summary>
        [HttpGet]
        [Route("info")]
        public IActionResult Info()
        {
            // Получим запрос, смапив конфигурацию на модель запроса
            var infoResponse = mapper.Map<HomeOptions, InfoResponse>(options.Value);
            // Вернём ответ
            return StatusCode(200, infoResponse);
        }
    }
}