using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер устройсив
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IMapper mapper;

        public DevicesController(IDeviceRepository deviceRepository,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            this.deviceRepository = deviceRepository;
            this.roomRepository = roomRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Просмотр списка подключенных устройств
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetDevices()
        {
            var devices = await deviceRepository.GetDevices();

            var resp = new GetDevicesResponse
            {
                DeviceAmount = devices.Length,
                Devices = mapper.Map<Device[], DeviceView[]>(devices)
            };

            return StatusCode(200, resp);
        }

        /// <summary>
        /// Удаление устройства
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var device = await deviceRepository.GetDeviceById(id);

            if (device == null)
                return StatusCode(400, $"Ошибка: Устройство с идентификатором {id} не существует.");

            await deviceRepository.DeleteDevice(device);

            return StatusCode(200, $"Устройство {device.Name} удалено.");
        }

        /// <summary>
        /// Добавление нового устройства
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add(AddDeviceRequest request)
        {
            var room = await roomRepository.GetRoomByName(request.RoomLocation);

            if (room == null)
                return StatusCode(400,
                    $"Ошибка: Комната {request.RoomLocation} не подключена. Сначала подключите комнату!");

            var device = await deviceRepository.GetDeviceByName(request.Name);

            if (device != null)
                return StatusCode(400, $"Ошибка: Устройство {request.Name} уже существует.");

            var newDevice = mapper.Map<AddDeviceRequest, Device>(request);
            await deviceRepository.SaveDevice(newDevice, room);

            return StatusCode(201, $"Устройство {request.Name} добавлено. Идентификатор: {newDevice.Id}");
        }

        /// <summary>
        /// Обновление существующего устройства
        /// </summary>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] EditDeviceRequest request)
        {
            var room = await roomRepository.GetRoomByName(request.NewRoom);

            if (room == null)
                return StatusCode(400, $"Ошибка: Комната {request.NewRoom} не подключена. Сначала подключите комнату!");

            var device = await deviceRepository.GetDeviceById(id);

            if (device == null)
                return StatusCode(400, $"Ошибка: Устройство с идентификатором {id} не существует.");

            var withSameName = await deviceRepository.GetDeviceByName(request.NewName);

            if (withSameName != null)
                return StatusCode(400,
                    $"Ошибка: Устройство с именем {request.NewName} уже подключено. Выберите другое имя!");

            await deviceRepository.UpdateDevice(
                device,
                room,
                new UpdateDeviceQuery(request.NewName, request.NewSerial)
            );

            return StatusCode(200,
                $"Устройство обновлено! Имя - {device.Name}, Серийный номер - {device.SerialNumber},  Комната подключения - {device.Room.Name}");
        }
    }
}