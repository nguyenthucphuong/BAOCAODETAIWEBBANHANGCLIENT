using System.Net.Http.Headers;

namespace ClientSaleApi.Models.Authentication
{
	//DelegatingHandler là abtract cho phép thêm token vào header vào mỗi yêu cầu trước khi gởi
	public class AuthorizationHeaderHandler : DelegatingHandler
	{
		//IHttpContextAccessor là interface trong cho phép truy cập vào HttpContext hiện tại
		//HttpContext chứa tất cả thông tin về yêu cầu HTTP hiện tại, bao gồm header, cookie...
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		// CancellationToken là một đối tượng để thông báo hủy bỏ một hoạt động.
		// Truyền CancellationToken vào một phương thức để kiểm tra xem token đã được hủy chưa và ngừng hoạt động nếu cần.
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext != null)
			{
				var token = httpContext.Request.Cookies["Token"];
				if (!string.IsNullOrEmpty(token))
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				}
			}
			// Nếu CancellationToken được hủy, hàm SendAsync sẽ ngừng hoạt động và ném ra một ngoại lệ OperationCanceledException
			return await base.SendAsync(request, cancellationToken);
		}
	}
}
