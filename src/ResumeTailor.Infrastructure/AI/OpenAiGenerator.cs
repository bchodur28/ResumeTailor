#pragma warning disable OPENAI001

using Microsoft.Extensions.Options;
using OpenAI.Responses;
using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Application.Common.Models;
using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResumeTailor.Infrastructure.AI;

public sealed class OpenAiGenerator<T>(ResponsesClient client, IOptions<OpenAIOptions> options) : IAiGenerator<T>
{
    private const decimal InputCostPerMillionTokens = 2.00m;
    private const decimal OutputCostPerMillionTokens = 12.00m;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = {new JsonStringEnumConverter()}
    };

    public async Task<AiResult<T>> GenerateAsync(AiContext context, CancellationToken cancellationToken = default)
    {
        var responseOptions = new CreateResponseOptions
        {
            Model = options.Value.Model,
            TextOptions = new ResponseTextOptions
            {
                TextFormat = ResponseTextFormat.CreateJsonSchemaFormat(context.jsonSchemaFormatName, BinaryData.FromString(context.jsonSchema), jsonSchemaIsStrict: true)
            }
        };

        responseOptions.InputItems.Add(ResponseItem.CreateUserMessageItem(context.prompt));

        var response = await client.CreateResponseAsync(responseOptions, cancellationToken);
        if (response.Value.Status != ResponseStatus.Completed)
        {
            throw new InvalidOperationException($"OpenAI response was not completed. Status: {response.Value.Status}");
        } 

        var json = response.Value.GetOutputText();
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException("OpenAI returned no output text.");
        }

        var aiResponse = JsonSerializer.Deserialize<T>(json, SerializerOptions)
            ?? throw new InvalidOperationException("OpenAI returned an empty response.");

        return new AiResult<T>(MetaData: CreateAiMetaData(response),Value: aiResponse);
    }

    private static AiMetaData CreateAiMetaData(ClientResult<ResponseResult> response)
    {
        var inputTokens = response.Value.Usage.InputTokenCount;
        var outputTokens = response.Value.Usage.OutputTokenCount;
        var totalTokens = response.Value.Usage.TotalTokenCount;

        var estimatedCost = CalculateEstimatedCost(inputTokens, outputTokens);

        return new AiMetaData(
            Model: response.Value.Model,
            InputTokens: inputTokens,
            OutputTokens: outputTokens,
            TotalTokens: totalTokens,
            Cost: estimatedCost
        );
    }

    private static decimal CalculateEstimatedCost(int inputTokens,int outputTokens)
    {
        var inputCost =
            inputTokens / 1_000_000m * InputCostPerMillionTokens;

        var outputCost =
            outputTokens / 1_000_000m * OutputCostPerMillionTokens;

        return inputCost + outputCost;
    }
}
#pragma warning restore OPENAI001
