using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService photoService;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {
            this._userRepository = userRepository;
            this.photoService = photoService;
            this._mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return _mapper.Map<MemberDto>(user);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);
            if (user == null) return NotFound();
            return user;
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdatesDto memberUpdatesDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());

            if (user == null) return BadRequest("Cannot update user");

            _mapper.Map(memberUpdatesDto, user);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed  to update the user due to internal error-test-git");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());

            if (user == null) return BadRequest("Cannot update user");

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction(

                nameof(GetUser),
                new { username = user.UserName },
                _mapper.Map<PhotoDto>(photo)
            );
            }


            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {

            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find user.");

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo.");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);


            if (currentMain != null) currentMain.IsMain = false;


            photo.IsMain = true;
            if (await _userRepository.SaveAllAsync()) return NoContent();

            // If the save fails, return a 400 Bad Request status with a descriptive message.
            return BadRequest("Problem setting main photo.");
        }
        [HttpDelete("delete-photo/{photoId:int}")]

        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find user.");
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");


        }
    }




}
