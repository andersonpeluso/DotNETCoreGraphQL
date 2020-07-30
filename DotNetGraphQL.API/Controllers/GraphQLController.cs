﻿using GraphQL;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotNetGraphQL.API.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQLController : ControllerBase
    {
        private readonly ExemploSchema _schema;

        public GraphQLController(ExemploSchema schema)
        {
            _schema = schema;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();

            var schema = _schema;

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
            }).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result);
        }
    }
}