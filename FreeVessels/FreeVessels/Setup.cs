using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace FreeVessel
{
    class Setup
    {
        public static Root Retrieve(String filePath)
        {
            var input = File.OpenText(filePath);
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();
            var Output = deserializer.Deserialize<Root>(input);
            return Output;
        }

        public class Root
        {
            public GeneralSettings General { get; set; }
            public FVSettings FreeVessel { get; set; }
        }

        public class GeneralSettings
        {
            public string DefaultPrefix { get; set; }
            public string ReinitializeCommand { get; set; }
        }

        public class FVSettings
        {
            public List<VesselSettings> Vessels { get; set; }
            public List<RestrictedPF> RestrictedPlayfields { get; set; }
        }

        public class VesselSettings
        {
            public string Command { get; set; }
            public int UsageCost { get; set; }
            public int LevelMin { get; set; }
            public int LevelMax { get; set; }
            public string SpawnType { get; set; }
            public List<Spawnable> Spawn { get; set; }
        }

        public class RestrictedPF
        {
            public string ListName { get; set; }
            public List<string> Playfields { get; set; }
        }

        public class Spawnable
        {
            public string Blueprint { get; set; }
            public string EntityType { get; set; }
        }

        public static void WriteYaml(string Path, Root ConfigData)
        {
            File.WriteAllText(Path, "---\r\n");
            Serializer serializer = new SerializerBuilder()
                .Build();
            string WriteThis = serializer.Serialize(ConfigData);
            File.AppendAllText(Path, WriteThis);

        }
    }
}
