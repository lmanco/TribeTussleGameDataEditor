using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TribeTussleGameDataEditor.API.Util
{
    public interface IYAMLWriter<T>
    {
        void WriteToYAML(string fileName, T obj);
    }

    public class YAMLWriter<T> : IYAMLWriter<T>
    {
        private readonly IFileWriter _fileWriter;
        private readonly IFileReader _fileReader;

        public YAMLWriter(IFileWriter fileWriter, IFileReader fileReader)
        {
            _fileWriter = fileWriter;
            _fileReader = fileReader;
        }

        public void WriteToYAML(string fileName, T obj)
        {
            if (!_fileReader.FileDirectoryExists(fileName))
                _fileWriter.CreateFileDirectory(fileName);
            var serializer = new SerializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
            _fileWriter.WriteAllText(fileName, serializer.Serialize(obj));
        }
    }
}
