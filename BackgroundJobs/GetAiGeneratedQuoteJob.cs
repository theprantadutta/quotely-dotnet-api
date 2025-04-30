namespace quotely_dotnet_api.BackgroundJobs;

public class GetAiGeneratedQuoteJob
{
    //     public async Task Invoke()
    // {
    //     _logger.LogInformation(
    //         "Starting AI-powered GetAllQuoteJob at {StartTime}...",
    //         DateTime.Now.ToCustomLogDateFormat()
    //     );
    //
    //     try
    //     {
    //         await Lock.WaitAsync();
    //
    //         // 1. Get existing authors to associate quotes
    //         var existingAuthors = await _appDbContext.Authors
    //             .Where(a => a.IsAiGenerated || a.QuoteCount > 0)
    //             .ToListAsync();
    //
    //         if (!existingAuthors.Any())
    //         {
    //             _logger.LogWarning("No authors found - run GetAllAuthorJob first");
    //             return;
    //         }
    //
    //         // 2. Generation parameters
    //         var quotesPerAuthor = 3; // Quotes per author
    //         var batchesPerRun = 5;   // Authors per job execution
    //
    //         // 3. Process in batches
    //         foreach (var authorBatch in existingAuthors.Chunk(batchesPerRun))
    //         {
    //             _logger.LogInformation(
    //                 "Processing batch of {Count} authors: {Authors}",
    //                 authorBatch.Length,
    //                 string.Join(", ", authorBatch.Select(a => a.Name))
    //             );
    //
    //             foreach (var author in authorBatch)
    //             {
    //                 await ProcessAuthorQuotes(author, quotesPerAuthor);
    //                 await Task.Delay(1500); // Rate limiting
    //             }
    //
    //             await _appDbContext.SaveChangesAsync();
    //             await Task.Delay(3000); // Between batches
    //         }
    //
    //         _logger.LogInformation(
    //             "Generated {TotalQuotes} quotes across {AuthorCount} authors",
    //             existingAuthors.Sum(a => quotesPerAuthor),
    //             existingAuthors.Count
    //         );
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, "AI quote generation failed");
    //     }
    //     finally
    //     {
    //         Lock.Release();
    //         _logger.LogInformation(
    //             "Finished at {EndTime}",
    //             DateTime.Now.ToCustomLogDateFormat()
    //         );
    //     }
    // }
    //
    // private async Task ProcessAuthorQuotes(Author author, int count)
    // {
    //     var prompt = $"""
    //         Generate {count} authentic-sounding quotes by {author.Name} ({author.Era}),
    //         author of {author.FamousWork}. Follow these rules:
    //
    //         1. Each quote should sound like their real work
    //         2. Maximum 25 words per quote
    //         3. Include 3-5 thematic tags per quote
    //         4. Never duplicate existing famous quotes
    //
    //         Format response as:
    //         {{
    //             "quotes": [
    //                 {{
    //                     "content": "To be or not to be...",
    //                     "tags": ["existentialism", "doubt", "shakespeare"],
    //                     "length": 42
    //                 }}
    //             ]
    //         }}
    //         """;
    //
    //     try
    //     {
    //         var response = await _openAiClient.PostAsJsonAsync("chat/completions", new
    //         {
    //             model = "gpt-4",
    //             messages = new[] { new { role = "user", content = prompt } },
    //             response_format = new { type = "json_object" },
    //             temperature = 0.7 // For creative but consistent output
    //         });
    //
    //         var result = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
    //         var quotes = result?.Choices[0].Message.Content;
    //
    //         if (quotes != null)
    //         {
    //             var quoteData = JsonSerializer.Deserialize<QuoteResponse>(quotes);
    //             await SaveQuotesForAuthor(author, quoteData.Quotes);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogWarning(ex, "Failed to generate quotes for {Author}", author.Name);
    //     }
    // }
    //
    // private async Task SaveQuotesForAuthor(Author author, List<QuoteDto> quotes)
    // {
    //     foreach (var quote in quotes)
    //     {
    //         // Deduplication check
    //         var quoteHash = GenerateQuoteHash(quote.Content, author.Id);
    //         var exists = await _appDbContext.Quotes
    //             .AnyAsync(q => q.ContentHash == quoteHash);
    //
    //         if (!exists)
    //         {
    //             var newQuote = new Quote
    //             {
    //                 Id = Guid.NewGuid().ToString(),
    //                 Content = quote.Content,
    //                 ContentHash = quoteHash,
    //                 Author = author.Name,
    //                 AuthorSlug = author.Slug,
    //                 Length = quote.Content.Length,
    //                 Tags = quote.Tags,
    //                 IsAiGenerated = true,
    //                 DateAdded = DateTime.UtcNow,
    //                 DateModified = DateTime.UtcNow
    //             };
    //
    //             await _appDbContext.Quotes.AddAsync(newQuote);
    //             author.QuoteCount++;
    //         }
    //     }
    // }
    //
    // private string GenerateQuoteHash(string content, string authorId)
    // {
    //     var normalized = $"{content.Trim().ToLower()}|{authorId}";
    //     using var sha256 = SHA256.Create();
    //     return Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(normalized)));
    // }
    //
    // // Supporting classes
    // private record OpenAiResponse(List<Choice> Choices);
    // private record Choice(Message Message);
    // private record Message(string Content);
    // private record QuoteResponse(List<QuoteDto> Quotes);
    // private record QuoteDto(string Content, List<string> Tags, int Length);
}
