
using System.Net;
using System.Net.Http.Json;
using IW5Forms.Common.Models.Form;
using Microsoft.VisualBasic;

namespace IW5Forms.Api.App.EndToEndTests;

public class FormControllerTests : IAsyncDisposable
{
    private readonly IW5FormsApiApplicationFactory app;
    private readonly Lazy<HttpClient> client;

    public FormControllerTests()
    {
        app = new IW5FormsApiApplicationFactory();
        client = new Lazy<HttpClient>(app.CreateClient());
    }

    public async ValueTask DisposeAsync()
    {
        await app.DisposeAsync();
    }

    [Fact]
    public async void GetAllForms_Returns_At_Least_One_Form()
    {
        var response = await client.Value.GetAsync("/api/form");

        response.EnsureSuccessStatusCode();

        var forms = await response
            .Content
            .ReadFromJsonAsync<ICollection<FormListModel>>();
        Assert.NotNull(forms);
        Assert.NotEmpty(forms);
    }

    [Fact]
    public async void GetFormById_Returns_Form_If_Exists()
    {
        var formId = new Guid("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e");

        var response = await client.Value.GetAsync($"/api/form/{formId}");
        response.EnsureSuccessStatusCode();

        var form = await response.Content.ReadFromJsonAsync<FormDetailModel>();
        Assert.NotNull(form);
        Assert.Equal(formId, form.Id);
    }

    [Fact]
    public async void GetFormById_Returns_Not_Found_If_Does_Not_Exist()
    {
        var formId = Guid.NewGuid();

        var response = await client.Value.GetAsync($"/api/form/{formId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async void CreateForm_Create_New_Form()
    {
        var newForm = new FormDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "This is form testing",
            BeginTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(1),
            Incognito = false,
            SingleTry = true,
        };

        var response =
            await client.Value.PostAsJsonAsync("/api/form", newForm);
        response.EnsureSuccessStatusCode();

        var createdForm =
            await response.Content.ReadFromJsonAsync<FormDetailModel>();
        Assert.NotNull(createdForm);
        Assert.Equal(newForm.Id, createdForm.Id);
        Assert.Equal(
            newForm.EndTime,
            createdForm.EndTime
        );
    }

    [Fact]
    public async void UpdateForm_Updates_Form_Details()
    {
        var formId = new Guid("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e");
        var updatedForm = new FormDetailModel
        {
            Id = formId,
            Name = "This is form testing",
            BeginTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(3),
            Incognito = false,
            SingleTry = true,
        };

        var response = await client.Value.PutAsJsonAsync(
            $"/api/form/{formId}",
            updatedForm
        );
        response.EnsureSuccessStatusCode();

        var form =
            await response.Content.ReadFromJsonAsync<FormDetailModel>();
        Assert.NotNull(form);
        Assert.Equal(updatedForm.EndTime, form.EndTime);
    }

    [Fact]
    public async void DeleteForm_Deletes_Existing_Form()
    {
        var formId = new Guid("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e");

        var response = await client.Value.DeleteAsync($"/api/form/{formId}");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await client.Value.GetAsync($"/api/form/{formId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}