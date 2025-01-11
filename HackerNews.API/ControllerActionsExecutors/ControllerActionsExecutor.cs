using HackerNews.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace HackerNews.API.ControllerActionsExecutors
{
    /// <summary>
    /// The controller actions executor.
    /// </summary>
    public class ControllerActionsExecutor
    {
        private readonly ILogger<ControllerActionsExecutor> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerActionsExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ControllerActionsExecutor(ILogger<ControllerActionsExecutor> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executes the and handle errors.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="func">The func.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <returns>An IActionResult.</returns>
        public async Task<IActionResult> ExecuteAndHandleErrorsAsync<T>(ControllerBase controller, Func<Task<T>> func, [CallerMemberName] string callingMethod = "")
        {
            try
            {
                var result = await func().ConfigureAwait(false);
                return controller.Ok(result);
            }
            catch (AppExteranalApiException exception)
            {
                return HandleAppExteranalApiException(controller, exception, callingMethod);
            }
            catch (AppNotFoundApiException exception)
            {
                return HandleAppNotFoundApiException(controller, exception, callingMethod);
            }
            catch (Exception exception)
            {
                return HandleUnknownException(controller, exception, callingMethod);
            }
        }

        /// <summary>
        /// Handles the app not found api exception.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <returns>An IActionResult.</returns>
        private IActionResult HandleAppNotFoundApiException(ControllerBase controller, AppNotFoundApiException exception, string callingMethod)
        {
            return LogException(controller, callingMethod, exception, StatusCodes.Status404NotFound, $"{callingMethod} item not found");
        }

        /// <summary>
        /// Handles the app exteranal api exception.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <returns>An IActionResult.</returns>
        private IActionResult HandleAppExteranalApiException(ControllerBase controller, AppExteranalApiException exception, string callingMethod)
        {
            return LogException(controller, callingMethod, exception, StatusCodes.Status500InternalServerError, $"{callingMethod} failed");
        }

        /// <summary>
        /// Handles the unknown exception.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <returns>An IActionResult.</returns>
        private IActionResult HandleUnknownException(ControllerBase controller, Exception exception, string callingMethod)
        {
            return LogException(controller, callingMethod, exception, StatusCodes.Status500InternalServerError, $"{callingMethod} failed");
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="controllerMessage">The controller message.</param>
        /// <returns>An IActionResult.</returns>
        private IActionResult LogException(ControllerBase controller, string callingMethod, Exception exception, int statusCode, string controllerMessage)
        {
            var message = $"Exception in {callingMethod}: {exception}";

            _logger.LogError(exception, message);

            return controller.StatusCode(statusCode, controllerMessage);
        }
    }
}
