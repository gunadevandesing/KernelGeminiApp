// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using  Microsoft.SemanticKernel.Connectors.Google;

public class Program
{
    const string ModelId = "gemini-1.5-flash";
    // static readonly string ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new ArgumentNullException("GEMINI_API_KEY environment variable is not set.");
    static readonly string ApiKey = "";
    
    static async Task Main(){
        #pragma warning disable SKEXP0070
        var builder =  Kernel.CreateBuilder().AddGoogleAIGeminiChatCompletion(ModelId, ApiKey);

        Kernel kernel = builder.Build();
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        //Settings
        GeminiPromptExecutionSettings settings =  new(){
            Temperature = 0.8,
            MaxTokens= 8192
        };

        var chatHistory = new ChatHistory();

        //Initiate chat
        string? userInput;
        while(true){
            Console.Write("User >> ");
            userInput = Console.ReadLine();

            if(userInput == null){
                break;
            }

            //Add user input
            chatHistory.AddUserMessage(userInput);

            var result = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                executionSettings: settings,
                kernel: kernel
            );

            //Print the results
            Console.WriteLine("Assistant >> "+ result);

            //add  assistant response to chat history
            chatHistory.AddMessage(result.Role, result.Content ?? string.Empty);

        }
    }
 }

