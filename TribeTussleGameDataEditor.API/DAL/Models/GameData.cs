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
        [ValidQuestions(QuestionWord = "round")]
        public MainGameQuestion[] Questions { get; set; }
        [ValidQuestions(QuestionWord = "fast money question")]
        public Question[] FastMoney { get; set; }
    }

    public class Question
    {
        public string Prompt { get; set; }
        public Answer[] Answers { get; set; }
    }

    public class MainGameQuestion : Question
    {
        public uint Scale { get; set; }
    }

    public class Answer
    {
        public string Text { get; set; }
        public uint Value { get; set; }
    }

    public class GameDataRequestDTO
    {
        [Required(ErrorMessage = "A name for the game data is required.")]
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
            return fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }

    public class ValidQuestionsAttribute : ValidationAttribute
    {
        public string QuestionWord { get; set; } = "question";

        public override bool IsValid(object value)
        {
            var questions = value as Question[];
            int questionNumber = 1;
            ErrorMessage = string.Join('\n', questions.Aggregate(new List<string>(), (errorMessages, question) =>
            {
                if (question.Prompt == null)
                    errorMessages.Add($"A prompt for {QuestionWord} {questionNumber} is required.");
                if (question.Answers == null || question.Answers.Length < 1)
                    errorMessages.Add($"At least one answer is required for {QuestionWord} {questionNumber}.");
                if (question is MainGameQuestion mainGameQuestion && (mainGameQuestion.Scale < 1 || mainGameQuestion.Scale > 3))
                    errorMessages.Add($"The scale for {QuestionWord} {questionNumber} must be between 1 and 3.");
                int answerNum = 1;
                errorMessages.AddRange(question.Answers.Aggregate(new List<string>(), (answerErrors, answer) =>
                {
                    if (answer.Text == null)
                        errorMessages.Add($"Answer text is required for answer {answerNum} in {QuestionWord} {questionNumber}.");
                    return answerErrors;
                }));
                questionNumber++;
                return errorMessages;
            }));
            return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
