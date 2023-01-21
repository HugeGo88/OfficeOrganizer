using System.Text;
using Markdig;
using Markdig.Renderers;
using Microsoft.UI.Xaml.Controls;
using OfficeOrganizer.ViewModels;

namespace OfficeOrganizer.Views;
public sealed partial class WriterPage : Page
{
    public WriterViewModel ViewModel
    {
        get;
    }

    string indexHtmlContent = string.Empty;

    public WriterPage()
    {
        ViewModel = App.GetService<WriterViewModel>();
        InitializeComponent();
    }

    private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var indexHtml = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\HtmlTemplates\index.html");
        if (String.IsNullOrEmpty(indexHtmlContent))
        {
            indexHtmlContent = File.ReadAllText(indexHtml);
        }
        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        MarkdownPipeline? pipeline = null;
        pipeline = pipeline ?? new MarkdownPipelineBuilder().UseAdvancedExtensions().UsePipeTables().Build();
        pipeline.Setup(renderer);
        StringBuilder html = new();
        html.Append(indexHtmlContent);
        html.Replace("{CONTENT}", Markdig.Markdown.ToHtml(ContentBox.Text, pipeline));
        await WebView.EnsureCoreWebView2Async();
        WebView.NavigateToString(html.ToString());
    }
}
