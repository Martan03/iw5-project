using AutoMapper;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;

namespace IW5Forms.Api.DAL.Memory.Repositories;

public class FormRepository : IFormRepository
{
    private readonly IList<FormEntity> forms;
    private readonly IList<QuestionEntity> questions;
    private readonly IList<AnswerEntity> answers;

    public FormRepository(Storage storage)
    {
        forms = storage.Forms;
        questions = storage.Questions;
        answers = storage.Answers;
    }

    public IList<FormEntity> GetAll()
    {
        return forms;
    }

    public FormEntity? GetById(Guid id)
    {
        var formEntity = forms.SingleOrDefault(entity => entity.Id == id);
        if (formEntity is not null)
        {
            formEntity.Questions = GetQuestionsByFormId(id);
            foreach (var question in formEntity.Questions)
            {
                question.Answers = GetAnswersByQuestionId(question.Id);
            }
        }
        return formEntity;
    }

    public Guid Insert(FormEntity entity)
    {
        forms.Add(entity);

        InsertQuestions(entity, entity.Questions);

        return entity.Id;
    }
    public Guid? Update(FormEntity entity)
    {
        var existingForm = forms.SingleOrDefault(t => t.Id == entity.Id);
        if (existingForm is not null)
        {
            existingForm.Questions = GetQuestionsByFormId(entity.Id);
            UpdateQuestions(entity, existingForm);
        }

        return existingForm?.Id;
    }

    public void Remove(Guid id)
    {
        var questionsToRemove = questions.Where(t => t.FormId == id);
        DeleteQuestions(questionsToRemove);

        var formToRemove = forms.Single(form => form.Id.Equals(id));

        forms.Remove(formToRemove);
    }

    public bool Exists(Guid id)
    {
        return forms.Any(form => form.Id == id);
    }

    private IList<QuestionEntity> GetQuestionsByFormId(Guid id)
    {
        return questions.Where(t => t.FormId == id).ToList();
    }

    private IList<AnswerEntity> GetAnswersByQuestionId(Guid id)
    {
        return answers.Where(t => t.QuestionId == id).ToList();
    }

    private void InsertQuestions(
        FormEntity existing,
        IEnumerable<QuestionEntity> questionsToInsert
    )
    {
        foreach (var question in questionsToInsert)
        {
            questions.Add(new QuestionEntity
            {
                Id = question.Id,
                QuestionType = question.QuestionType,
                Text = question.Text,
                Description = question.Description,
                Options = question.Options,
                FormId = existing.Id,
            });
        }
    }

    private void UpdateQuestions(FormEntity updated, FormEntity existing)
    {
        var questionsToDelete = existing.Questions.Where(t =>
            !updated.Questions.Select(a => a.Id).Contains(t.Id));
        DeleteQuestions(questionsToDelete);

        var questionsToInsert = updated.Questions.Where(t =>
            !existing.Questions.Select(a => a.Id).Contains(t.Id));
        InsertQuestions(existing, questionsToInsert);

        var questionsToUpdate = updated.Questions.Where(t =>
            existing.Questions.Select(a => a.Id).Contains(t.Id));
        UpdateQuestions(existing, questionsToUpdate);
    }

    private void UpdateQuestions(
        FormEntity formEntity,
        IEnumerable<QuestionEntity> questionsToUpdate
    )
    {
        foreach (var question in questionsToUpdate)
        {
            var questionEntity = questions.Single(t => t.Id == question.Id);
            if (questionEntity is not null)
            {
                questionEntity.QuestionType = question.QuestionType;
                questionEntity.Text = question.Text;
                questionEntity.Description = question.Description;
                questionEntity.Options = question.Options;
            }
        }
    }

    private void DeleteQuestions(IEnumerable<QuestionEntity> questionsToDelete)
    {
        var toDelete = questionsToDelete.ToList();
        for (int i = 0; i < toDelete.Count; i++)
        {
            var questionEntity = toDelete.ElementAt(i);
            questions.Remove(questionEntity);
        }
    }
}
