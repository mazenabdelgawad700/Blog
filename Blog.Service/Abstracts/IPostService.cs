﻿using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface IPostService
    {
        Task<ReturnBase<int>> AddPostAsync(Post post);
        Task<ReturnBase<bool>> UpdatePostAsync(Post post);
        Task<ReturnBase<Post>> GetPostForUpdateAsync(int postId);
        Task<ReturnBase<bool>> DeletePostAsync(int postId);
        Task<ReturnBase<Post>> GetPostByIdAsync(int postId);
        Task<ReturnBase<IQueryable<Post>>> GetPostsAsync();
        Task<ReturnBase<IQueryable<Post>>> GetUserPostsAsync(string userId);
    }
}
