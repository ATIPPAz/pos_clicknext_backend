using PosApi.Context;
using PosApi.Models;
using PosApi.ViewModels;
using PosApi.ViewModels.ReceiptViewModel;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Security.Cryptography.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PosApi.Services
{
    public class ReceiptRepository
    {
        readonly posContext _posContext;
        public ReceiptRepository(posContext posContext)
        {
            _posContext = posContext;
        }


        public void createReceiptDetail(List<CreateReceiptDetails> newReceiptdetail,int receiptId)
        {
            List<receiptdetail> newRD = new List<receiptdetail>();
            newReceiptdetail.ForEach(receiptdetail =>
            {
                newRD.Add(new Models.receiptdetail()
                {
                    receiptId = receiptId,

                    itemId = receiptdetail.itemId,
                    itemQty = receiptdetail.itemQty,
                    itemPrice = receiptdetail.itemPrice,
                    itemDiscount = receiptdetail.itemDiscount,
                    itemDiscountPercent = receiptdetail.itemDiscountPercent,
                    itemAmount = receiptdetail.itemAmount,
                    unitId = receiptdetail.unitId
                });
            });
            _posContext.receiptdetails.AddRange(newRD);
            _posContext.SaveChanges();

        }


        public int createReceipts(receipt newReceipt)
        {
            int id = getLength() + 1;
            newReceipt.receiptCode += id;
            _posContext.receipts.Add(newReceipt);
            _posContext.SaveChanges();
            return newReceipt.receiptId;
        }
        private int getLength()
        {
            return (from _receipt in _posContext.receipts
                    orderby _receipt.receiptId descending
                    select _receipt).ToList().Count;
        }

        public List<ReceiptAllResponse> getAllReceipts(string startDate = "", string endDate = "")
        {
            if (startDate == "" && endDate == "")
            {
                return (from _receipt in _posContext.receipts
                             orderby _receipt.receiptId
                             select new ReceiptAllResponse
                             {
                                 receiptCode = _receipt.receiptCode,
                                 receiptId = _receipt.receiptId,
                                 receiptDate = _receipt.receiptDate,
                                 receiptGrandTotal = _receipt.receiptGrandTotal,
                             }).ToList();
            }
            else
            {
                DateTime startDateTime = DateTime.Parse(startDate);
                DateTime endDateTime = DateTime.Parse(endDate);
                return (from _receipt in _posContext.receipts
                             where DateTime.Compare(startDateTime, _receipt.receiptDate) <= 0 && DateTime.Compare(_receipt.receiptDate, endDateTime) <= 0
                             orderby _receipt.receiptId
                             select new ReceiptAllResponse
                             {
                                 receiptCode = _receipt.receiptCode,
                                 receiptId = _receipt.receiptId,
                                 receiptDate = _receipt.receiptDate,
                                 receiptGrandTotal = _receipt.receiptGrandTotal,
                             }).ToList();
            }
        }

        public ReceiptOneResponse getOneReceipt(int receiptId)
        {
            var query =
                        from _receipt in _posContext.receipts

                        join data in (from _receiptDetail in _posContext.receiptdetails
                                      join _unit in _posContext.units on _receiptDetail.unitId equals _unit.unitId
                                      join _item in _posContext.items on _receiptDetail.itemId equals _item.itemId
                                      where _receiptDetail.receiptId == receiptId
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
                                          itemPrice = _receiptDetail.itemPrice,

                                      }
                                      ) on _receipt.receiptId equals data.receiptId into _receiptListItem
                        where _receipt.receiptId == receiptId
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

            var result = query.ToList().First();
            ReceiptOneResponse responses = new ReceiptOneResponse()
            {
                receiptId = result.receiptId,
                receiptDate = result.receiptDate,
                receiptGrandTotal = result.receiptGrandTotal,
                receiptCode = result.receiptCode,
                receiptTotalBeforeDiscount = result.receiptTotalBeforeDiscount,
                receiptTotalDiscount = result.receiptTotalDiscount,
                receiptSubTotal = result.receiptSubTotal,
            };

            result.receiptdetails.ToList().ForEach(receiptDetails =>
            {
                responses.receiptdetails.Add(new receiptdetails()
                {

                    receiptDetailId = receiptDetails.receiptDetailId,
                    itemAmount = receiptDetails.itemAmount,
                    itemDiscount = receiptDetails.itemDiscount,
                    itemDiscountPercent = receiptDetails.itemDiscountPercent,
                    itemName = receiptDetails.itemName,
                    itemCode = receiptDetails.itemCode,
                    unitName = receiptDetails.unitName,
                    itemPrice = receiptDetails.itemPrice,
                    itemQty = receiptDetails.itemQty,
                });
            });

            return responses;
        }

    }
}
