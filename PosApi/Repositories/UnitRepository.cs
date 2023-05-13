using PosApi.Context;
using PosApi.Models;
using PosApi.ViewModels;
using System.Collections.Generic;

namespace PosApi.Services
{
    public class UnitRepository
    {
        readonly posContext _posContext;
        public UnitRepository(posContext posContext)
        {
            _posContext = posContext;
        }
        /*public List<ItemResponse> getAllItems()
        {
            List<ItemResponse> responses = (from item in _posContext.items
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
            return responses;
        }
        public int createItem(item newItem)
        {
            _posContext.items.Add(newItem);
            _posContext.SaveChanges();
            return newItem.unitId;
        }
        public void deleteItem(int id)
        {
            item itemDelete =  _posContext.items.Single(item => item.itemId == id);
            _posContext.items.Remove(itemDelete);
            _posContext.SaveChanges();
        }
        public ItemResponse updateItem(item item)
        {
            return new ItemResponse();
        }*/
    }
}
