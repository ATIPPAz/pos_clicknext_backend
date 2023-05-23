using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PosApi.Context;
using PosApi.Models;
using PosApi.ViewModels;
using PosApi.ViewModels.ReceiptViewModel;
using System.Collections.Generic;
using System.Globalization;
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


        public void createReceiptDetail(List<CreateReceiptDetails> newReceiptdetail, int receiptId)
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

        public PrefixViewModel getPrefix()
        {
            PrefixViewModel res = new PrefixViewModel();
            res.prefix_keyName = (from _prefix in _posContext.prefix_keys
                                  select _prefix).Single().prefix_keyName;
            return res;
        }
        public int createReceipts(receipt newReceipt)
        {
            string prefix = getPrefix().prefix_keyName;
            receipt oldReceipt = (from _receipt in _posContext.receipts
                                  where _receipt.receiptCode.StartsWith(prefix)
                                  orderby _receipt.receiptId descending
                                  select _receipt).FirstOrDefault();
            int idReceipt;
            if (oldReceipt == null)
            {
                idReceipt = 0;
            }
            else
            {
                string idSub = oldReceipt.receiptCode.Substring(oldReceipt.receiptCode.Length-4, 4);
                
                idReceipt = Convert.ToInt32(idSub);
                
            }

            idReceipt += 1;
            string id = idReceipt.ToString().PadLeft(4, '0');
            newReceipt.receiptCode += prefix + id;
            _posContext.receipts.Add(newReceipt);
            _posContext.SaveChanges();
            return newReceipt.receiptId;
        }

        public List<ReceiptAllResponse> getAllReceipts(string startDate = "", string endDate = "")
        {
            DateTime _temp;
            DateTime? startDateTime = DateTime.TryParse(startDate, out _temp) ? _temp: (DateTime?)null;
            DateTime? endDateTime = DateTime.TryParse(endDate, out _temp) ? _temp : (DateTime?)null;
                return (from _receipt in _posContext.receipts
                        where (startDateTime == null || startDateTime <= _receipt.receiptDate) && (endDateTime == null || endDateTime >= _receipt.receiptDate)
                        orderby _receipt.receiptId
                        select new ReceiptAllResponse
                        {
                            receiptCode = _receipt.receiptCode,
                            receiptId = _receipt.receiptId,
                            receiptDate = _receipt.receiptDate,
                            receiptGrandTotal = _receipt.receiptGrandTotal,
                       }).ToList();
        }

        public ReceiptOneResponse getOneReceipt(int receiptId)
        {
            var receipt = (from _receipt in _posContext.receipts
                    where _receipt.receiptId == receiptId
                    select _receipt).Single();

            var details = (from _receiptDetail in _posContext.receiptdetails
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

                           }).ToList();
            ReceiptOneResponse responses = new ReceiptOneResponse()
            {
                receiptId = receipt.receiptId,
                receiptDate = receipt.receiptDate,
                receiptGrandTotal = receipt.receiptGrandTotal,
                receiptCode = receipt.receiptCode,
                receiptTotalBeforeDiscount = receipt.receiptTotalBeforeDiscount,
                receiptTotalDiscount = receipt.receiptTotalDiscount,
                receiptSubTotal = receipt.receiptSubTotal,
                receiptTradeDiscount = receipt.receiptTradeDiscount
            };

            foreach (var receiptDetails in details)
            {
                responses.receiptdetails.Add(new receiptdetails
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
            }
            return responses;
        }

    }
}
