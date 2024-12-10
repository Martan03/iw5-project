using IW5Forms.Common.BL.Facades;
using IW5Forms.Common.Models.Question;
using IW5Forms.Common.Models.User;

namespace IW5Forms.Web.BL.Facades;

public class SearchFacade : IAppFacade
{
    private readonly ISearchApiClient apiClient;

    public SearchFacade(ISearchApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public async Task<List<UserListModel>> GetAllUsersByNameAsync(string name)
    {
        List<UserListModel> users = [];
        var apiUsers = await apiClient.UserAsync(name);

        if (apiUsers != null)
            users.AddRange(apiUsers);

        return users;
    }

    public async Task<List<QuestionListModel>> GetAllQuestionsByTextAsync(string text)
    {
        List<QuestionListModel> questions = [];
        var apiQuestions = await apiClient.TextAsync(text);

        if (apiQuestions != null)
            questions.AddRange(apiQuestions);

        return questions;
    }

    public async Task<List<QuestionListModel>> GetAllQuestionsByDescriptionAsync(string description)
    {
        List<QuestionListModel> questions = [];
        var apiQuestions = await apiClient.DescriptionAsync(description);

        if (apiQuestions != null)
            questions.AddRange(apiQuestions);

        return questions;
    }
}
