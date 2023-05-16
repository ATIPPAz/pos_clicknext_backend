using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using PosApi.Context;
using PosApi.Models;
using PosApi.ViewModels;
using PosApi.ViewModels.UnitViewModel;
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
        public List<unit> getAllUnits()
        {
            List<unit> units = (from unit in _posContext.units
                                select unit).ToList();
            
            return units;
        }
        public int createUnit(unit newUnit)
        {

            using (IDbContextTransaction transaction = _posContext.Database.BeginTransaction())
            {
                try
                {
                    _posContext.units.Add(newUnit);
                    _posContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return newUnit.unitId;
        }
        public void deleteUnit(int id)
        {
            unit itemDelete = _posContext.units.Single(item => item.unitId == id);
            _posContext.units.Remove(itemDelete);
            _posContext.SaveChanges();
        }
        public void updateUnit(unit unitEdit)
        {
            unit unitUpdate = _posContext.units.Single(item => item.unitId == unitEdit.unitId);
            unitUpdate.unitName = unitEdit.unitName;
            _posContext.SaveChanges();
        }
    }
}
