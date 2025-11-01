using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KotkovAPI.Data.Services
{
    public class StatusService
    {
        private readonly KotkovAPIContext _context;

        public StatusService(KotkovAPIContext context)
        {
            _context = context;
        }
        private static StatusResponseDTO StatusToResponseDTO(Status status) =>
            new StatusResponseDTO
            {
                Id = status.Id,
                Name = status.Name
            };
        private static UpdateStatusDTO StatusToUpdateDTO(Status status) => new UpdateStatusDTO
        {
            Name = status.Name
        };
        public IEnumerable<StatusResponseDTO> GetAll()
        {
            return _context.Statuses.Select(s => StatusToResponseDTO(s)).ToList();
        }
        public StatusResponseDTO? GetById(int id)
        {
            var status = _context.Statuses
                .FirstOrDefault(s => s.Id == id);
            if (status == null)
            {
                return null;
            }
            return StatusToResponseDTO(status);
        }
        public StatusResponseDTO Create(CreateStatusDTO statusDTO)
        {
            var status = new Status
            {
                Name = statusDTO.Name
            };

            _context.Statuses.Add(status);
            _context.SaveChanges();

            return StatusToResponseDTO(status);
        }
        public StatusResponseDTO? Update(int id, UpdateStatusDTO statusDTO)
        {
            var existingStatus = _context.Statuses.Find(id);
            if (existingStatus != null)
            {
                existingStatus.Name = statusDTO.Name;

                _context.SaveChanges();
                return StatusToResponseDTO(existingStatus);
            }
            return null;
        }
        public bool Delete(int id)
        {
            var status = _context.Statuses.Find(id);
            if (status != null)
            {
                try
                {
                    _context.Statuses.Remove(status);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            return true;
        }
    }
}