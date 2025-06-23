using AutoMapper;
using Blog.Domain.Dtos;
using Blog.Domain.Entities;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.Hubs;
using Blog.Service.Abstracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Implementaions
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext; // Add SignalR

        public NotificationService(
            AppDbContext context,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext) // Inject SignalR Hub
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task CreateCommentNotificationAsync(int postId, int commentId, string actorUserId)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null || post.UserId == actorUserId)
                return;

            var actor = await _context.Users.FindAsync(actorUserId);
            if (actor == null) return;

            var notification = new Notification
            {
                UserId = post.UserId,
                ActorUserId = actorUserId,
                Type = NotificationType.Comment,
                Message = $"{actor.UserName} commented on your post: {post.Title}",
                PostId = postId,
                CommentId = commentId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send real-time notification
            var notificationDto = _mapper.Map<NotificationDto>(notification);
            notificationDto.ActorUserName = actor.UserName;
            notificationDto.PostTitle = post.Title;

            await _hubContext.Clients.Group($"user_{post.UserId}")
                .SendAsync("ReceiveNotification", notificationDto);

            // Also send updated unread count
            var unreadCount = await GetUnreadCountAsync(post.UserId);
            await _hubContext.Clients.Group($"user_{post.UserId}")
                .SendAsync("UpdateUnreadCount", unreadCount);
        }

        public async Task CreateLikeNotificationAsync(int postId, string actorUserId)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null || post.UserId == actorUserId)
                return;

            var existingNotification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.PostId == postId &&
                                        n.ActorUserId == actorUserId &&
                                        n.Type == NotificationType.Like);

            if (existingNotification != null) return;

            var actor = await _context.Users.FindAsync(actorUserId);
            if (actor == null) return;

            var notification = new Notification
            {
                UserId = post.UserId,
                ActorUserId = actorUserId,
                Type = NotificationType.Like,
                Message = $"{actor.UserName} liked your post: {post.Title}",
                PostId = postId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send real-time notification
            var notificationDto = _mapper.Map<NotificationDto>(notification);
            notificationDto.ActorUserName = actor.UserName;
            notificationDto.PostTitle = post.Title;

            await _hubContext.Clients.Group($"user_{post.UserId}")
                .SendAsync("ReceiveNotification", notificationDto);

            var unreadCount = await GetUnreadCountAsync(post.UserId);
            await _hubContext.Clients.Group($"user_{post.UserId}")
                .SendAsync("UpdateUnreadCount", unreadCount);
        }

        public async Task CreateNewPostNotificationAsync(int postId, string authorUserId)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null) return;

            // Get all users except the author (you can modify this for followers)
            var usersToNotify = await _context.Users
                .Where(u => u.Id != authorUserId)
                .ToListAsync();

            var notifications = usersToNotify.Select(user => new Notification
            {
                UserId = user.Id,
                ActorUserId = authorUserId,
                Type = NotificationType.NewPost,
                Message = $"{post.User.UserName} shared a new post: {post.Title}",
                PostId = postId
            }).ToList();

            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();

            // Send real-time notifications to all users
            var notificationDto = new NotificationDto
            {
                Message = $"{post.User.UserName} shared a new post: {post.Title}",
                Type = NotificationType.NewPost,
                ActorUserName = post.User.UserName,
                PostTitle = post.Title,
                PostId = postId,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var user in usersToNotify)
            {
                await _hubContext.Clients.Group($"user_{user.Id}")
                    .SendAsync("ReceiveNotification", notificationDto);

                var unreadCount = await GetUnreadCountAsync(user.Id);
                await _hubContext.Clients.Group($"user_{user.Id}")
                    .SendAsync("UpdateUnreadCount", unreadCount);
            }
        }

        public async Task CreateReplyNotificationAsync(int commentId, int replyId, string actorUserId)
        {
            var comment = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null || comment.UserId == actorUserId)
                return;

            var actor = await _context.Users.FindAsync(actorUserId);
            if (actor == null) return;

            var notification = new Notification
            {
                UserId = comment.UserId,
                ActorUserId = actorUserId,
                Type = NotificationType.Reply,
                Message = $"{actor.UserName} replied to your comment on: {comment.Post.Title}",
                PostId = comment.PostId,
                CommentId = replyId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send real-time notification
            var notificationDto = _mapper.Map<NotificationDto>(notification);
            notificationDto.ActorUserName = actor.UserName;
            notificationDto.PostTitle = comment.Post.Title;

            await _hubContext.Clients.Group($"user_{comment.UserId}")
                .SendAsync("ReceiveNotification", notificationDto);

            var unreadCount = await GetUnreadCountAsync(comment.UserId);
            await _hubContext.Clients.Group($"user_{comment.UserId}")
                .SendAsync("UpdateUnreadCount", unreadCount);
        }

        // ... rest of the methods remain the same
        public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId, int pageSize = 20)
        {
            var notifications = await _context.Notifications
                .Include(n => n.ActorUser)
                .Include(n => n.Post)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();

                // Send updated unread count in real-time
                var unreadCount = await GetUnreadCountAsync(userId);
                await _hubContext.Clients.Group($"user_{userId}")
                    .SendAsync("UpdateUnreadCount", unreadCount);
            }
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}
