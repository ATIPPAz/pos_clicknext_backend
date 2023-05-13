using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosApi.Context;
using PosApi.Models;
using PosApi.Services;
using PosApi.ViewModels.ItemViewModel;
using PosApi.ViewModels.ReceiptViewModel;

namespace PosApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReceiptController : ControllerBase
    {
        readonly posContext _posContext;
        readonly ILogger logger;
        readonly ReceiptRepository receiptService;
        public ReceiptController(posContext posContext, ILogger<ReceiptController> logger)
        {
            _posContext = posContext;
            this.logger = logger;
            receiptService = new ReceiptRepository(posContext);
        }


        [HttpGet]
        public IActionResult getAllReceipt(string startDate = "", string endDate = "")
        {
            try
            {
                List<ReceiptAllResponse> result = receiptService.getAllReceipts(startDate, endDate);
                return new JsonResult(new ResponseObject<List<ReceiptAllResponse>>()
                {
                    data = result,
                    statusCode = Models.StatusCode.successReturn
                });
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult getOneReceipt(int receiptId)
        {
            try
            {
                ReceiptOneResponse result = receiptService.getOneReceipt(receiptId);
                return new JsonResult(new ResponseObject<ReceiptOneResponse>()
                {
                    data = result,
                    statusCode = Models.StatusCode.successReturn
                });
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult createReceipt([FromBody] CreateReceiptRequest itemData)
        {

            try
            {
                receipt newReceipt = new receipt()
                {
                    receiptCode = itemData.receiptCode,
                    receiptDate = itemData.receiptDate,
                    receiptTotalBeforeDiscount = itemData.receiptTotalBeforeDiscount,
                    receiptTotalDiscount = itemData.receiptTotalDiscount,
                    receiptSubTotal = itemData.receiptSubTotal,
                    receiptTradeDiscount = itemData.receiptTradeDiscount,
                    receiptGrandTotal = itemData.receiptGrandTotal
                };
                receiptService.createReceipts(newReceipt,itemData.receiptdetails);
                /*  int res = receiptService.createItem(newItem);*/
                return new JsonResult(new ResponseObject()
                {
                    data = new object(),
                    statusCode = Models.StatusCode.created
                });
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }


        }

        [HttpGet]
        public IActionResult test()
        {
           /* var x = from _receiptDetail in _posContext.receiptdetails
                    where _receiptDetail.receiptId == 7
                    join _unit in _posContext.units on _receiptDetail.unitId equals _unit.unitId
                    join _item in _posContext.items on _receiptDetail.itemId equals _item.itemId
                    select new
                    {
                        receiptId = _receiptDetail.receiptId,
                        receiptDetailId = _receiptDetail.receiptDetailId,
                        itemAmount = _receiptDetail.itemAmount,
                        itemDiscount = _receiptDetail.itemDiscount,
                        itemDiscountPercent = _receiptDetail.itemDiscountPercent,
                        itemName = _item.itemName,
                        itemCode = _item.itemCode,
                        unitName = _unit.unitName,
                        itemQty = _receiptDetail.itemQty,
                        itemPrice = _receiptDetail.itemPrice
                    };*/
           var x = from _receipt in _posContext.receipts
         
                   join data in (from _receiptDetail in _posContext.receiptdetails
                                 join _unit in _posContext.units on _receiptDetail.unitId equals _unit.unitId
                                 join _item in _posContext.items on _receiptDetail.itemId equals _item.itemId
                                 where _receiptDetail.receiptId == 7
                                 select new
                                 {
                                     receiptId = _receiptDetail.receiptId,
                                     receiptDetailId = _receiptDetail.receiptDetailId,
                                     itemAmount = _receiptDetail.itemAmount,
                                     itemDiscount = _receiptDetail.itemDiscount,
                                     itemDiscountPercent = _receiptDetail.itemDiscountPercent,
                                     itemName = _item.itemName,
                                     itemCode = _item.itemCode,
                                     unitName = _unit.unitName,
                                     itemQty = _receiptDetail.itemQty,
                                     itemPrice = _receiptDetail.itemPrice
                                 }

                                 ) on _receipt.receiptId equals data.receiptId into _receiptListItem
                   where _receipt.receiptId == 7
                   select new
                   {
                       receiptId = _receipt.receiptId,
                       receiptDate = _receipt.receiptDate,
                       receiptGrandTotal = _receipt.receiptGrandTotal,
                       receiptCode = _receipt.receiptCode,
                       receiptTotalBeforeDiscount = _receipt.receiptTotalBeforeDiscount,
                       receiptTotalDiscount = _receipt.receiptTotalDiscount,
                       receiptSubTotal = _receipt.receiptSubTotal,
                       receiptdetails = _receiptListItem,
                       receiptTradeDiscount = _receipt.receiptTradeDiscount
                   };

            return Ok(x.ToList());
        }
    }
}
