using KotkovAPI.DTOs;
using KotkovAPI.Models;

namespace KotkovAPI.Data.Services
{
    public class StatusService
    {
        private readonly KotkovAPIContext _context;

        public StatusService(KotkovAPIContext context)
        {
            _context = context;
        }
        private static StatusDTO StatusToDTO(Status status) =>
            new StatusDTO
            {
                Id = status.Id,
                Name = status.Name
            };
        public IEnumerable<StatusDTO> GetAll()
        {
            return _context.Statuses.Select(s => StatusToDTO(s)).ToList();
        }
        public StatusDTO? GetById(int id)
        {
            var status = _context.Statuses
                .FirstOrDefault(s => s.Id == id);
            if (status == null)
            {
                return null;
            }
            return StatusToDTO(status);
        }
        public Status Create(StatusDTO statusDTO)
        {
            var status = new Status
            {
                Name = statusDTO.Name
            };

            _context.Statuses.Add(status);
            _context.SaveChanges();

            return status;
        }
        public StatusDTO? Update(int id, StatusDTO statusDTO)
        {
            var existingStatus = _context.Statuses.Find(id);
            if (existingStatus != null)
            {
                existingStatus.Name = statusDTO.Name;

                _context.SaveChanges();
                return StatusToDTO(existingStatus);
            }
            return null;
        }

        public void Delete(int id)
        {
            var status = _context.Statuses.Find(id);
            if (status != null)
            {
                _context.Statuses.Remove(status);
                _context.SaveChanges();
            }
        }

        
    }
}