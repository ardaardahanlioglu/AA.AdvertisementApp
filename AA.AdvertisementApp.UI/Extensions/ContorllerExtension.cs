using AA.AdvertisementApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace AA.AdvertisementApp.UI.Extensions
{
    public static class ContorllerExtension
    {
        public static IActionResult ResponseRedirectAction<T>(this Controller controller,
            IResponse<T> response, string actionName, string contollerName="")
        {
            if (response.ResponseType == ResponseType.NotFound)
                return controller.NotFound();
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach (var error in response.ValidationErrors)
                {
                    controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return controller.View(response.Data);
            }
            if (string.IsNullOrWhiteSpace(contollerName))
            {
                return controller.RedirectToAction(actionName); 
            }
            else
            {
                return controller.RedirectToAction(actionName, contollerName);
            }
            
        }
        public static IActionResult ResponseView<T>(this Controller controller, IResponse<T> response)
        {
            if (response.ResponseType == ResponseType.NotFound)
                return controller.NotFound();
            return controller.View(response.Data);
        }
        public static IActionResult ResponseRedirectAction(this Controller controller,
            IResponse response, string actionName)
        {
            if (response.ResponseType == ResponseType.NotFound)
                return controller.NotFound();
            return controller.RedirectToAction(actionName);
            
        }
    }
}
