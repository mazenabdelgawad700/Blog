﻿using Blog.Service.Abstracts;
using Blog.Service.Implementaions;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Service
{
    public static class ModuleIServiceDependancies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ISendEmailService, SendEmailService>();
            services.AddTransient<IConfirmEmailService, ConfirmEmailService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IPostPictureService, PostPictureService>();
            services.AddTransient<ILikeService, LikeService>();
            services.AddTransient<ICommentService, CommentService>();
            return services;
        }
    }
}
