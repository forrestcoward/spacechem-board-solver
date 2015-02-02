using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceChemSolver.Parse
{
    public static class ReactorParser
    {
        private static string reactorSchemaPath = ".\\Parse\\Schema\\reactor-schema-d3.json";

        public static Parse.Reactor Parse(string reactorJsonPath)
        {
            var reactorSchemaJson = File.ReadAllText(reactorSchemaPath);
            var reactorJson = File.ReadAllText(reactorJsonPath);

            JsonSchema reactorSchema = JsonSchema.Parse(reactorSchemaJson);
            JObject reactor = JObject.Parse(reactorJson);

            if(!reactor.IsValid(reactorSchema))
            {
                throw new Exception(string.Format("'{0}' did not pass schema validation"));
            }

            return JsonConvert.DeserializeObject<Parse.ReactorRoot>(reactor.ToString()).reactor;
        }

        private static bool Validate(Parse.Reactor reactor)
        {
            return false;
        }
    }
}
