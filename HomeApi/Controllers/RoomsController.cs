using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository repository;
        private readonly IMapper mapper;

        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await repository.GetRoomByName(request.Name);

            if (existingRoom == null)
            {
                var newRoom = mapper.Map<AddRoomRequest, Room>(request);
                await repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        /// <summary>
        /// Добавить или перезаписать комнату
        /// </summary>
        [HttpPut]
        [Route("put")]
        public async Task<IActionResult> Put([FromBody] PutRoomRequest request)
        {
            if (request.Id.HasValue)
            {
                var room = await repository.GetById(request.Id.Value);

                if (room == null)
                {
                    var newRoom = mapper.Map<PutRoomRequest, Room>(request);
                    await repository.AddRoom(newRoom);

                    return StatusCode(201);
                }

                await repository.UpdateRoom(room,
                    new UpdateRoomQuery(request.Id, request.Name, request.Area, request.GasConnected,
                        request.Voltage));

                return StatusCode(200);
            }
            else
            {
                var newRoom = mapper.Map<PutRoomRequest, Room>(request);

                await repository.AddRoom(newRoom);
                return StatusCode(201);
            }
        }
    }
}