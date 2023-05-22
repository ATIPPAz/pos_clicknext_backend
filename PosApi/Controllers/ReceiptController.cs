using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PosApi.Context;
using PosApi.helpers;
using PosApi.Models;
using PosApi.Services;
using PosApi.ViewModels;
using PosApi.ViewModels.ItemViewModel;
using PosApi.ViewModels.ReceiptViewModel;
using System.Transactions;

namespace PosApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReceiptController : ControllerBase
    {
        readonly ResponseHelper responseHelper = new ResponseHelper();
        readonly ILogger logger;
        readonly ReceiptRepository receiptService;
        public ReceiptController(ILogger<ReceiptController> logger, ReceiptRepository receiptService)
        {
            this.logger = logger;
            this.receiptService = receiptService;
        }

        [HttpGet]
        public IActionResult getAllReceipt(string startDate = "", string endDate = "")
        {

            try
            {
                List<ReceiptAllResponse> result = receiptService.getAllReceipts(startDate, endDate);
                return responseHelper.JsonGet<List<ReceiptAllResponse>>(result);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return responseHelper.JsonError();
            }
        }

        [HttpGet]
        public IActionResult getOneReceipt(int receiptId)
        {
            if (receiptId <= 0) return responseHelper.JsonError();
            try
            {
                ReceiptOneResponse result = receiptService.getOneReceipt(receiptId);
                return responseHelper.JsonGet<ReceiptOneResponse>(result);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return responseHelper.JsonError();
            }
        }

        [HttpGet]
        public IActionResult getPrefix()
        {
            try
            {
                return responseHelper.JsonGet<PrefixViewModel>(receiptService.getPrefix());
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return responseHelper.JsonError();
            }
        }

        [HttpPost]
        public IActionResult createReceipt([FromBody] CreateReceiptRequest itemData)
        {
            if (itemData.receiptTotalDiscount < 0) return responseHelper.JsonError();
            if (itemData.receiptTradeDiscount < 0) return responseHelper.JsonError();
            if (itemData.receiptTotalBeforeDiscount < 0) return responseHelper.JsonError();
            if (itemData.receiptGrandTotal < 0) return responseHelper.JsonError();
            if (itemData.receiptSubTotal < 0) return responseHelper.JsonError();
            if (itemData.receiptdetails == null) return responseHelper.JsonError();
            using (TransactionScope transactionScope = new TransactionScope())
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
                    logger.LogInformation(newReceipt.receiptGrandTotal.ToString());
                    int receiptId = receiptService.createReceipts(newReceipt);
                    receiptService.createReceiptDetail(itemData.receiptdetails, receiptId);
                    transactionScope.Complete();
                    return responseHelper.JsonCreate();
                }
                catch (Exception ex)
                {
                    logger.LogInformation(ex.Message);
                    transactionScope.Dispose();
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
