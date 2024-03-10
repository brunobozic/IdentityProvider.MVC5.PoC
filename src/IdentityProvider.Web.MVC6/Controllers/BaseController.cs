using AutoMapper;
using IdentityProvider.Repository.EFCore;
using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.EFDataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Module.CrossCutting.Cookies;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IdentityProvider.Web.MVC6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected readonly ICookieStorageService _cookieStorageService;
        protected readonly ILogger<BaseController> _errorLogService;
        protected readonly IConfiguration Configuration;
        protected readonly IHttpContextAccessor ContextAccessor;
        protected readonly IMapper Mapper;
        protected readonly IMemoryCache MemCache;
        protected readonly IMyUnitOfWork UnitOfWork;
        private readonly IOptionsSnapshot<ApplicationSettings> _configurationValues;

        public BaseController(IMyUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<ApplicationSettings> configurationValues, IMemoryCache memCache, IHttpContextAccessor contextAccessor, IConfiguration configuration, ICookieStorageService cookieStorageService, ILogger<BaseController> logger)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            _configurationValues = configurationValues;
            MemCache = memCache;
            ContextAccessor = contextAccessor;
            Configuration = configuration;
            _cookieStorageService = cookieStorageService;
            _errorLogService = logger;
        }
        public ApplicationUser Account => HttpContext.Items["Account"] as ApplicationUser;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = new AppDbContext().Users.SingleOrDefault(u => u.UserName == username);
                ViewData["FullName"] = $"{user?.FirstName} {user?.LastName}";
            }
        }

        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        public ViewResult HandleException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var exception = filterContext.Exception;
            _errorLogService.LogError(exception.Message, exception);

            var viewResult = View("Error", new HandleErrorInfo(exception, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString()));
            return viewResult;
        }

        public static string ErrorWrapper(IEnumerable<ErrorFailure> exceptions, string nameOfMethod)
        {
            var sb = new StringBuilder();
            foreach (var exc in exceptions)
            {
                sb.Append(exc.message);
                Log.ForContext("NameOfMethod", nameOfMethod ?? "N/A")
                   .ForContext("Exception", sb.ToString() ?? "N/A")
                   .Error(sb.ToString());
            }
            return sb.ToString();
        }
        public static string ErrorWrapper(Exception exc, string nameOfMethod)
        {
            Log.ForContext("NameOfMethod", nameOfMethod ?? "N/A")
               .ForContext("Exception", exc?.Message ?? "N/A")
               .ForContext("InnerException", exc?.InnerException?.Message ?? "N/A")
               .ForContext("StackTrace", exc?.StackTrace ?? "N/A")
               .Error("Error while working on: {nameOfMethod}", nameOfMethod ?? "N/A");
            return "";
        }
        public static string HandleEnvironmentBasedResponse(Exception ex, string environment, string nameOfMethod,
        string messageTemplate, string correlationId, string origin)
        {
            if (ex != null)
                ErrorWrapper(ex, nameOfMethod);

            if (environment == "Production")
            {
                return string.Format(messageTemplate + ", your ticket Id is: " + correlationId);
            }

            if (ex != null)
                return ex?.Message + " " + Environment.NewLine + ex?.StackTrace + " " + Environment.NewLine +
                       ex?.InnerException?.Message + Environment.NewLine + "- your ticket Id is: " + correlationId;

            return "Unspecified error occured, xour ticket Id is: " + correlationId;
        }

        public static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }

        protected PagedResult<TDto> ConvertToPagedResult<TEntity, TDto>(PagedResult<TEntity> pagedResult)
        {
            return new PagedResult<TDto>
            {
                Count = pagedResult.Count,
                PageCount = pagedResult.PageCount,
                Data = Mapper.Map<IEnumerable<TDto>>(pagedResult.Data)
            };
        }
    }
}
