using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.DAL.Memory;

public class Storage
{
    private readonly IList<Guid> answerGuids = new List<Guid>
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

    private readonly IList<Guid> questionGuids = new List<Guid>
    {
        new("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
        new("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
        new("2caa29d8-61f0-4c1d-850d-4d70003e6aef"),
        new("c3542130-589c-4302-a441-a110fcadd45a"),
    };

    private readonly IList<Guid> formGuids = new List<Guid>
    {
        new("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        new("a8ee7ce8-9903-4f42-afb4-b2c34dfb7ccf"),
    };

    private readonly IList<Guid> userGuids = new List<Guid>
    {
        new("d57fa919-64e4-4b9c-b25e-bbd12120eb42"),
        new("342f4698-47b0-4a7a-a654-7e484cb55815"),
    };

    public IList<AnswerEntity> Answers { get; } = new List<AnswerEntity>();
    public IList<QuestionEntity> Questions { get; } = new List<QuestionEntity>();
    public IList<FormEntity> Forms { get; } = new List<FormEntity>();

    public Storage(bool seedData = true)
    {
        if (seedData)
        {
            SeedAnswers();
            SeedQuestions();
            SeedForms();
        }
    }

    private void SeedAnswers()
    {
        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[0],
            Text = "Apple",
            ResponderId = userGuids[0],
            QuestionId = questionGuids[0],
        });

        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[1],
            Text = "Carrot",
            ResponderId = userGuids[1],
            QuestionId = questionGuids[0],
        });

        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[2],
            Text = "Eggplant",
            ResponderId = userGuids[0],
            QuestionId = questionGuids[1],
        });

        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[3],
            Text = "Cherry",
            ResponderId = userGuids[1],
            QuestionId = questionGuids[1],
        });

        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[4],
            Text = "Arch Linux",
            QuestionId = questionGuids[2],
        });

        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[5],
            Text = "WiNDowS",
            QuestionId = questionGuids[2],
        });

        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[6],
            Text = "Yes",
            QuestionId = questionGuids[3],
        });

        Answers.Add(new AnswerEntity
        {
            Id = answerGuids[7],
            Text = "Definitely yes",
            QuestionId = questionGuids[3],
        });
    }

    private void SeedQuestions()
    {
        Questions.Add(new QuestionEntity
        {
            Id = questionGuids[0],
            QuestionType = IW5Forms.Common.Enums.QuestionTypes.TextAnswer,
            Text = "What company is called after fruit?",
            Description = "Really easy question...",
            FormId = formGuids[0],
        });

        Questions.Add(new QuestionEntity
        {
            Id = questionGuids[1],
            QuestionType = IW5Forms.Common.Enums.QuestionTypes.ManyOptions,
            Text = "Which of the following is not a vegetable?",
            Options = ["Carrot", "Cabbage", "Eggplant", "Cherry"],
            FormId = formGuids[0],
        });

        Questions.Add(new QuestionEntity
        {
            Id = questionGuids[2],
            QuestionType = IW5Forms.Common.Enums.QuestionTypes.TextAnswer,
            Text = "What OS are you using?",
            FormId = formGuids[1],
        });

        Questions.Add(new QuestionEntity
        {
            Id = questionGuids[3],
            QuestionType = IW5Forms.Common.Enums.QuestionTypes.ManyOptions,
            Text = "Is Linux better than other OS?",
            Options = ["Yes", "Definitely yes", "100%!", "Absolutely"],
            FormId = formGuids[1],
        });
    }

    private void SeedForms()
    {
        Forms.Add(new FormEntity
        {
            Id = formGuids[0],
            Name = "Fruit and vegetables",
            BeginTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(7),
            Incognito = false,
            SingleTry = true,
        });

        Forms.Add(new FormEntity
        {
            Id = formGuids[1],
            Name = "Linux better",
            BeginTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(7),
            Incognito = true,
            SingleTry = false,
        });
    }
}
