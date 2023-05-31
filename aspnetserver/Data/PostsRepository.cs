using Microsoft.EntityFrameworkCore;

namespace aspnetserver.Data
{
    internal static class PostsRepository
    {
        //GET LIST
        internal async static Task<List<Post>> GetPostsAsync()
        {
            //"using" better way, because it's efficient to clean up garbage collector after use (by the microsoft docs)
            using (var db = new AppDBContext())
            {
                return await db.Posts.ToListAsync();
            }
        }
        //GET BY ID
        internal async static Task<Post> GetPostByIdAsync(int postId)
        {
            using (var db = new AppDBContext())
            {
                return await db.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
            }
        }
        //ADD TO DB
        internal async static Task<bool> CreatePostAsync(Post postToCreate)
        {
            using (var db = new AppDBContext())
            {
                try
                {
                    await db.Posts.AddAsync(postToCreate);

                    //If amount of chabges are more than or equal to 1 it worked, it's true
                    return await db.SaveChangesAsync() >= 1;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        //UPDATE POST
        internal async static Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            using (var db = new AppDBContext())
            {
                try
                {
                    db.Posts.Update(postToUpdate);

                    //If amount of chabges are more than or equal to 1 it worked, it's true
                    return await db.SaveChangesAsync() >= 1;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        //DELETE POST
        internal async static Task<bool> DeletePostAsync(int postId)
        {
            using (var db = new AppDBContext())
            {
                try
                {
                    Post postToDelete = await GetPostByIdAsync(postId);

                    db.Posts.Remove(postToDelete);

                    //If amount of chabges are more than or equal to 1 it worked, it's true
                    return await db.SaveChangesAsync() >= 1;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

    }
}
