using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TribeTussleGameDataEditor.API.DAL.Models
{
    public class GameData
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public GameDataData Data { get; set; }
    }

    public class GameDataData
    {
        [Required]
        public MainGameQuestion[] Questions { get; set; }
        [Required]
        public Question[] FastMoney { get; set; }
    }

    public class Question
    {
        [Required]
        public string Prompt { get; set; }
        [Required]
        public Answer[] Answers { get; set; }
    }

    public class MainGameQuestion : Question
    {
        [Required]
        [Range(1, 3, ErrorMessage = "Game data question scale must be between 1 and 3.")]
        public uint Scale { get; set; }
    }

    public class Answer
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public uint Value { get; set; }
    }

    public class GameDataRequestDTO
    {
        [Required]
        [ValidFileName(ErrorMessage = "The game data name may not contain any of the following characters: \\ / : * ? \" < > |")]
        public string Name { get; set; }
        [Required]
        public GameDataData Data { get; set; }
    }

    public class ValidFileNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string fileName = value as string;
            return !string.IsNullOrEmpty(fileName) && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}
