using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TolidiAyhan.Pages.Error
{
    public class ErrorPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect($"/Login");
        }
    }
}
