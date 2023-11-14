namespace CarShop.Extensions
{
    public static class AjaxRequestExtension
    {

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                return false;
            }
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
