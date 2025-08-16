using System;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UserRepository(DataContext context, IMapper mapper)
    {
        this._context = context;
        this._mapper = mapper;
    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await _context.Users
       .Where(x => x.UserName == username)
       .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
       .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = _context.Users
              .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
              .AsQueryable();
        return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);

    }

    public async Task<IEnumerable<AppUser>> GetUserAsync()
    {
        return await _context.Users
            .Include(x => x.Photos).ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await _context.Users.
        FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users
        .Include(x => x.Photos)
    .SingleOrDefaultAsync(x => x.UserName == userName);

    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;

    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}
