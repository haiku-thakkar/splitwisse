using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSplitWise.DBContext;
using SecondSplitWise.Model;
using SecondSplitWise.Repository;
using SecondSplitWise.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecondSplitWise.Apis
{
    [Produces("application/json")]
    [Route("api/transaction")]
    public class TransactionController : Controller
    {
        ITransactionRepository _TransactionRepository;
        ILogger _Logger;
        private SecondSplitWiseContext _Context;

        public TransactionController(ITransactionRepository transRepo, ILoggerFactory loggerFactory, SecondSplitWiseContext context)
        {
            _TransactionRepository = transRepo;
            _Logger = loggerFactory.CreateLogger(nameof(TransactionController));
            _Context = context;
        }

        // GET api/transaction/id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> GetTransaction(int id)
        {

            try
            {
                var transaction = await _TransactionRepository.GetTransactionAsync(id);
                return Ok(transaction);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // GET api/transaction/all/groupid
        [HttpGet("all/{Groupid}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> GetGroupTransactions(int Groupid)
        {

            try
            {
                var transactions = await _TransactionRepository.GetGroupTransactionsAsync(Groupid);
                return Ok(transactions);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // GET api/transaction/all/userid/friendid
        [HttpGet("all/{Userid}/{Friendid}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> GetIndividualTransactions(int Userid, int Friendid)
        {

            try
            {
                var transactions = await _TransactionRepository.GetIndividualTransactionsAsync(Userid,Friendid);
                return Ok(transactions);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // GET api/transaction/alltrans/userid
        [HttpGet("alltrans/{Userid}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> GetAllTransactions(int Userid)
        {

            try
            {
                var transactions = await _TransactionRepository.GetAllTransactionsAsync(Userid);
                return Ok(transactions);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // POST api/transaction && PUT settlement
        [HttpPost]
        [ProducesResponseType(typeof(ApiGeneralResponse), 201)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> RecordPayment([FromBody]Transactions payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiGeneralResponse { Status = false });
            }

            try
            {
                var newTrans = await _TransactionRepository.RecordPaymentAsync(payment);
                if (newTrans == null)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return CreatedAtAction("GetTransactionRoute", new { id = newTrans.TransactionId },
                            new ApiGeneralResponse { Status = true, id = newTrans.TransactionId });

            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }
    }
}
