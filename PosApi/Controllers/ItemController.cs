using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosApi.Context;
using PosApi.Models;
using PosApi.Services;
using PosApi.ViewModels.ItemViewModel;

namespace PosApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ItemController : ControllerBase
    {
        readonly posContext _posContext;
        readonly ILogger logger;
        readonly ItemRepository itemService;
        public ItemController(posContext posContext, ILogger<ItemController> logger)
        {
            _posContext = posContext;
            this.logger = logger;
            itemService = new ItemRepository(posContext);
        }


        [HttpGet]
        public IActionResult getItems()
        {
            try
            {
                List<ItemResponse> items = itemService.getAllItems();
                return new JsonResult(new ResponseObject<List<ItemResponse>>()
                {
                    data = items,
                    statusCode = Models.StatusCode.successReturn
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new JsonResult(new ResponseObject<Error>()
                {
                    data = new Error() { message = "server error" },
                    statusCode = Models.StatusCode.error
                });
            }

           
        }

        [HttpPost]
        public IActionResult updateItem([FromBody] UpdateItemRequest req)
        {
            try
            {
                itemService.updateItem(req);
                return new JsonResult(new ResponseObject()
                {
                    data = new object(),
                    statusCode = Models.StatusCode.successNoReturn
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new JsonResult(new ResponseObject<Error>()
                {
                    data =  new Error() { message="server error"},
                    statusCode = Models.StatusCode.error
                });
            }
           
        }

        [HttpPost]
        public IActionResult deleteItem([FromBody] DeleteItemRequest req)
        {
            try
            {
                itemService.deleteItem(req.itemId);
                return new JsonResult(new ResponseObject()
                {
                    data = new object(),
                    statusCode = Models.StatusCode.successNoReturn
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new JsonResult(new ResponseObject<Error>()
                {
                    data = new Error() { message = "server error" },
                    statusCode = Models.StatusCode.error
                });
            }
           
        }

        [HttpPost]
        public IActionResult createItem([FromBody] ItemCreateRequest itemData)
        {
            try
            {
                item newItem = new item()
                {
                    itemName = itemData.itemName,
                    itemCode = itemData.itemCode,
                    itemPrice = itemData.itemPrice,
                    unitId = itemData.unitId
                };
                int res = itemService.createItem(newItem);
                return new JsonResult(new ResponseObject()
                {
                    data = new object(),
                    statusCode = Models.StatusCode.created
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new JsonResult(new ResponseObject<Error>()
                {
                    data = new Error() { message = "server error" },
                    statusCode = Models.StatusCode.error
                });
            }


        }
    }
}
