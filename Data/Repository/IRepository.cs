using Blog2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog2.Data.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        void RemovePost(int id);
        void AddPost(Post post);
        void UpdatePost(Post post);

        Task<bool> SaveChangesAsync();

    }
}
