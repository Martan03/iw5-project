using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Api.DAL.Memory;
using IW5Forms.Api.DAL.Memory.Repositories;
using Newtonsoft.Json;

namespace IW5Forms.Api.DAL.IntegrationTests;

public class InMemoryDatabaseFixture : IDatabaseFixture
{
    public IList<Guid> AnswerGuids { get; } = new List<Guid>
    {
        new("df935095-8709-4040-a2bb-b6f97cb416dc"),
        new("23b3902d-7d4f-4213-9cf0-112348f56238"),
        new("7f251cd6-3ac4-49be-b3e7-d1f9f7cfdd3a"),
        new("adb7daf1-8a6c-4cbb-b4f5-631a9b7f7287"),
        new("a8978e5d-0c5b-449c-9dc0-0a01563c0c3b"),
        new("0e88301e-cd92-47cf-8ee7-5cb0752e9f82"),
        new("e79f129f-3153-41df-8e84-8bcd7a077648"),
        new("a62a2fb6-2b80-45b1-8f82-1401a6834abe"),
    };

    public IList<Guid> QuestionGuids { get; } = new List<Guid>
    {
        new("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
        new("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
        new("2caa29d8-61f0-4c1d-850d-4d70003e6aef"),
        new("c3542130-589c-4302-a441-a110fcadd45a"),
    };

    public IList<Guid> FormGuids { get; } = new List<Guid>
    {
        new("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        new("a8ee7ce8-9903-4f42-afb4-b2c34dfb7ccf"),
    };

    public IList<Guid> UserGuids { get; } = new List<Guid>
    {
        new("d57fa919-64e4-4b9c-b25e-bbd12120eb42"),
        new("342f4698-47b0-4a7a-a654-7e484cb55815"),
    };

    private readonly Lazy<Storage> inMemoryStorage;

    public InMemoryDatabaseFixture()
    {
        inMemoryStorage = new Lazy<Storage>(CreateInMemoryStorage);
    }

    public AnswerEntity? GetAnswerDirectly(Guid answerId)
    {
        var answer = inMemoryStorage.Value.Answers.SingleOrDefault(
            a => a.Id == answerId
        );
        return DeepClone(answer);
    }

    public QuestionEntity? GetQuestionDirectly(Guid questionId)
    {
        var question = inMemoryStorage.Value.Questions.SingleOrDefault(
            t => t.Id == questionId
        );

        if (question is not null)
        {
            question.Answers = inMemoryStorage.Value.Answers.Where(
                a => a.QuestionId == questionId
            ).ToList();
        }

        return DeepClone(question);
    }

    public FormEntity? GetFormDirectly(Guid formId)
    {
        var form = inMemoryStorage.Value.Forms.SingleOrDefault(
            t => t.Id == formId
        );

        if (form is not null)
        {
            form.Questions = inMemoryStorage.Value.Questions.Where(
                t => t.FormId == formId
            ).ToList();
        }

        return DeepClone(form);
    }

    public IQuestionRepository GetQuestionRepository()
    {
        return new QuestionRepository(inMemoryStorage.Value);
    }

    public IFormRepository GetFormRepository()
    {
        return new FormRepository(inMemoryStorage.Value);
    }

    private Storage CreateInMemoryStorage()
    {
        var storage = new Storage();
        return storage;
    }

    private T? DeepClone<T>(T input)
    {
        var json = JsonConvert.SerializeObject(input);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
