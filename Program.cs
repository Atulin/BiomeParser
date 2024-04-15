using System.Diagnostics;
using System.Net;
using System.Text;

Console.Write("Enter file path: ");
var path = Console.ReadLine() ?? "";

var time = new Stopwatch();
time.Start();

var lines = File.ReadAllLines(path, Encoding.UTF8);

Console.WriteLine($"Found {lines.Length} lines");

const string style = """
     body {
        display: flex;
        flex-direction: column;
        font-family: sans-serif;
     }
     h2 {
     	font-size: 110%;
     }
     .codeblock {
     	padding: .5rem;
     	margin: .5rem 0;
     	display: flex;
     	flex-direction: column;
     	background-color: rgba(0,0,0,0.01);
     	border: 1px solid rgba(0,0,0,0.18);
     }
     .line {
     	display: inline;
     	
     	&.green {
     		color: green;
     		background-color: rgba(0,128,0,0.2);
     		i {
     			background-color: green;
     		}
     	}
     	
     	&.red {
     		color: red;
     		background-color: rgba(255,0,0,0.2);
     		i {
     			background-color: red;
     		}
     	}
     
     	i {
     		display: inline-block;
     		width: 1px;
     		height: 10px;
     		background-color: black;
     	}
     	* {
     		display: inline;
     	}
     }
     .msg {
     	display: flex;
     	flex-direction: row;
     	&.warn {
     		color: rgb(255,145,0);
     	}
     	&.info {
     		color: rgb(0,0,255);
     	}
     	.sig {
     		display: block;
     		width: 1.5rem;
     		text-align: center;
     	}
     }
     """;

var sb = new StringBuilder();
sb.AppendLine($"""
	<html lang='en'>
	<head>
		<meta charset="UTF-8">
		<title>Biome report</title>
		<style>
		{style}
		</style>
	</head>
	<body>
	""");

var codeblock = false;
var count = 1;
foreach (var line in lines)
{
	Console.Write($"{count++}/{lines.Length}");
	Console.SetCursorPosition(0, Console.CursorTop);
	
	if (string.IsNullOrWhiteSpace(line))
	{
		continue;
	}
	
	if (!line.StartsWith(' '))
	{
		if (codeblock)
		{
			codeblock = false;
			sb.AppendLine("</div>");
		}
		sb.AppendLine($"<h2>{line.E()}</h2>");
	}
	else if (line.Contains('\u2502') && line.Split('\u2502') is [var pre, var post])
	{
		if (!codeblock)
		{
			codeblock = true;
			sb.AppendLine("<div class='codeblock'>");
		}
		var color = post.TrimStart().FirstOrDefault() switch
		{
			'+' => "green",
			'-' => "red",
			_ => ""
		};

		sb.AppendLine($"""
		               <div class='line {color}'>
		                   <pre class='pre'>{pre.E()}</pre>
		                   <i></i>
		                   <pre class='post'>{post.E()}</pre>
		               </div>
		               """);
	}
	else if (line is [' ', ' ', var sign, ' ', ..])
	{
		if (codeblock)
		{
			codeblock = false;
			sb.AppendLine("</div>");
		}
		sb.AppendLine(sign switch
		{
			'i' => $"<span class='msg info'><span class='sig'>{sign}</span>{line[3..].E()}</span>",
			'\u00d7' => $"<span class='msg warn'><span class='sig'>{sign}</span>{line[3..].E()}</span>",
			_ => $"<span class='msg'>{line.E()}</span>"
		});
	}
	else
	{
		if (codeblock)
		{
			codeblock = false;
			sb.AppendLine("</div>");
		}
		sb.AppendLine($"<span>{line.E()}</span>");
	}

}

sb.AppendLine("</body></html>");

File.WriteAllText("./diag.html", sb.ToString(), Encoding.UTF8);

time.Stop();
Console.WriteLine($"Completed in {time.ElapsedMilliseconds} milliseconds");

internal static class Helpers
{
	public static string E(this string s) => WebUtility.HtmlEncode(s);
}
