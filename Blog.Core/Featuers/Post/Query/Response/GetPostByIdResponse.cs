namespace Blog.Core.Featuers.Post.Query.Response
{
    public class GetPostByIdResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? UserProfilePicture { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Summary { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int ViewsCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<PostPictureDto> Pictures { get; set; } = new();
        public List<LikeDto> Likes { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = new();
    }

    // DTO for post pictures
    public class PostPictureDto
    {
        public int Id { get; set; }
        public string PictureUrl { get; set; }
        public int DisplayOrder { get; set; }
    }

    // DTO for likes - keep it simple since likes don't need much detail in responses
    public class LikeDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; } // For displaying who liked
        public string? UserProfilePicture { get; set; }
        public DateTime LikedAt { get; set; }
    }

    // DTO for comments with support for threading
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? UserProfilePicture { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        // For threaded comments
        public List<CommentDto> Replies { get; set; } = new();
        public int RepliesCount { get; set; }
    }
}
