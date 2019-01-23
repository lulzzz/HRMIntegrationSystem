using System;
using System.Net;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shared.Exceptions;
using Shared.Middleware;

#pragma warning disable 1998

namespace Common.Api.Tests.Unit
{
    [TestFixture]
    public class ExceptionHandlingUnitTests
    {
        public ILogger<ExceptionHandling> Logger => A.Fake<ILogger<ExceptionHandling>>();

        [SetUp]
        public void Setup()
        {
            Context = A.Fake<HttpContext>();
        }

        public HttpContext Context { get; set; }

        [Test]
        public async Task NotFoundExceptionThrown_Returns404()
        {
            var middleware = new ExceptionHandling(Logger, async ctx => throw new NotFoundException());

            await middleware.Invoke(Context);

            Assert.AreEqual((int) HttpStatusCode.NotFound, Context.Response.StatusCode);
        }

        [Test]
        public async Task UnhandledExceptionThrown_Returns500()
        {
            var middleware = new ExceptionHandling(Logger, ctx => throw new Exception());

            await middleware.Invoke(Context);

            Assert.AreEqual((int)HttpStatusCode.InternalServerError, Context.Response.StatusCode);
        }

        [Test]
        public async Task ValidationExceptionThrown_Returns400()
        {
            var middleware = new ExceptionHandling(Logger, async ctx => throw new ValidationException());

            await middleware.Invoke(Context);

            Assert.AreEqual((int) HttpStatusCode.BadRequest, Context.Response.StatusCode);
        }

        [Test]
        public async Task NotImplementedExceptionThrown_Returns501()
        {
            var middleware = new ExceptionHandling(Logger, async ctx => throw new NotImplementedException());

            await middleware.Invoke(Context);

            Assert.AreEqual((int)HttpStatusCode.NotImplemented, Context.Response.StatusCode);
        }
    }
}