using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using IW5Forms.Common.Models.User;
using IW5Forms.Web.App.Pages.Search;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IW5Forms.Web.App.Pages;

public partial class SearchPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private SearchFacade SearchFacade { get; set; } = null!;
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;
    [Inject]
    private UserFacade UserFacade { get; set; } = null!;

    private List<SearchAbleEntity> searchEntity { get; set; } = [];
    private List<QuestionListModel> Questions { get; set; } = [];
    private List<UserListModel> Users { get; set; } = [];

    private bool isUser = false;
    private bool isQuestionText = false;
    private bool searchPerfomed = false;
    private string searchQuery = string.Empty;
    private string selectedCriteria = "Text";

    private async void PerformSearch()
    {
        searchEntity = [];
        searchPerfomed = true;
        switch (selectedCriteria)
        {
            case "Text":
                isUser = false;
                isQuestionText = true;
                Questions = await SearchFacade.GetAllQuestionsByTextAsync(searchQuery);
                searchEntity.AddRange(Questions.Select(ques => new SearchAbleEntity() { Id = ques.Id, NameOrText = ques.Text}));
                break;
            case "Description":
                isUser = false;
                isQuestionText = false;
                Questions = await SearchFacade.GetAllQuestionsByDescriptionAsync(searchQuery);
                searchEntity.AddRange(Questions.Select(ques => new SearchAbleEntity() { Id = ques.Id, NameOrText = ques.Description }));
                break;
            default:
                isUser = true;
                isQuestionText = false;
                Users = await SearchFacade.GetAllUsersByNameAsync(searchQuery);
                searchEntity.AddRange(Users.Select(user => new SearchAbleEntity() { Id = user.Id, NameOrText = user.Name }));
                break;
        }
        StateHasChanged();
    }

    public async Task DeleteEntityAsync(Guid id)
    {
        if (isUser)
            await UserFacade.DeleteAsync(id);
        else
            await QuestionFacade.DeleteAsync(id);

        var ent = searchEntity.FirstOrDefault(ent => ent.Id == id);
        if (ent != null)
            searchEntity.Remove(ent);
    }

    public void NavDetail(Guid id)
    {
        if(isUser)
            navigationManager.NavigateTo($"/users/{id}");
        else
            navigationManager.NavigateTo($"/question/{id.ToString() ?? ""}");
    }

    public void NavEditor(Guid? id)
    {
        if (isUser)
            navigationManager.NavigateTo($"/users/editor/{id?.ToString() ?? ""}");
        else
            navigationManager.NavigateTo($"/question/edit/{id?.ToString() ?? ""}");
    }

    public async Task Delete(Guid id)
    {
        await DeleteEntityAsync(id);
        StateHasChanged();
    }

}
