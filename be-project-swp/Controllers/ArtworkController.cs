using AutoMapper;
using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.Category;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworkController : ControllerBase
    {
        private readonly IArtworkService _artworkService;
        private readonly IAuthService _authService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public ArtworkController(IArtworkService artworkService, IAuthService authService, ILogService logService, IMapper mapper, ICategoryService categoryService)
        {
            _artworkService = artworkService;
            _authService = authService;
            _logService = logService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var artworks = await _artworkService.GetAll();
            if (artworks is null)
                return null;
            return Ok(artworks);
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(string? search,string? searchBy, double? from, double? to, string? sortBy)
        {
            var artworks = await _artworkService.SearchArtwork(search, searchBy, from, to, sortBy);
            if (artworks is null)
                return NotFound("Artworks not available");
            return Ok(_mapper.Map<List<ArtworkDto>>(artworks));
        }

        [HttpGet]
        [Route("get-by-userId")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetByUserIdAsync()
        {
            string userName = HttpContext.User.Identity.Name;
            string userId = await _authService.GetCurrentUserId(userName);
            var artworks = _artworkService.GetArtworkByUserId(userId);
            return Ok(_mapper.Map<List<ArtworkDto>>(artworks));

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var artwork = await _artworkService.GetById(id);
                if(artwork is null)
                    return NotFound(new GeneralServiceResponseDto
                    {
                        IsSucceed = false,
                        StatusCode = 204,
                        Message = "Artwork not found"
                    });
                else
                {
                    return Ok(_mapper.Map<ArtworkDto>(artwork));
                }
            }
            catch
            {
                return BadRequest("Somethong wrong");
            }
        }

        

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> Create(CreateArtwork artworkDto)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                string userNameCurrent = await _authService.GetCurrentUserName(userName);
                await _artworkService.CreateArtwork(artworkDto, userId, userNameCurrent);
                await _logService.SaveNewLog(userName, "Create New Artwork");
                return Ok(new GeneralServiceResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 203,
                    Message = "Create new Artwork Successfully"
                });
            }
            catch
            {
                return BadRequest("Create new Artwork Failed");
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var result = _artworkService.Delete(id);
                if(result > 0)
                {
                    _logService.SaveNewLog(userName, "Delete Artwork");
                    return Ok(new GeneralServiceResponseDto
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Delete Artwork Successfully"
                    });
                }
                else
                {
                    return BadRequest(new GeneralServiceResponseDto
                    {
                        IsSucceed = true,
                        StatusCode = 400,
                        Message = "Delete Artwork Failed"
                    });
                }
            }
            catch
            {
                return BadRequest("Somethong wrong");
            }
        }

        [HttpPut]
        [Route("update-artwork")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> UpdateArtwork(long id, UpdateArtwork artworkDt)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                string userNameCurrent = await _authService.GetCurrentUserName(userName);
                await _logService.SaveNewLog(userName, "Update Artwork");
                await _artworkService.UpdateArtwork(id, artworkDt);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
