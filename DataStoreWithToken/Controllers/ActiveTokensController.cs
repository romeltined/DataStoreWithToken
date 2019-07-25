using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataStoreWithToken.Data;
using DataStoreWithToken.Models;

namespace DataStoreWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveTokensController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActiveTokensController(ApplicationDbContext context)
        {
            _context = context;
        }

       

        // POST: api/ActiveTokens
        [HttpPost]
        public async Task<ActionResult<TokenResponse>> PostActiveToken(ActiveToken activeToken)
        {

            ////ActiveToken activeToken = new ActiveToken
            ////{
            ////    Otp = content
            ////};
            _context.ActiveToken.Add(activeToken);
            await _context.SaveChangesAsync();

            //Check Otp and return token

            TokenResponse tokenresponse = new TokenResponse();

            tokenresponse.TokenValue = "memel";
            tokenresponse.ResponseCode = "00";
            tokenresponse.ResponseMsg = "OK";

            return tokenresponse;
            //return CreatedAtAction("GetActiveToken", new { id = activeToken.Id }, activeToken);
        }

       

    }
}
