using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosApi.Context;
using PosApi.helpers;
using PosApi.Models;
using PosApi.Services;
using PosApi.ViewModels.ReceiptViewModel;
using PosApi.ViewModels.UnitViewModel;
using System.Transactions;

namespace PosApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UnitController : ControllerBase
    {
        readonly ResponseHelper responseHelper = new ResponseHelper();
        readonly ILogger logger;
        readonly UnitRepository unitRepository;

        public UnitController(ILogger<UnitController> logger, UnitRepository unitRepository)
        {
            this.logger = logger;
            this.unitRepository = unitRepository;
        }


        [HttpGet]
        public IActionResult getUnits()
        {
            try
            {
                List<unit> units = unitRepository.getAllUnits();
                return responseHelper.JsonGet<List<unit>>(units);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return responseHelper.JsonError();
            }
        }

        [HttpPost]
        public IActionResult updateUnit([FromBody] unitUpdateRequest unit)
        {
            if (unit.unitId == 0) return responseHelper.JsonError();
            if (string.IsNullOrEmpty(unit.unitName)) return responseHelper.JsonError();
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    unit unitEdit = new unit()
                    {
                        unitId = unit.unitId,
                        unitName = unit.unitName,
                    };
                    unitRepository.updateUnit(unitEdit);
                    transactionScope.Complete();
                    return responseHelper.JsonUpdate();
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                    transactionScope.Dispose();
                    return responseHelper.JsonError();
                }
            }

        }

        [HttpPost]
        public IActionResult deleteUnit([FromBody] unitDeleteRequest req)
        {
            if (req.unitId <= 0) return responseHelper.JsonError();
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {

                    unitRepository.deleteUnit(req.unitId);
                    transactionScope.Complete();
                    return responseHelper.JsonDelete();

                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                    transactionScope.Dispose();
                    return responseHelper.JsonError();
                }
            }
        }

        [HttpPost]
        public IActionResult createUnit([FromBody] unit unitData)
        {
            if (string.IsNullOrEmpty(unitData.unitName)) return responseHelper.JsonError();
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    unitRepository.createUnit(unitData);
                    transactionScope.Complete();
                    return responseHelper.JsonCreate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                    transactionScope.Dispose();
                    return responseHelper.JsonError();
                }
            }
        }
    }
}
