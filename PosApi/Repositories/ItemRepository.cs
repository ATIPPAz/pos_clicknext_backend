using PosApi.Context;
using PosApi.Models;
using PosApi.ViewModels.ItemViewModel;
using System.Collections.Generic;

namespace PosApi.Services
{
    public class ItemRepository
    {
        readonly posContext _posContext;
        public ItemRepository(posContext posContext)
        {
            _posContext = posContext;
        }
        public List<ItemResponse> getAllItems()
        {
            return (from item in _posContext.items
                    join _unit in _posContext.units on item.unitId equals _unit.unitId
                    orderby item.itemId
                    select new ItemResponse
                    {
                        unitId = item.unitId,
                        itemName = item.itemName,
                        itemPrice = item.itemPrice,
                        itemCode = item.itemCode,
                        itemId = item.itemId,
                        unitName = _unit.unitName
                    }).ToList();
        }
        public int createItem(item newItem)
        {
            _posContext.items.Add(newItem);
            _posContext.SaveChanges();
            return newItem.unitId;
        }
        public void deleteItem(int id)
        {
            item itemDelete = _posContext.items.Single(item => item.itemId == id);
            _posContext.items.Remove(itemDelete);
            _posContext.SaveChanges();
        }
        public void updateItem(UpdateItemRequest itemUpdate)
        {
            item itemUpdateDb = _posContext.items.Single(item => item.itemId == itemUpdate.itemId);
            itemUpdateDb.itemName = itemUpdate.itemName;
            itemUpdateDb.itemPrice = itemUpdate.itemPrice;
            itemUpdateDb.unitId = itemUpdate.unitId;
            _posContext.SaveChanges();
        }
    }
}
