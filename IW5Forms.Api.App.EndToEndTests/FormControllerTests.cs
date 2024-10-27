
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

   

    


}