using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSplitWise.DBContext;
using SecondSplitWise.DataModel;
using SecondSplitWise.Model;
using SecondSplitWise.Repository;
using SecondSplitWise.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecondSplitWise.Apis
{
    [Produces("application/json")]
    [Route("api/bill")]
    public class ExpenseController : Controller
    {
        IExpenseRepository _ExpenseRepository;
        ILogger _Logger;
        private SecondSplitWiseContext _Context;

        public ExpenseController(IExpenseRepository billRepo, ILoggerFactory loggerFactory, SecondSplitWiseContext context)
        {
            _ExpenseRepository = billRepo;
            _Logger = loggerFactory.CreateLogger(nameof(ExpenseController));
            _Context = context;
        }

        // GET api/bill/id
        [HttpGet("{id}", Name = "GetBillRoute")]
        [ProducesResponseType(typeof(ExpenseResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> SingleBill(int id)
        {
            try
            {
                var bill = await _ExpenseRepository.GetBillAsync(id);
                return Ok(bill);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // POST api/bill
        [HttpPost]
        [ProducesResponseType(typeof(ApiGeneralResponse), 201)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> CreateBill([FromBody]ExpenseModel bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiGeneralResponse { Status = false });
            }

            try
            {
                var newBill = await _ExpenseRepository.InsertBillAsync(bill);
                if (newBill == null)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return CreatedAtRoute("GetBillRoute", new { id = newBill.BillId },
                        new ApiGeneralResponse { Status = true, id = newBill.BillId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // PUT api/bill/id
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiGeneralResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> UpdateBill(int id, [FromBody]Expense bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiGeneralResponse { Status = false });
            }

            try
            {
                var status = await _ExpenseRepository.UpdateBillAsync(bill);
                if (!status)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return Ok(new ApiGeneralResponse { Status = true, id = bill.BillId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }


        // GET api/bill/allbill/userid
        [HttpGet("allbill/{id}")]
        [ProducesResponseType(typeof(List<ExpenseResponse>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> AllBills(int id)
        {

            try
            {
                var bills = await _ExpenseRepository.GetAllExpenses(id);
                return Ok(bills);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // GET api/bill/all/userid/friendid
        [HttpGet("all/{Userid}/{Friendid}")]
        [ProducesResponseType(typeof(List<ExpenseResponse>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> IndividualBills(int Userid, int Friendid)
        {

            try
            {
                var bills = await _ExpenseRepository.GetIndividualExpenses(Userid, Friendid);
                return Ok(bills);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // GET api/bill/all/groupid
        [HttpGet("all/{Groupid}")]
        [ProducesResponseType(typeof(List<ExpenseResponse>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> GroupBills(int Groupid)
        {

            try
            {
                var bills = await _ExpenseRepository.GetGroupExpenses(Groupid);
                return Ok(bills);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // DELETE api/bill/id
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiGeneralResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            try
            {
                var status = await _ExpenseRepository.DeleteBillAsync(id);
                if (!status)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return Ok(new ApiGeneralResponse { Status = true, id = id });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }
    }
}
