using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class AttendenceResponseDTO
    {
        public required int StudentId { get; set; }
        public required int CourseId { get; set; }
        public required int StatusId { get; set; }
    }


    //add create dto
}