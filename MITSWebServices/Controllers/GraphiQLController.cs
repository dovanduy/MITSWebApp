using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraphQL;
using GraphQL.Types;
using MITSDataLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GraphQL.Validation;
using MITSBusinessLib.GraphQL;

namespace MITSWebServices.Controllers
{
    //[Authorize(Policy = "Admin")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class GraphQLController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly IEnumerable<IValidationRule> _validationRules;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, IEnumerable<IValidationRule> validationRules)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _validationRules = validationRules;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
                ValidationRules = _validationRules
                
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }

   
}