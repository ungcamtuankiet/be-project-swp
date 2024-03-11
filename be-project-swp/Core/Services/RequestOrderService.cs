using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace be_artwork_sharing_platform.Core.Services
{
    public class RequestOrderService : IRequestOrderService
    {
        private readonly ApplicationDbContext _context;

        public RequestOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendRequesrOrder(SendRequest sendRequest, string userName_Request, string userId_Receivier, string fullName)
        {
            var request = new RequestOrder
            {
                FullName = fullName,
                UserName_Sender = userName_Request,
                UserId_Receivier = userId_Receivier,
                Email = sendRequest.Email,
                PhoneNumber = sendRequest.PhoneNumber,
                Category_Artwork = sendRequest.Category_Artwork,
                Text = sendRequest.Text
            };
            await _context.RequestOrders.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ReceiveRequestDto> GetMineOrderByUserId(string user_Id)
        {
            var receivier = _context.RequestOrders.Where(f => f.UserId_Receivier == user_Id)
                .Select(f => new ReceiveRequestDto
                {
                    FullName_Sender = f.FullName,
                    Email_Sender = f.Email,
                    PhoneNo_Sender = f.PhoneNumber,
                    Category_Artwork = f.Category_Artwork,
                    Text = f.Text,
                    CreatedAt = f.CreatedAt,
                    IsActive = f.IsActive,
                    IsDeleted = f.IsDeleted,
                }).ToList();
            return receivier;
        }

        public IEnumerable<RequestOrderDto> GetMineRequestByUserName(string user_Name)
        {
            var request = _context.RequestOrders.Where(f => f.UserName_Sender == user_Name)
                .Select(f => new RequestOrderDto
                {
                    FullName = f.FullName,
                    Email = f.Email,
                    PhoneNumber = f.PhoneNumber,
                    Category_Artwork = f.Category_Artwork,
                    Text = f.Text,
                    CreatedAt= f.CreatedAt,
                    IsActive = f.IsActive,
                    IsDeleted = f.IsDeleted,
                }).ToList();
            return request;
        }
    }
}
