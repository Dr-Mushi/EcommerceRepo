using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptApiController : ControllerBase
    {

        private readonly IReceiptRepository _receipt;


        public ReceiptApiController(IReceiptRepository receipt)
        {
            _receipt = receipt;
        }

        // GET: Receipt
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceipt()
        {
            return Ok(await _receipt.GetReceiptItems());
        }

        // POST: Receipt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> PostReceipt([Bind("ID,CartID")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {

                _receipt.AddReceipt(receipt);
                await _receipt.SaveReceipt();

                return Ok(GetReceipt().Result);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteReceipt(int id)
        {
            await _receipt.DeleteReceipt(id);
            return Ok(await _receipt.SaveReceipt());
        }




    }
}
