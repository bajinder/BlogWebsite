using Blog2.Data.FileManager;
using Blog2.Data.Repository;
using Blog2.Models;
using Blog2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repo,IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;

        }
        
        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }
        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new PostViewModel()
                {
                    Id = post.Id,
                    Body = post.Body,
                    Title = post.Title,
                    CurrentImage = post.Image           // Send the current image to UI
                }) ;
                ;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post()
            {
                Id = vm.Id,
                Body = vm.Body,
                Title = vm.Title,

                Image = (vm.Image != null) ? await _fileManager.SaveImage(vm.Image) : vm.CurrentImage
            };
            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Index");
            else
                return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id > 0)
            {
                _repo.RemovePost((int)id);
                await _repo.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
