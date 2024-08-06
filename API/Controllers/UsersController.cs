using System.Security.Claims;
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
    public class UsersController(IUserRepository userRepositorypository, IMapper mapper) : BaseApiController
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

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(username == null) return BadRequest("No username found in token");

            var user = await userRepositorypository.GetUserByUsernameAsync(username);

            if(user == null) return BadRequest("Could not find user");

            mapper.Map(memberUpdateDto, user);

            if(await userRepositorypository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update the user");
        }
    }
}