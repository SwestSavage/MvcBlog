using System.ComponentModel.DataAnnotations;

namespace MvcBlog.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile UploadedFile { get; set; }
        [Required]
        public int СategoryId { get; set; }
        public List<int> TagsIds { get; set; }
    }
}
