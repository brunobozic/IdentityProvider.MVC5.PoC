﻿using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.ViewModels.Error;
using StructureMap;
using System.Web.Mvc;

namespace IdentityProvider.Controllers.Controllers
{
    public class ErrorController : BaseController
    {
        private ICookieStorageService _cookieStorageService;
        [DefaultConstructor]
        public ErrorController(
            ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
            , IApplicationConfiguration applicationConfiguration)
        : base(
            cookieStorageService
            , errorLogService
            , applicationConfiguration
            )
        {
            _cookieStorageService = cookieStorageService;
        }

        public ViewResult Error()
        {
            var evm = (ErrorViewModel)TempData["ErrorViewModel"];

            return View(evm);
        }

        public ViewResult UserFriendlyError()
        {
            var evm = (HandleErrorInfo)TempData["HandleErrorInfo"];

            return View(evm);
        }
    }
}