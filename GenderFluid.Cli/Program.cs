using Newtonsoft.Json;

namespace GenderFluid.Cli;

static class Program
{
    static void Main(string? input, string? output)
    {
        if (input == null || output == null)
        {
            Console.WriteLine("Please specify the input and output files with the `--input` and `--output` CLI args!");
            return;
        }

        if (output.EndsWith(".json"))
        {
            using FileStream file = File.OpenRead(input);
            Entry[] entries = Trans.ReadTransFile(file);
        
            File.WriteAllText(output, JsonConvert.SerializeObject(entries, Formatting.Indented));
        }
        else if (output.EndsWith(".trans"))
        {
            Entry[] entries = JsonConvert.DeserializeObject<Entry[]>(File.ReadAllText(input))!;

            using FileStream outputFile = File.Create(output);
            Trans.WriteTransFile(entries, outputFile);
        }
    }
}