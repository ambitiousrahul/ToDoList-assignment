using Microsoft.EntityFrameworkCore;

namespace UrbanFTProject.ToDoList.Data
{ 
    public class UrbanFTAssignmentDbContext : DbContext
    {
        public UrbanFTAssignmentDbContext(DbContextOptions<UrbanFTAssignmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTask> TodoTasks { get; set; } = default!;

        public DbSet<AspNetUsers> AspnetUsers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
