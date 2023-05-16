using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosApi.Context;
using PosApi.helpers;
using PosApi.Models;
using PosApi.Services;
using PosApi.ViewModels.ItemViewModel;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PosApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ItemController : ControllerBase
    {

        readonly ILogger logger;
        readonly ItemRepository itemService;
        ResponseHelper responseHelper = new ResponseHelper();
        public ItemController(ItemRepository itemService, ILogger<ItemController> logger)
        {
            this.logger = logger;
            this.itemService = itemService;
        }


        [HttpGet]
        public IActionResult getItems()
        {
            try
            {
                List<ItemResponse> items = itemService.getAllItems();
                return responseHelper.JsonGet<List<ItemResponse>>(items);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return responseHelper.JsonError();
            }
        }

        [HttpPost]
        public IActionResult updateItem([FromBody] UpdateItemRequest req)
        {
            if (req.itemPrice < 0) return responseHelper.JsonError();
            if (req.unitId < 0) return responseHelper.JsonError();
            if (string.IsNullOrEmpty(req.itemName)) return responseHelper.JsonError();
            if (req.itemId < 0) return responseHelper.JsonError();
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    itemService.updateItem(req);
                    transaction.Complete();
                    return responseHelper.JsonUpdate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                    transaction.Dispose();
                    return responseHelper.JsonError();
                }
            }
        }

        [HttpPost]
        public IActionResult deleteItem([FromBody] DeleteItemRequest req)
        {
            if (req.itemId <= 0) return responseHelper.JsonError();
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    itemService.deleteItem(req.itemId);
                    transaction.Complete();
                    return responseHelper.JsonDelete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                    transaction.Dispose();
                    return responseHelper.JsonError();
                }

            }
        }

        [HttpPost]
        public IActionResult createItem([FromBody] ItemCreateRequest itemData)
        {
            if (string.IsNullOrEmpty(itemData.itemCode)) return responseHelper.JsonError();
            if (itemData.itemPrice < 0) return responseHelper.JsonError();
            if (itemData.unitId <= 0) return responseHelper.JsonError();
            if (string.IsNullOrEmpty(itemData.itemName)) return responseHelper.JsonError();
            using (TransactionScope transaction = new TransactionScope())
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
                    transaction.Complete();
                    return responseHelper.JsonCreate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                    transaction.Dispose();
                    return responseHelper.JsonError();
                }
            }
        }
    }
}
