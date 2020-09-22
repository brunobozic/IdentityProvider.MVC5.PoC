using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToBeConvertedToVb.PoC.Desktop
{
    static class Program
    {
        private static Container container;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            // Add handler to handle the exception raised by main threads
            Application.ThreadException +=
            new ThreadExceptionEventHandler(Application_ThreadException);

            // Add handler to handle the exception raised by additional threads
            AppDomain.CurrentDomain.UnhandledException +=
            new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //Omitted some code about configuration files and WinForms initialization for brevity

            var services = new ServiceCollection();
            ServiceProvider = ConfigureServices(services);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Bootstrap();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile("log-{Date}.txt")
                .CreateLogger();

            Log.Information("Application Started");

            Application.Run(container.GetInstance<InitialForm>());
        }

        private static Logger _logger; // I noticed that you don't have to do
                                       // this in the ASP .NET samples as it's
                                       // taken care of "under the hood".
                                       // However, I couldn't see any
                                       // alternative in the standard
                                       // WinForms template in VS.



        public static IServiceProvider ServiceProvider { get; private set; }

        private static ServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(GetLogger(), true));
            GetLogger().Fatal("Hello!!"); // Just testing... nothing happens. Where's my log file?
            //Omitted registration of various unrelated services for brevity

            return services.BuildServiceProvider();
        }
        private static void Bootstrap()
        {
            // Create the container as usual.
            container = new Container();

            // Register your types, for instance:
            //container.Register<IUserRepository, SqlUserRepository>(Lifestyle.Singleton);
            //container.Register<IUserContext, WinFormsUserContext>();
            //container.Register<Form1>();

            // Optionally verify the container.
            container.Verify();
        }

        private static Logger GetLogger()
        {
            if (_logger is null)
            {
                _logger = new LoggerConfiguration()
                .WriteTo.File(@"c:\BenjaminLog.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }

            return _logger;
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
          
            GetLogger().Fatal("MyHandler caught : " + e.Message);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // All exceptions thrown by the main thread are handled over this method

            ShowExceptionDetails(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // All exceptions thrown by additional threads are handled in this method

            ShowExceptionDetails(e.ExceptionObject as Exception);

            // Suspend the current thread for now to stop the exception from throwing.
            Thread.CurrentThread.Suspend();
        }

        static void ShowExceptionDetails(Exception Ex)
        {
            // Do logging of exception details
            MessageBox.Show(Ex.Message, Ex.TargetSite.ToString(),MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //public static class PresenterFactory
        //{
        //    private static IContainer container;

        //    public static TPresenter Create<TPresenter>()
        //        where TPresenter : IPresenter
        //    {
        //        return (TPresenter)
        //            container.Resolve(typeof(TPresenter));
        //    }

        //    public static void SetContainer(IContainer container)
        //    {
        //        PresenterFactory.container = container;
        //    }
        //}
    }
}

