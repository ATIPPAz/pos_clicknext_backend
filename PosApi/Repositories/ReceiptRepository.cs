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

        public int createReceipts(receipt newReceipt, List<CreateReceiptDetails> newReceiptdetail)
        {
            receipt receipt = getOneReceipt();
            int id = receipt.receiptId + 1;
            newReceipt.receiptCode += id;
            _posContext.receipts.Add(newReceipt);
            _posContext.SaveChanges();
            /* receipt = getOneReceipt();*/
            List<receiptdetail> newRD = new List<receiptdetail>();
            newReceiptdetail.ForEach(receiptdetail =>
            {
                newRD.Add(new Models.receiptdetail()
                {
                    receiptId = id,
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

            return newReceipt.receiptId;
        }
        private receipt getOneReceipt()
        {
            return (from _receipt in _posContext.receipts
                    orderby _receipt.receiptId descending
                    select _receipt).ToList().First();
        }

        public List<ReceiptAllResponse> getAllReceipts(string startDate = "", string endDate = "")
        {
            List<ReceiptAllResponse> responses;
            if (startDate == "" && endDate == "")
            {
                responses = (from _receipt in _posContext.receipts
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
                responses = (from _receipt in _posContext.receipts
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
            return responses;
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
                                          itemPrice = _receiptDetail.itemPrice
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

        /*
                    var query = from _receipt in _posContext.receipts
                                join _receiptDetail in _posContext.receiptdetails on _receipt.receiptId equals _receiptDetail.receiptId into _receiptListItem 
                               *//* join _unit in _posContext.units on _receiptItem.unitId equals _unit.unitId*/
        /* join _item in _posContext.items on _receiptItem.itemId equals _item.itemId*//*
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
            *//* receiptTradeDiscount = _receipt.receiptTradeDiscount,
             itemQty = _receiptDetail.itemQty,
             itemPrice = _receiptDetail.itemPrice,
             itemDiscount = _receiptDetail.itemDiscount,
             itemDiscountPercent = _receiptDetail.itemDiscountPercent,
             itemAmount = _receiptDetail.itemAmount,*//*

         };*/



        /*  var query = from _receipt in _posContext.receipts
              where _receipt.receiptId == receiptId
              from _receiptDetail in _receipt.receiptdetails
              join _unit in _posContext.units on _receiptDetail.unitId equals _unit.unitId
              join _item in _posContext.items on _receiptDetail.itemId equals _item.itemId
              *//* group _receiptDetail by _receiptDetail.itemId;*//*
              select new
              {

                  receiptId = _receipt.receiptId,
                  receiptDate = _receipt.receiptDate,
                  receiptGrandTotal = _receipt.receiptGrandTotal,
                  receiptCode = _receipt.receiptCode,
                  receiptTotalBeforeDiscount = _receipt.receiptTotalBeforeDiscount,
                  receiptTotalDiscount = _receipt.receiptTotalDiscount,
                  receiptSubTotal = _receipt.receiptSubTotal,
                  receiptdetails = _receiptDetail,
                  receiptTradeDiscount = _receipt.receiptTradeDiscount,
                  itemQty = _receiptDetail.itemQty,
                  itemPrice = _receiptDetail.itemPrice,
                  itemDiscount = _receiptDetail.itemDiscount,
                  itemDiscountPercent = _receiptDetail.itemDiscountPercent,
                  itemAmount = _receiptDetail.itemAmount,
                  itemName = _item.itemName,
                  unitName = _unit.unitName
              };*/


        /*
                ((from _receipt in _posContext.receipts
                                 where receiptId == _receipt.receiptId
                                 select new
                                 {
                                     receiptId = _receipt.receiptId,
                                     receiptDate = _receipt.receiptDate,
                                     receiptGrandTotal = _receipt.receiptGrandTotal,
                                     receiptCode = _receipt.receiptCode,
                                     receiptTotalBeforeDiscount = _receipt.receiptTotalBeforeDiscount,
                                     receiptTotalDiscount = _receipt.receiptTotalDiscount,
                                     receiptSubTotal = _receipt.receiptSubTotal,
                                     receiptTradeDiscount = _receipt.receiptTradeDiscount,
                                 }).ToList().First()) join

                    *//*var query = from _receipt in _posContext.receipts
                                where receiptId == _receipt.receiptId
                                join _receiptDetail in _posContext.receiptdetails on _receipt.receiptId equals _receiptDetail.receiptId
                                orderby _receiptDetail.receiptDetailId
                                group _receiptDetail by _receiptDetail.unitId
                    ;

        */
        /*      var d = query.ToList();*//*


        var receipt = (from _receipt in _posContext.receipts
                       where receiptId == _receipt.receiptId
                       select new
                       {
                           receiptId = _receipt.receiptId,
                           receiptDate = _receipt.receiptDate,
                           receiptGrandTotal = _receipt.receiptGrandTotal,
                           receiptCode = _receipt.receiptCode,
                           receiptTotalBeforeDiscount = _receipt.receiptTotalBeforeDiscount,
                           receiptTotalDiscount = _receipt.receiptTotalDiscount,
                           receiptSubTotal = _receipt.receiptSubTotal,
                           receiptTradeDiscount = _receipt.receiptTradeDiscount,
                       }).ToList().First();
        ReceiptOneResponse responses = new ReceiptOneResponse()
        {
            receiptDate = receipt.receiptDate,
            receiptGrandTotal = receipt.receiptGrandTotal,
            receiptCode = receipt.receiptCode,
            receiptTotalBeforeDiscount = receipt.receiptTotalBeforeDiscount,
            receiptTotalDiscount = receipt.receiptTotalDiscount,
            receiptSubTotal = receipt.receiptSubTotal,
            receiptTradeDiscount = receipt.receiptTradeDiscount,
            receiptId = receipt.receiptId,
        };
        Console.WriteLine(responses);
        var receiptDetail = from _receiptDetail in _posContext.receiptdetails
                            where _receiptDetail.receiptId == receiptId
                            join _item in _posContext.items on _receiptDetail.itemId equals _item.itemId
                            join _unit in _posContext.units on _receiptDetail.unitId equals _unit.unitId
                            select new
                            {
                                receiptDetailId = _receiptDetail.receiptDetailId,
                                receiptId = _receiptDetail.receiptId,
                                itemId = _receiptDetail.itemId,
                                itemQty = _receiptDetail.itemQty,
                                itemPrice = _receiptDetail.itemPrice,
                                itemDiscount = _receiptDetail.itemDiscount,
                                itemDiscountPercent = _receiptDetail.itemDiscountPercent,
                                itemAmount = _receiptDetail.itemAmount,
                                unitId = _receiptDetail.unitId,
                                itemName = _item.itemName,
                                unitName = _unit.unitName
                            };

        foreach (var _receiptDetail in receiptDetail)
        {
            responses.receiptdetails.Add(
                new receiptdetails()
                {
                    *//* receiptDetailId = _receiptDetail.receiptDetailId,
                     receiptId = _receiptDetail.receiptId,
                     itemId = _receiptDetail.itemId,*/
        /*   unitId = _receiptDetail.unitId,*//*
        itemQty = _receiptDetail.itemQty,
        itemPrice = _receiptDetail.itemPrice,
        itemDiscount = _receiptDetail.itemDiscount,
        itemDiscountPercent = _receiptDetail.itemDiscountPercent,
        itemAmount = _receiptDetail.itemAmount,
        itemName = _receiptDetail.itemName,
        unitName = _receiptDetail.unitName
    })
};*/
        /* var xx = query.ToList();*//*
         var x = new List<receiptdetail>();
         foreach (var item1 in xx)
         {
             x.Add(item1);
             Console.WriteLine(item1);
         }
         foreach (var x in query)
         {
             Console.WriteLine(x);

         }*/
    }
}
