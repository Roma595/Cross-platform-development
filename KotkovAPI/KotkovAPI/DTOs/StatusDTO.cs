using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace KotkovAPI.DTOs
{
    public class StatusResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
    public class CreateStatusDTO
    {
        [MaxLength(45)]
        public required string Name { get; set; }
    }
    public class UpdateStatusDTO
    {
        [MaxLength(45)]
        public required string Name { get; set; }
    }

}