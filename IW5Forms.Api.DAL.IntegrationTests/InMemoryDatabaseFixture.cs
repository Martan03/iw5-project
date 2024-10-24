using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Api.DAL.Memory;
using IW5Forms.Api.DAL.Memory.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.DAL.IntegrationTests;

public class InMemoryDatabaseFixture : IDatabaseFixture
{
    public IList<Guid> AnswerGuids => throw new NotImplementedException();
    public IList<Guid> QuestionGuids => throw new NotImplementedException();
    public IList<Guid> FormGuids => throw new NotImplementedException();
    public IList<Guid> UserGuids => throw new NotImplementedException();

    private readonly Lazy<Storage> inMemoryStorage;

    public InMemoryDatabaseFixture()
    {
        inMemoryStorage = new Lazy<Storage>(CreateInMemoryStorage);
    }

    public QuestionEntity? GetQuestionDirectly(Guid questionId)
    {
        var question = inMemoryStorage.Value.Questions.SingleOrDefault(
            t => t.Id == questionId
        );
        return DeepClone(question);
    }

    public FormEntity? GetFormDirectly(Guid formId)
    {
        var form = inMemoryStorage.Value.Forms.SingleOrDefault(
            t => t.Id == formId
        );
        if (form is null)
        {
            return null;
        }

        form.Questions = inMemoryStorage.Value.Questions.Where(
            t => t.FormId == formId
        ).ToList();
        return DeepClone(form);
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
