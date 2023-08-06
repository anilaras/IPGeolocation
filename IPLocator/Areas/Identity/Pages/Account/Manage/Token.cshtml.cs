// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IPLocator.Data;
using IPLocator.Data.Repository.Abstracts;
using IPLocator.Helpers;
using IPLocator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IPLocator.Areas.Identity.Pages.Account.Manage
{
    public class TokenModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserTokensRepository _userTokensRepository;
        private ApplicationDbContext _dbContext;
        public TokenModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserTokensRepository userTokensRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userTokensRepository = userTokensRepository;
        }

        [Display(Name = "Api Token")]
        public string Token { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(IdentityUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var UserToken = _userTokensRepository.GetWhere(t => t.IdentityUserId.Equals(Guid.Parse(userId))).FirstOrDefault();

            if (UserToken != null)
            {
                Token = UserToken.Token;
                ViewData["GenerateStatus"] = "Refresh Token";

            }
            else
            {
                Token = "";
                ViewData["GenerateStatus"] = "Generate Token";
            }
        }
        public async Task<string> GenerateTokenAsync()
        {
            var token = UtilsHelper.Hash(Guid.NewGuid().ToString());

            var user = _userManager.GetUserId(User);

            var userTokenCount = _userTokensRepository.GetWhere(x => x.IdentityUserId.Equals(Guid.Parse(user))).Select(d => d.Token).Count();
            if (userTokenCount > 1)
            {
                return "you cant generate mode token"!;
            }
            _userTokensRepository.Create(new UserTokens
            {
                IdentityUserId = Guid.Parse(user),
                Token = token,
            });
            return token;
        }

        public string DeleteToken(string token)
        {
            //Not implemented
            return token;
        }

        public string GetUserToken()
        {
            var user = _userManager.GetUserId(User);
            var userToken = _userTokensRepository.GetWhere(x => x.IdentityUserId.Equals(Guid.Parse(user))).Select(d => d.Token).FirstOrDefault();

            if (string.IsNullOrEmpty(userToken))
            {
                return "NOTOKEN";
            }
            return userToken;
        }

        public string RefreshUserToken()
        {
            var user = _userManager.GetUserId(User);
            var userToken = _userTokensRepository.GetWhere(x => x.IdentityUserId.Equals(Guid.Parse(user))).FirstOrDefault();
            var newToken = UtilsHelper.Hash(Guid.NewGuid().ToString());
            userToken.Token = newToken;
            _userTokensRepository.Update(userToken);
            return newToken;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            try
            {
                string UserToken = _userTokensRepository.GetWhere(t => t.IdentityUserId.Equals(Guid.Parse(user.Id))).FirstOrDefault().Token;

            }
            catch (System.NullReferenceException)
            {

                var gToken = GenerateTokenAsync();
                if (true)
                {
                    StatusMessage = "Token Generated.";
                    return RedirectToPage();
                }
            }
            RefreshUserToken();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your Token has been updated";
            return RedirectToPage();
        }
    }
}
