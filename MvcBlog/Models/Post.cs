namespace MvcBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public Category Сategory { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }
}
