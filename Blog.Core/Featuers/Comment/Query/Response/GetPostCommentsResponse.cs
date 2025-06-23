namespace Blog.Core.Featuers.Comment.Query.Response
{
    public class GetPostCommentsResponse
    {
        public int Id { get; set; } // Comment id
        public string UserId { get; set; }
        public string Content { get; set; }
        public int RepliesCount { get; set; }
    }
}
