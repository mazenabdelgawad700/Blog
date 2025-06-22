namespace Blog.Core.Featuers.Post.Query.Response
{
    public class GetUserPostsResponse
    {
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MainImageUrl { get; set; }
    }
}
