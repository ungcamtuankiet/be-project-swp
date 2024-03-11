using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestOrderController : ControllerBase
    {
        private readonly IRequestOrderService _requestOrderService;
        private readonly ILogService _logService;
        private readonly IAuthService _authService;

        public RequestOrderController(IRequestOrderService requestOrderService, ILogService logService, IAuthService authService)
        {
            _requestOrderService = requestOrderService;
            _logService = logService;
            _authService = authService;
        }

        [HttpPost]
        [Route("send-request")]
        [Authorize]
        public async Task<IActionResult> SendRequestOrder(SendRequest sendRequest, string user_Id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                string userNameRequest = await _authService.GetCurrentUserName(userName);
                string fullNameResquest = await _authService.GetCurrentFullName(userName);
                if(userId == user_Id)
                {
                    return BadRequest(new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "You can not request you"
                    });
                }
                else
                {
                    _logService.SaveNewLog(userId, "Send Request Order");
                    _requestOrderService.SendRequesrOrder(sendRequest, userNameRequest, user_Id, fullNameResquest);
                    return Ok(new GeneralServiceResponseDto()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Send Request to Order Artwork Successfully"
                    });
                }
            }
            catch
            {
                return BadRequest("Send Request Failed");
            }
        }

        [HttpGet]
        [Route("get-mine-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetRequestOfMines()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string currentUserName = await _authService.GetCurrentUserName(userName);
                var result = _requestOrderService.GetMineRequestByUserName(currentUserName);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpGet]
        [Route("get-mine-order")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetOrderOfMines()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var result = _requestOrderService.GetMineOrderByUserId(userId);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }
    }
}
