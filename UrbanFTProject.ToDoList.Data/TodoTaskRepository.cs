using Microsoft.EntityFrameworkCore;
using System.Data;
using UrbanFTProject.Data;
using UrbanFTProject.Repository;

namespace UrbanFTProject.ToDoList.Data
{
    internal class TodoTaskRepository : IRepository<TodoTask>
    {
        private readonly UrbanFTAssignmentDbContext _context;

        public TodoTaskRepository(UrbanFTAssignmentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoTask>?> GetAllAsync()
        {
            return await _context.TodoTasks.ToListAsync();
        }

        public async Task<TodoTask?> GetByIdAsync(int id)
        {
            return await _context.TodoTasks.FindAsync(id);
        }

        public async Task AddAsync(TodoTask entity)
        {
            await _context.TodoTasks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoTask entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<DataRowState> DeleteAsync(int entityId)
        {
            var existingTask=await GetByIdAsync(entityId);            
            if(existingTask!= null)
            {             
                _context.TodoTasks.Remove(existingTask);
                await _context.SaveChangesAsync();

                return DataRowState.Deleted;
            }

            return DataRowState.Unchanged;            
        }
    }
}
