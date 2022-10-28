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

    public WriterPage()
    {
        ViewModel = App.GetService<WriterViewModel>();
        InitializeComponent();
    }

    private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        MarkdownPipeline pipeline = null;
        pipeline = pipeline ?? new MarkdownPipelineBuilder().UseAdvancedExtensions().UsePipeTables().Build();
        pipeline.Setup(renderer);
        StringBuilder letterContent = new();
        StringBuilder html = new();
        html.Append(" <style type=\"text/css\">\r\n        :root {\r\n            --cvjmRed: #ED1443;\r\n            --cvjmRedMark: #ED1443A0\r\n        }\r\n\r\n        body {\r\n            zoom: 1;\r\n            font-family: 'Source Sans Pro', sans-serif;\r\n        }\r\n\r\n        .page-header, .page-header-space {\r\n            height: 150px;\r\n            width: 100%;\r\n        }\r\n\r\n        .page-footer, .page-footer-space {\r\n            height: 150px;\r\n            width: 100%;\r\n        }\r\n\r\n        .page-footer {\r\n            position: fixed;\r\n            bottom: 0mm;\r\n            left: 0mm;\r\n            right: 0;\r\n            font-size: 7pt;\r\n            line-height: 7pt;\r\n        }\r\n\r\n        .page-header {\r\n            position: fixed;\r\n            top: 11.7mm;\r\n            margin: 0;\r\n            width: 100%;\r\n            font-size: 7pt;\r\n            line-height: 7pt;\r\n        }\r\n\r\n        .page {\r\n            page-break-after: always;\r\n            width: 100%;\r\n            font-family: 'Source Sans Pro', sans-serif;\r\n            font-size: 11pt;\r\n            line-height: 14pt;\r\n        }\r\n\r\n        .footer-tabel {\r\n            margin-left: 25mm;\r\n            text-align: left;\r\n            margin-bottom: 11.7mm;\r\n        }\r\n\r\n        @page {\r\n            size: A4;\r\n            margin: 0%;\r\n        }\r\n\r\n        @media print {\r\n            thead {\r\n                display: table-header-group;\r\n            }\r\n\r\n            tfoot {\r\n                display: table-footer-group;\r\n            }\r\n\r\n            body {\r\n                margin: 0%;\r\n                zoom: 1;\r\n            }\r\n\r\n            .pagebreak {\r\n                clear: both;\r\n                page-break-after: always;\r\n            }\r\n        }\r\n\r\n        .grid-container {\r\n            display: grid;\r\n            grid-template-columns: auto auto;\r\n        }\r\n\r\n        a {\r\n            color: var(--cvjmRed);\r\n            text-decoration: none;\r\n        }\r\n\r\n        h1 {\r\n            font-weight: lighter;\r\n            font-size: 20pt;\r\n            line-height: 25pt;\r\n        }\r\n\r\n        h2 {\r\n            font-weight: bold;\r\n            font-size: 16pt;\r\n            line-height: 20pt;\r\n            color: var(--cvjmRed);\r\n        }\r\n\r\n        h3 {\r\n            font-weight: bold;\r\n            font-size: 13pt;\r\n            line-height: 1.25;\r\n        }\r\n\r\n        h4, h5, h6 {\r\n            font-weight: bold;\r\n            font-size: 11pt;\r\n            line-height: 1.25;\r\n        }\r\n\r\n        blockquote {\r\n            position: relative;\r\n            padding: 25px;\r\n            font-size: 13pt;\r\n            font-weight: bold;\r\n            width: auto;\r\n            display: table;\r\n            quotes: \"\\201C\"\"\\201D\"\"\\2018\"\"\\2019\"\r\n        }\r\n\r\n            blockquote:before {\r\n                position: absolute;\r\n                color: var(--cvjmRed);\r\n                content: open-quote;\r\n                font-size: 5em;\r\n                top: 25px;\r\n                right: -20px;\r\n            }\r\n\r\n            blockquote:after {\r\n                position: absolute;\r\n                color: var(--cvjmRed);\r\n                content: close-quote;\r\n                font-size: 5em;\r\n                bottom: 0px;\r\n                left: -20px;\r\n            }\r\n\r\n            blockquote p {\r\n                display: inline;\r\n            }\r\n\r\n        cite{\r\n            font-weight: lighter;\r\n        }\r\n\r\n        mark {\r\n            background-color: var(--cvjmRedMark);\r\n        }\r\n\r\n        code {\r\n            font-size: 9pt;\r\n        }\r\n\r\n        ul {\r\n            list-style-type: square !important;\r\n        }\r\n\r\n        .section-1 table {\r\n            width: 100%;\r\n            border-collapse: collapse;\r\n        }\r\n\r\n        .section-1 tr:nth-child(even) {\r\n            background: #CCC\r\n        }\r\n\r\n        .section-1 tr:nth-child(odd) {\r\n            background: #FFF\r\n        }\r\n    </style>");
        html.Append(Markdig.Markdown.ToHtml(ContentBox.Text, pipeline));
        await WebView.EnsureCoreWebView2Async();
        WebView.NavigateToString(html.ToString());
    }
}
