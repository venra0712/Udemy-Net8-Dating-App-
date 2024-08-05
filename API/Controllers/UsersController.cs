using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepositorypository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepositorypository.GetMembersAsync();


            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            var user = await userRepositorypository.GetMemberAsync(username);

            if(user == null) return NotFound();
            
            return user;
        }
    }
}