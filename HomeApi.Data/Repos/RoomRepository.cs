using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext context;

        public RoomRepository(HomeApiContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = context.Entry(room);
            if (entry.State == EntityState.Detached)
                await context.Rooms.AddAsync(room);

            await context.SaveChangesAsync();
        }

        public async Task<Room> GetById(Guid id)
        {
            return await context.Rooms
                .Where(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            if (!string.IsNullOrEmpty(query.Name))
                room.Name = query.Name;

            room.Area = query.Area;
            room.GasConnected = query.GasConnected;
            room.Voltage = query.Voltage;

            var entry = context.Entry(room);
            if (entry.State == EntityState.Detached)
                context.Rooms.Update(room);

            await context.SaveChangesAsync();
        }
    }
}