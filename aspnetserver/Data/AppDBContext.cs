using Microsoft.EntityFrameworkCore;

namespace aspnetserver.Data
{
    internal sealed class AppDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        //Use built in DB, by calling in terminal "dotnet ef database update" it will be generated the .db file (reading using the DB reader(SQLite))
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=./Data/AppDB.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Post[] postsToSeed = new Post[6];

            for (int i = 1; i <= 6; i++)
            {
                postsToSeed[i - 1] = new Post
                {
                    PostId = i,
                    Title = $"Post {i}",
                    Content = $"This is post {i} and it has some example content."
                };
            }
            //Tell entity framework to seed the post, to add the data
            modelBuilder.Entity<Post>().HasData(postsToSeed);
        }
    }
}
