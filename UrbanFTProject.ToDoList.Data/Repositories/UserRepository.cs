
using System.Data;
using Microsoft.EntityFrameworkCore;
using UrbanFTProject.Repository;

namespace UrbanFTProject.ToDoList.Data.Repositories
{
    internal class UserRepository : IRepository<AspNetUsers>, IUserRepository
    {
        private readonly UrbanFTAssignmentDbContext _context;

        public UserRepository(UrbanFTAssignmentDbContext context)
        {
            _context = context;
        }

        public Task<AspNetUsers> AddAsync(AspNetUsers entity)
        {
            throw new NotImplementedException();
        }

        public Task<DataRowState> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AspNetUsers>?> GetAllAsync()
        {
            return await _context.AspnetUsers.ToListAsync();
        }

        public async Task<AspNetUsers?> GetByIdAsync(int id)
        {
            return await _context.AspnetUsers.Where(u=>u.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<AspNetUsers> GetUserByEmail(string email) => await _context.AspnetUsers.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();

        public Task UpdateAsync(AspNetUsers entity)
        {
            throw new NotImplementedException();
        }
    }
}
