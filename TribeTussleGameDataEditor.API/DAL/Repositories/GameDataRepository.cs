using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TribeTussleGameDataEditor.API.DAL.Models;
using TribeTussleGameDataEditor.API.Util;

namespace TribeTussleGameDataEditor.API.DAL.Repositories
{
    public interface IGameDataRepository
    {
        string[] ListNamesByUserId(long userId);
        GameData GetByUserIdAndName(long userId, string name);
        void Create(long userId, GameData gameData);
        void Update(long userId, GameData gameData, string oldName);
        void DeleteByUserIdAndName(long userId, string name);
        bool GameDataFileExists(long userId, string name);
    }

    public class GameDataRepository : IGameDataRepository
    {
        private const string DefaultGameDataDir = "C:\\tribe_tussle_game_data";
        private readonly IYAMLReader<GameDataData> _yamlReader;
        private readonly IYAMLWriter<GameDataData> _yamlWriter;
        private readonly IFileWriter _fileWriter;
        private readonly string _gameDataDir;

        public GameDataRepository(IYAMLReader<GameDataData> yamlReader, IYAMLWriter<GameDataData> yamlWriter,
            IFileWriter fileWriter, IConfiguration configuration)
        {
            _yamlReader = yamlReader;
            _yamlWriter = yamlWriter;
            _fileWriter = fileWriter;
            _gameDataDir = configuration["FileDataDirectory"] ?? DefaultGameDataDir;
        }

        public string[] ListNamesByUserId(long userId)
        {
            try
            {
                return _yamlReader.GetYAMLFileNamesInDirectory(GetUserGameDataDirectory(userId));
            }
            catch (DirectoryNotFoundException)
            {
                return Array.Empty<string>();
            }
        }

        public GameData GetByUserIdAndName(long userId, string name)
        {
            try
            {
                return new GameData
                {
                    Name = name,
                    Data = _yamlReader.ReadFromYAML(GetGameDataFile(userId, name))
                };
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public void Create(long userId, GameData gameData)
        {
            _yamlWriter.WriteToYAML(GetGameDataFile(userId, gameData.Name), gameData.Data);
        }

        public void DeleteByUserIdAndName(long userId, string name)
        {
            _fileWriter.DeleteFile(GetGameDataFile(userId, name));
        }

        public void Update(long userId, GameData gameData, string oldName)
        {
            string oldGameDataFileName = GetGameDataFile(userId, oldName);
            string newGameDataFileName = GetGameDataFile(userId, gameData.Name);
            if (!_yamlReader.YAMLFileExists(oldGameDataFileName))
                throw new FileNotFoundException();
            if (_yamlReader.YAMLFileExists(newGameDataFileName) && oldGameDataFileName != newGameDataFileName)
                throw new GameDataUpdateConflictException();
            Create(userId, gameData);
            if (oldGameDataFileName != newGameDataFileName)
                DeleteByUserIdAndName(userId, oldName);
        }

        public bool GameDataFileExists(long userId, string name)
        {
            return _yamlReader.YAMLFileExists(GetGameDataFile(userId, name));
        }

        private string GetGameDataFile(long userId, string name)
        {
            return $"{Path.Combine(GetUserGameDataDirectory(userId), name)}{YAMLReader<GameDataData>.YAMLFileExtension}";
        }

        private string GetUserGameDataDirectory(long userId)
        {
            return Path.Combine(_gameDataDir, userId.ToString());
        }
    }

    public class GameDataUpdateConflictException : Exception { }
}
