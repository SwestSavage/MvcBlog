﻿namespace MvcBlog.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}