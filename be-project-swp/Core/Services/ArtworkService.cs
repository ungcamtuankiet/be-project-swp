using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace be_artwork_sharing_platform.Core.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ArtworkService(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<ArtworkDto>> GetAll()
        {
            var artworks = await _context.Artworks
                .Select(a => new ArtworkDto
                {
                    Id = a.Id,
                    User_Id = a.User_Id,
                    User_Name = a.User_Name,
                    Name = a.User_Name,
                    Description = a.Description,
                    Url_Image = a.Url_Image,
                    Price = a.Price,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsActive = a.IsActive,
                    IsDeleted = a.IsDeleted,
                }).ToListAsync();
            return artworks;
        }

        public async Task<IEnumerable<Artwork>> SearchArtwork(string? search,string? searchBy, double? from, double? to, string? sortBy)
        {
            var artworks = _context.Artworks.Include(a => a.Category).AsQueryable();
            #region Filter
            if(searchBy is null)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    artworks = artworks.Where(a => a.Name.Contains(search));
                }
            }
            if(searchBy is not null)
            {
                if (searchBy.Equals("category_name"))
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        artworks = artworks.Where(a => a.Category_Name.Contains(search));
                    }
                }
                else if (searchBy.Equals("user_name"))
                    if (!string.IsNullOrEmpty(search))
                    {
                        artworks = artworks.Where(a => a.User_Name.Contains(search));
                    }
            }
            if (from.HasValue)
            {
                artworks = artworks.Where(a => a.Price >= from);
            }
            if (to.HasValue)
            {
                artworks = artworks.Where(a => a.Price <= to);
            }
            #endregion

            #region Sorting
            artworks = artworks.OrderBy(a => a.Name);

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "price_asc":
                        artworks = artworks.OrderBy(a => a.Price);
                        break;
                    case "price_desc":
                        artworks = artworks.OrderByDescending(a => a.Price);
                        break;
                }
            }
            #endregion
            return artworks.ToList();
        }

        public async Task<IEnumerable<Artwork>> GetArtworkByUserId(string user_Id)
        {
            var artworks = _context.Artworks.Where(a => a.User_Id == user_Id);
            if (artworks is null)
                return null;
            return artworks.ToList();
        }

        public async Task<Artwork> GetById(long id)
        {
            return _context.Artworks.Find(id) ?? throw new Exception("Artwork not found");
        }

        public async Task CreateArtwork(CreateArtwork artworkDto, string user_Id, string user_Name)
        {
            var artwork = new Artwork
            {
                User_Id = user_Id,
                User_Name = user_Name,
                Category_Name = artworkDto.Category_Name,
                Name = artworkDto.Name,
                Description = artworkDto.Description,
                Price = artworkDto.Price,
                Url_Image = artworkDto.Url_Image,
            };
            
            await _context.Artworks.AddAsync(artwork);
            await _context.SaveChangesAsync();
        }

        public int Delete(long id)
        {
            var artwork = _context.Artworks.Find(id) ?? throw new Exception("Artwork not found");
            _context.Remove(artwork);
            return _context.SaveChanges();
        }

        public async Task UpdateArtwork(long id, UpdateArtwork updateArtwork)
        {
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);
            if(artwork is not null)
            {
                artwork.Name = updateArtwork.Name;
                artwork.Category_Name = updateArtwork.Category_Name;
                artwork.Description = updateArtwork.Description;
                artwork.Url_Image = updateArtwork.Url_Image;
                artwork.Price = updateArtwork.Price;
            }
            _context.Update(artwork);
            _context.SaveChanges();
        }
    }
}
