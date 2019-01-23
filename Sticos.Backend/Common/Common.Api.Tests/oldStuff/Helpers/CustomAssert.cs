using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Common.Api.Tests.Helpers
{
    public static class CustomAssert
    {
        public static T AssertOkResponse<T>(ActionResult<T> response) where T : class
        {
            Assert.IsInstanceOf<OkObjectResult>(response.Result);
            var result = response.Result as OkObjectResult;
            var resultValue = result?.Value as T;
            Assert.AreEqual(200, result?.StatusCode);

            return resultValue;
        }

        public static IList<T> AssertOkResponseCount<T>(ActionResult<IEnumerable<T>> response, int expectedCount)
            where T : class
        {
            var resultValue = AssertOkResponse(response).ToList();
            Assert.AreEqual(expectedCount, resultValue.Count);

            return resultValue;
        }
        public static IList<T> AssertOkResponseCount<T>(ActionResult<ICollection<T>> response, int expectedCount)
            where T : class
        {
            var resultValue = AssertOkResponse(response).ToList();
            Assert.AreEqual(expectedCount, resultValue.Count);

            return resultValue;
        }

        public static void AssertBadResponse<T>(ActionResult<T> response) where T : class
        {
            Assert.IsInstanceOf<BadRequestResult>(response.Result);
            var result = response.Result as BadRequestResult;
            Assert.AreEqual(400, result?.StatusCode);
        }

        public static T AssertCreatedAtResponse<T>(ActionResult<T> response) where T : class
        {
            Assert.IsInstanceOf<CreatedAtActionResult>(response.Result);
            var result = response.Result as CreatedAtActionResult;
            var resultValue = result?.Value as T;

            return resultValue;
        }

        public static void AssertStatusCodeResult(IActionResult response, int statusCode)
        {
            Assert.IsInstanceOf<StatusCodeResult>(response);
            var convertedResponse = response as StatusCodeResult;
            Assert.AreEqual(statusCode, convertedResponse?.StatusCode);
        }

        public static void AssertStatusCodeResult<T>(IActionResult response, int statusCode)
        {
            Assert.IsInstanceOf<T>(response);
            AssertStatusCodeResult(response, statusCode);
        }
    }
}