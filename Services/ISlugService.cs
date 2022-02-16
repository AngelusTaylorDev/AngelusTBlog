using System;

namespace AngelusTBlog.Services
{
    public interface ISlugService
    {
        // returns a string that is URL friendly from the tittle - aka slug
        string UrlFriendly(string title);

        // Seet if the slug is unique or not 
        bool IsUnique(string slug);
    }
}
