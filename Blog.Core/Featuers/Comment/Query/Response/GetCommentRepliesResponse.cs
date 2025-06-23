namespace Blog.Core.Featuers.Comment.Query.Response
{
    public class GetCommentRepliesResponse
    {
        public int Id { get; set; }
        public int ParentCommentId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
