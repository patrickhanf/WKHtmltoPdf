namespace Prh.Controllers
{
    [Route("api/[controller]")]
    public class TestingController : BaseControllerPrivate<TestingController>
    {
        public TestingController(IMemoryCache _cache, IControlSettings controllerSettings) : base(_cache, controllerSettings)
        {

        }

        [HttpPost]
        [Route("GeneratePdf")]
        [AllowAnonymous]
        public IActionResult GeneratePdf(string html)
        {

        var pdfBytes =  WKHtmltoPdf.GeneratePdf(html, new System.Drawing.Size(800, 1024)).Result;
        return File(pdfBytes, "application/pdf", "demo.pdf");
        
        }
	}
}