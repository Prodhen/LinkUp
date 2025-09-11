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
        var query = _context.Users.AsQueryable();
        query = query.Where(u => u.UserName != userParams.CurrentUsername);

        if (userParams.Gender != null)
        {
            query = query.Where(u => u.Gender == userParams.Gender);
        }
          var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge-1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };
        var projectedQuery = query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider);

        return await PagedList<MemberDto>.CreateAsync(projectedQuery, userParams.PageNumber, userParams.PageSize);

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
