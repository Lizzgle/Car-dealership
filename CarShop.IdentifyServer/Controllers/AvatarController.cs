using CarShop.IdentifyServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CarShop.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AvatarController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _defaultAvatar;
        private readonly ILogger<AvatarController> _logger;

        public AvatarController(UserManager<ApplicationUser> userManager,
                            IWebHostEnvironment environment,
                            IConfiguration configuration,
                            ILogger<AvatarController> logger)
        {
            _environment = environment;
            _userManager = userManager;
            _defaultAvatar = configuration.GetSection("DefaultAvatarName").Value;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation($"_environment.WebRootPath: {_environment.WebRootPath}");

            var id = _userManager.GetUserId(User);
            if (id is null)
                return BadRequest("User not found");

            string searcPattern = $@"{id}.*";
            var files = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "Images"), searcPattern);

            string imagePath;
            if (files.Any())
            {
                imagePath = files[0];
            }
            else
            {
                imagePath = Path.Combine(_environment.WebRootPath, "Images", _defaultAvatar);
            }

            FileStream fs = new(imagePath, FileMode.Open);
            string ext = Path.GetExtension(imagePath);

            var extProvider = new FileExtensionContentTypeProvider();
            return File(fs, extProvider.Mappings[ext]);

        }
    }
}
