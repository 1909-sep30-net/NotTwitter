﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Filters
{
	public class AuthourizeCheckFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var methodAttributes = context.MethodInfo.GetCustomAttributes(true);
			var controllerAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);

			var methodAllowAnonymous = methodAttributes.OfType<AllowAnonymousAttribute>().Any();
			var authorize = methodAttributes.Union(controllerAttributes).OfType<AuthorizeAttribute>().Any();

			if (!methodAllowAnonymous && authorize)
			{
				operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

				var bearerScheme = new OpenApiSecurityScheme
				{
					// the id here must match the security scheme name defined in the startup file
					Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerAuth" }
				};

				operation.Security = new List<OpenApiSecurityRequirement>
				{
                    // scope list should be empty when definition type is bearer
                    new OpenApiSecurityRequirement { [ bearerScheme ] = Array.Empty<string>() }
				};
			}
		}
	}
}
