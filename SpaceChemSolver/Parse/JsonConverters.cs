using Newtonsoft.Json;
using SpaceChemSolver.Simulation;
using System;

namespace SpaceChemSolver.Parse
{
    public class InstructionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return InstructionMapper.GetInstruction(reader.Value.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Instruction);
        }
    }

    public class DirectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return InstructionMapper.GetDirection(reader.Value.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Direction);
        }
    }
}
