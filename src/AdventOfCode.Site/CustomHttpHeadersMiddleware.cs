﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Security.Cryptography;

namespace MartinCostello.AdventOfCode.Site;

/// <summary>
/// A class representing a middleware for writing custom HTTP headers.
/// </summary>
public sealed class CustomHttpHeadersMiddleware
{
    private static readonly string ContentSecurityPolicyTemplate = string.Join(
        ';',
        new[]
        {
            "default-src 'self'",
            "script-src 'self' 'nonce-{0}' cdn.jsdelivr.net cdnjs.cloudflare.com",
            "script-src-elem 'self' 'nonce-{0}' cdn.jsdelivr.net cdnjs.cloudflare.com",
            "style-src 'self' 'nonce-{0}' cdn.jsdelivr.net cdnjs.cloudflare.com",
            "style-src-elem 'self' 'nonce-{0}' cdn.jsdelivr.net cdnjs.cloudflare.com",
            "img-src 'self'",
            "font-src 'self' cdnjs.cloudflare.com",
            "connect-src 'self'",
            "media-src 'none'",
            "object-src 'none'",
            "child-src 'self'",
            "frame-ancestors 'none'",
            "form-action 'self'",
            "block-all-mixed-content",
            "base-uri 'self'",
            "manifest-src 'self'",
            "upgrade-insecure-requests",
        });

    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomHttpHeadersMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next request delegate in the pipeline.</param>
    public CustomHttpHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the specified middleware.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="environment">The current web host environment.</param>
    /// <returns>
    /// The result of invoking the middleware.
    /// </returns>
    public Task Invoke(HttpContext context, IWebHostEnvironment environment)
    {
        byte[] data = RandomNumberGenerator.GetBytes(32);
        string nonce = Convert.ToBase64String(data).Replace('+', '/');

        context.Items["csp-hash"] = nonce;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");

            if (environment.IsProduction())
            {
                context.Response.Headers.Add("Content-Security-Policy", ContentSecurityPolicy(nonce));
            }

            if (context.Request.IsHttps)
            {
                context.Response.Headers.Add("Expect-CT", "max-age=1800");
            }

            context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'");
            context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            context.Response.Headers.Add("Referrer-Policy", "no-referrer-when-downgrade");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Download-Options", "noopen");

            if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
            }

            context.Response.Headers.Add("X-Request-Id", context.TraceIdentifier);
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            return Task.CompletedTask;
        });

        return _next(context);
    }

    private static string ContentSecurityPolicy(string nonce)
        => string.Format(CultureInfo.InvariantCulture, ContentSecurityPolicyTemplate, nonce);
}