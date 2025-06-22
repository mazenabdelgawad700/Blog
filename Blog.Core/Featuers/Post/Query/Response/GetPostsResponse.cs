namespace Blog.Core.Featuers.Post.Query.Response
{
    public class GetPostsResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MainImageUrl { get; set; }
    }
}
