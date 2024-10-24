using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;

namespace IW5Forms.Api.DAL.Memory.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly IList<QuestionEntity> questions;
    private readonly IList<AnswerEntity> answers;

    public QuestionRepository(Storage storage)
    {
        questions = storage.Questions;
        answers = storage.Answers;
    }

    public IList<QuestionEntity> GetAll()
    {
        return questions;
    }

    public QuestionEntity? GetById(Guid id)
    {
        var questionEntity = questions.SingleOrDefault(
            recipe => recipe.Id == id
        );

        if (questionEntity is not null)
        {
            questionEntity.Answers = GetAnswersByQuestionId(id);
        }

        return questionEntity;
    }

    public Guid Insert(QuestionEntity entity)
    {
        questions.Add(entity);

        foreach (var answer in entity.Answers)
        {
            answers.Add(new AnswerEntity
            {
                Id = answer.Id,
                Text = answer.Text,
                ResponderId = answer.ResponderId,
                QuestionId = entity.Id,
            });
        }

        return entity.Id;
    }

    public Guid? Update(QuestionEntity questionEntity)
    {
        var existingQuestion = questions.SingleOrDefault(
            entity => entity.Id == questionEntity.Id
        );

        if (existingQuestion is not null)
        {
            existingQuestion.Answers =
                GetAnswersByQuestionId(questionEntity.Id);
            UpdateAnswers(questionEntity, existingQuestion);
        }

        return existingQuestion?.Id;
    }

    public void Remove(Guid id)
    {
        var answersToRemove = answers.Where(t => t.QuestionId == id);
        DeleteAnswers(answersToRemove);

        var questionToRemove = questions.SingleOrDefault(t => t.Id == id);
        if (questionToRemove is not null)
        {
            questions.Remove(questionToRemove);
        }
    }

    public bool Exists(Guid id)
    {
        return questions.Any(question => question.Id == id);
    }

    private IList<AnswerEntity> GetAnswersByQuestionId(Guid questionId)
    {
        return answers.Where(
            answerEntity => answerEntity.QuestionId == questionId
        ).ToList();
    }

    private void InsertAnswers(
        QuestionEntity existing,
        IEnumerable<AnswerEntity> answersToInsert
    )
    {
        foreach (var answer in answersToInsert )
        {
            answers.Add(new AnswerEntity
            {
                Id = answer.Id,
                Text = answer.Text,
                ResponderId = answer.ResponderId,
                QuestionId = answer.QuestionId,
            });
        }
    }

    private void UpdateAnswers(QuestionEntity updated, QuestionEntity existing)
    {
        var answersToDelete = existing.Answers.Where(t =>
            !updated.Answers.Select(a => a.Id).Contains(t.Id));
        DeleteAnswers(answersToDelete);

        var answersToInsert = updated.Answers.Where(t =>
            !existing.Answers.Select(a => a.Id).Contains(t.Id));
        InsertAnswers(existing, answersToInsert);

        var answersToUpdate = updated.Answers.Where(t =>
            existing.Answers.Select(a => a.Id).Contains(t.Id));
        UpdateAnswers(answersToUpdate);
    }

    private void UpdateAnswers(IEnumerable<AnswerEntity> answersToUpdate)
    {
        foreach (var answer in answersToUpdate)
        {
            var answerEntity = answers.Single(t => t.Id == answer.Id);
            if (answerEntity is not null)
            {
                answerEntity.Text = answer.Text;
                answerEntity.ResponderId = answer.ResponderId;
            }
        }
    }

    private void DeleteAnswers(IEnumerable<AnswerEntity> answersToDelete)
    {
        var toDelete = answersToDelete.ToList();
        for (int i = 0; i < toDelete.Count; i++)
        {
            var answerEntity = toDelete.ElementAt(i);
            answers.Remove(answerEntity);
        }
    }
}
