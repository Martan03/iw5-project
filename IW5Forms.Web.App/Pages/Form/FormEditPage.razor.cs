using IW5Forms.Common.Models.Form;
using IW5Forms.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace IW5Forms.Web.App.Pages;

public partial class FormEditPage
{
    [Inject]
    private NavigationManager navigationManager { get; set; } = null!;

    [Inject]
    private FormFacade FormFacade { get; set; } = null!;

    private FormDetailModel Data { get; set; } = GetNewForm();

    [Parameter]
    public Guid Id { get; init; }

    private DateTime? BeginDate
    {
        get => Data.BeginTime;
        set => Data.BeginTime = value ?? Data.BeginTime;
    }

    private TimeSpan? BeginTime
    {
        get => Data.BeginTime.TimeOfDay;
        set => Data.BeginTime = value is not null ?
            Data.BeginTime.Date.Add(value.Value) : Data.BeginTime;
    }

    private DateTime? EndDate
    {
        get => Data.EndTime;
        set => Data.EndTime = value ?? Data.EndTime;
    }

    private TimeSpan? EndTime
    {
        get => Data.EndTime.TimeOfDay;
        set => Data.EndTime = value is not null ?
            Data.EndTime.Date.Add(value.Value) : Data.EndTime;
    }

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            Data = await FormFacade.GetByIdAsync(Id);
        }

        await base.OnInitializedAsync();
    }

    public async Task Save()
    {
        await FormFacade.SaveAsync(Data);
        navigationManager.NavigateTo($"/forms");
    }

    public async Task Delete()
    {
        await FormFacade.DeleteAsync(Id);
        navigationManager.NavigateTo($"/forms");
    }

    private static FormDetailModel GetNewForm() => new() {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        BeginTime = DateTime.Now,
        EndTime = DateTime.Now.AddDays(30),
        Incognito = false,
        SingleTry = true,
    };
}
