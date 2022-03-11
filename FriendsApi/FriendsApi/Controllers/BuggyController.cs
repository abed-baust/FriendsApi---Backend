﻿using FriendsApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FriendsApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController (DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet ("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret Text";
        }

        
        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if(thing == null)return NotFound();
            return Ok(thing);
        }

        
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            try
            {
                var thing = _context.Users.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Computer says no!");
            }
            
        }

        
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }



    }
}
