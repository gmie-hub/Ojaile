using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ojaile.Abstraction;

namespace Ojaile_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyItemController : ControllerBase
    {
        private readonly IPropertyItemService _propertyItem;
        public PropertyItemController(IPropertyItemService propertyItem)
        {
            _propertyItem = propertyItem;

        }

        [HttpGet]
        public IActionResult GetPropertyItem()
        {
            List<object> items = new List<object>();
            if(_propertyItem == null)
            {
                return NotFound();
            }
            else
            {
                _propertyItem.GetPropertyItem();
            }
            return Ok(items);
        }
    }
}
