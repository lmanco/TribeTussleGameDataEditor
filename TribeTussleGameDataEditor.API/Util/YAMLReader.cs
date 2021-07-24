using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TribeTussleGameDataEditor.API.Util
{
    public interface IYAMLReader<T>
    {
        T ReadFromYAML(string fileName);
        string[] GetYAMLFilesInDirectory(string directoryName);
        string[] GetYAMLFileNamesInDirectory(string directoryName);
        bool YAMLFileExists(string fileName);
    }

    public class YAMLReader<T> : IYAMLReader<T>
    {
        public const string YAMLFileExtension = ".yaml";
        private readonly IFileReader _fileReader;

        public YAMLReader(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public T ReadFromYAML(string fileName)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
            return deserializer.Deserialize<T>(_fileReader.ReadAllText(fileName));
        }

        public string[] GetYAMLFilesInDirectory(string directoryName)
        {
            return _fileReader.GetFilesInDirectory(directoryName, $"*{YAMLFileExtension}");
        }

        public string[] GetYAMLFileNamesInDirectory(string directoryName)
        {
            return _fileReader.GetFileNamesWithoutPathsOrExtensions(GetYAMLFilesInDirectory(directoryName));
        }

        public bool YAMLFileExists(string fileName)
        {
            if (_fileReader.GetFileExtension(fileName) != YAMLFileExtension)
                fileName += YAMLFileExtension;
            return _fileReader.FileExists(fileName);
        }
    }
}
