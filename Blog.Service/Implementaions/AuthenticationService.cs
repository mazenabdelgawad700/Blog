using Blog.Domain.Entities;
using Blog.Domain.Helpers;
using Blog.Infrastructure.Context;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Service.Implementaions
{
    internal class AuthenticationService : ReturnBaseHandler, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly AppDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISendEmailService _emailService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfirmEmailService confirmEmailService, AppDbContext dbContext, JwtSettings jwtSettings, IHttpContextAccessor httpContextAccessor, ISendEmailService emailService)
        {
            this._userManager = userManager;
            this._confirmEmailService = confirmEmailService;
            this._dbContext = dbContext;
            this._jwtSettings = jwtSettings;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<ReturnBase<bool>> RegisterUserAsync(ApplicationUser user, string password)
        {
            try
            {
                if (user is null)
                    return Failed<bool>("Invalid user data");

                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Bio) || string.IsNullOrEmpty(user.UserName))
                    return Failed<bool>("Please, enter required fields");

                var isUserExist = await _userManager.FindByEmailAsync(user.Email);

                if (isUserExist is not null)
                    return Failed<bool>("Email Address already used");

                user.Id = Guid.NewGuid().ToString();

                var createUserResult = await _userManager.CreateAsync(user, password);

                if (createUserResult.Succeeded)
                {
                    var sendConfirmationEmailResult = await _confirmEmailService.SendConfirmationEmailAsync(user);

                    while (!sendConfirmationEmailResult.Succeeded)
                        sendConfirmationEmailResult = await _confirmEmailService.SendConfirmationEmailAsync(user);

                    await _userManager.AddToRoleAsync(user, "User");

                    return Success(true, "user registerd successfully, please confirm your email address");
                }



                return Failed<bool>("Can not register user, pleas try again");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<string>> LoginAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return Failed<string>("Invalid Credentials");

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                    return Failed<string>("Wrong Email Or Password");

                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

                if (!isPasswordCorrect)
                    return Failed<string>("Wrong Email Or Password");

                string jwtId = Guid.NewGuid().ToString();
                string token = await GenerateJwtToken(user, jwtId);

                await BuildRefreshToken(user, jwtId);

                user.LastLoginAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                if (!user.EmailConfirmed)
                {
                    ReturnBase<bool> sendConfirmationEmailResult = await _confirmEmailService.SendConfirmationEmailAsync(user);
                    if (sendConfirmationEmailResult.Succeeded)
                    {
                        return Success<string>($"A Confirmation Email has been sent to {user.Email}. Please confirm your email first and then log in.");
                    }
                }
                return Success(token, "Logged in successfully");

            }
            catch (Exception ex)
            {
                return Failed<string>(ex.InnerException.Message);
            }
        }
        public async Task<ReturnBase<string>> RefreshTokenAsync(string accessToken)
        {
            try
            {
                if (!IsAccessTokenExpired(accessToken))
                    return Success("", "Access Token Is Valid");

                string? userId = GetUserIdFromToken(accessToken);
                string? jwtId = GetJwtIdFromToken(accessToken);

                if (jwtId is null || userId is null)
                    return Failed<string>("Invalid Access Token");

                RefreshToken? storedRefreshToken = await _dbContext.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.UserId.ToString() == userId && rt.JwtId == jwtId);

                if (storedRefreshToken is null || storedRefreshToken.IsRevoked)
                    return Failed<string>("Your session has expired. please log in again.");

                if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
                {
                    storedRefreshToken.IsRevoked = true;
                    _dbContext.RefreshTokens.Update(storedRefreshToken);
                    await _dbContext.SaveChangesAsync();
                    return Failed<string>("Your session has expired. please log in again.");
                }

                if (!storedRefreshToken.IsUsed)
                {
                    storedRefreshToken.IsUsed = true;
                    _dbContext.RefreshTokens.Update(storedRefreshToken);
                }

                ApplicationUser? user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                    return Failed<string>("Invalid Access Token");

                string newJwtId = Guid.NewGuid().ToString();
                string newAccessToken = await GenerateJwtToken(user, newJwtId);

                storedRefreshToken.JwtId = newJwtId;

                await _dbContext.SaveChangesAsync();

                if (newAccessToken is null)
                    return Failed<string>("FailedToGenerateNewAccessToken");

                return Success(newAccessToken, "New Access Token Created");
            }
            catch (Exception ex)
            {
                return Failed<string>(ex.InnerException.Message);
            }
        }
        private string? GetJwtIdFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }
        private string? GetUserIdFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value.ToString();
        }
        private bool IsAccessTokenExpired(string accessToken)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                if (tokenHandler.ReadToken(accessToken) is not JwtSecurityToken token)
                    return true;

                DateTimeOffset expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value));

                return expirationTime.UtcDateTime <= DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task BuildRefreshToken(ApplicationUser user, string jwtId)
        {
            RefreshToken newRefreshToken = new()
            {
                UserId = user.Id,
                UserRefreshToken = GenerateRefreshToken(),
                JwtId = jwtId,
                IsUsed = false,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate)
            };

            RefreshToken? existingRefreshTokenRecord = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == user.Id);

            if (existingRefreshTokenRecord is null)
            {
                await _dbContext.RefreshTokens.AddAsync(newRefreshToken);
            }
            else
            {
                existingRefreshTokenRecord.UserRefreshToken = GenerateRefreshToken();
                existingRefreshTokenRecord.CreatedAt = DateTime.UtcNow;
                existingRefreshTokenRecord.ExpiresAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate);

                _dbContext.RefreshTokens.Update(existingRefreshTokenRecord);
            }

            await _dbContext.SaveChangesAsync();
        }
        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user, string jwtId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims =
            [
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            ];
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }
        private async Task<string> GenerateJwtToken(ApplicationUser user, string jwtId)
        {
            List<Claim> claims = await GetClaimsAsync(user, jwtId);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ReturnBase<bool>> SendResetPasswordEmailAsync(string email)
        {
            try
            {
                if (email is null)
                    return Failed<bool>("Email is required");

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                    return Failed<bool>("User Not Found");

                string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                string encodedToken = WebUtility.UrlEncode(resetPasswordToken);
                HttpRequest requestAccessor = _httpContextAccessor.HttpContext.Request;

                UriBuilder uriBuilder = new()
                {
                    Scheme = requestAccessor.Scheme,
                    Host = requestAccessor.Host.Host,
                    Port = requestAccessor.Host.Port ?? -1,
                    Path = "api/applicationuser/ResetPassword",
                    Query = $"email={Uri.EscapeDataString(email)}&token={encodedToken}"
                };

                string returnUrl = uriBuilder.ToString();

                string message = $"To Reset Your Password Click This Link: <a href=\"{returnUrl}\">Reset Password</a>";

                var sendEmailResult = await _emailService.SendEmailAsync(email, message, "Reset Password Link", "text/html");

                if (sendEmailResult.Succeeded)
                    return Success(true, "Reset password email send successfully");

                return Failed<bool>(sendEmailResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
        public async Task<ReturnBase<bool>> ResetPasswordAsync(string resetPasswordToken, string newPassword, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(resetPasswordToken))
                    return Failed<bool>("Invalid Token");

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                    return Failed<bool>("User Not Found");

                string decodedToken = WebUtility.UrlDecode(resetPasswordToken);

                IdentityResult resetPasswordResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

                if (resetPasswordResult.Succeeded)
                    return Success(true, "Password has been reset successfully");

                return Failed<bool>(resetPasswordResult?.Errors?.FirstOrDefault()?.Description);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
