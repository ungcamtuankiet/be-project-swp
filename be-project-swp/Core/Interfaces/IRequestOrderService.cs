using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IRequestOrderService
    {
        Task SendRequesrOrder(SendRequest sendRequest, string userName_Request, string userId_Receivier, string fullName);
        IEnumerable<ReceiveRequestDto> GetMineOrderByUserId(string user_Name);
        IEnumerable<RequestOrderDto> GetMineRequestByUserName(string user_Id);
    }
}
