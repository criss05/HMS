using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IRoomRepository _roomRepository;

        public EquipmentController(IEquipmentRepository equipmentRepository, IRoomRepository roomRepository)
        {
            _equipmentRepository = equipmentRepository;
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Get all equipment.
        /// </summary>
        /// <returns>A list of equipment.</returns>
        /// <response code="200">Returns the list of equipment</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Equipment>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var equipment = await _equipmentRepository.GetAllAsync();
            return Ok(equipment);
        }

        /// <summary>
        /// Get equipment by ID.
        /// </summary>
        /// <param name="id">The ID of the equipment.</param>
        /// <returns>The requested equipment.</returns>
        /// <response code="200">Returns the equipment</response>
        /// <response code="404">If the equipment is not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Equipment), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            if (equipment == null)
                return NotFound();

            return Ok(equipment);
        }

        /// <summary>
        /// Create new equipment.
        /// </summary>
        /// <param name="dto">The equipment data transfer object.</param>
        /// <returns>The created equipment.</returns>
        /// <response code="201">Returns the newly created equipment</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Equipment), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(EquipmentDto dto)
        {
            var rooms = new List<Room>();
            foreach (int roomId in dto.RoomIds)
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                    return BadRequest($"Room with ID {roomId} not found.");
                rooms.Add(room);
            }

            var equipment = new Equipment
            {
                Name = dto.Name,
                Specification = dto.Specification,
                Type = dto.Type,
                Stock = dto.Stock,
                Rooms = rooms
            };

            await _equipmentRepository.AddAsync(equipment);
            return CreatedAtAction(nameof(GetById), new { id = equipment.Id }, equipment);
        }


        /// <summary>
        /// Update existing equipment.
        /// </summary>
        /// <param name="id">The ID of the equipment to update.</param>
        /// <param name="dto">The updated equipment data.</param>
        /// <response code="204">Successfully updated</response>
        /// <response code="404">If the equipment is not found</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, EquipmentDto dto)
        {
            var existing = await _equipmentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var rooms = new List<Room>();
            foreach (int roomId in dto.RoomIds)
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                    return BadRequest($"Room with ID {roomId} not found.");
                rooms.Add(room);
            }

            existing.Name = dto.Name;
            existing.Specification = dto.Specification;
            existing.Type = dto.Type;
            existing.Stock = dto.Stock;
            existing.Rooms = rooms;

            await _equipmentRepository.UpdateAsync(existing);
            return NoContent();
        }


        /// <summary>
        /// Delete equipment by ID.
        /// </summary>
        /// <param name="id">The ID of the equipment to delete.</param>
        /// <response code="204">Successfully deleted</response>
        /// <response code="404">If the equipment is not found</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _equipmentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _equipmentRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
